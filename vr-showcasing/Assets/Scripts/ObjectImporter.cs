/**************************************************
 * ObjectImporter klassen leser .obj og .mtl-filer
 * og importerer dem i Unity som mesh som til sammen
 * lager et 3D-modell med materialer og teksturer
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
public class ObjectImporter : MonoBehaviour
{
    /**Globale variabler som benyttes gjennom hele koden**/
    public float scale = 0.01f; 
    public bool hasTexture;

    public static bool inProgress = false;

    string objFileName;
    string mtlFileName;
    string objectName;
    int subMeshCounter = -1;
    public int linesPerFrame = 1200;
    /**Mesh klassen som benyttes for oppretelse av 3D-meshes**/
    Mesh mesh;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    /**GameObject klassen som opretter spillobjekter under oppretelse av mesh**/
    GameObject go;
    GameObject parent;
    /**Lister for OBJ filer**/
    List<Vector3> verticesList = new List<Vector3>();
    List<Vector3> normalesList = new List<Vector3>();
    List<Vector2> textureList = new List<Vector2>();
    /**Lister som holder indeksering av overnevnte lister for oppretelse av trekanter, og tildelig av teksturer**/
    List<int[]> globalVerticeIndexes = new List<int[]>();
    List<int[]> globalTextureIndexes = new List<int[]>();
    /**Lister som brukes til å dele ut trekantverdier til Mesh klassen**/
    List<int> triangles = new List<int>();
    List<int[]> fixedTrianglesList = new List<int[]>();
    /**Lister som brukes til å tildele riktige materialer til 3D-objektene**/
    List<string> materialNameOBJ = new List<string>();
    List<Material> addMaterials = new List<Material>();
    /**Når teksturer benyttes, vil innholdet i disse listene brukes for trekanter**/
    List<Vector3> fixedVerticeList = new List<Vector3>();
    List<Vector2> fixedTextureList = new List<Vector2>();
   
    /**For hvert nytt 3D-modell som importeres, så slettes gamle innholdet i listene, denne metoden sørger for dette**/
    void clearLists()
    {
        verticesList.Clear();
        normalesList.Clear();
        textureList.Clear();
        globalVerticeIndexes.Clear();
        globalTextureIndexes.Clear();
        triangles.Clear();
        fixedTrianglesList.Clear();
        materialNameOBJ.Clear();
        fixedVerticeList.Clear();
        fixedTextureList.Clear();
        subMeshCounter = -1;
    }

    /**Basert på filstien som fåes fra filebrowser, så opprettes det navn på .obj filen, og utifra dette oppretes også navn på .mtl filen**/
    void Path(string s)
    {
        objFileName = s.Replace("\\", "/");  
        mtlFileName = s.Replace(".obj", ".mtl");
        //hvis det ikke eksisterer noe .mtl fil, sett navnet til null, som senere brukes for å 
        if (!File.Exists(mtlFileName))
            mtlFileName = null;
    }

    /**Metode for å fikse tekst innholdet for forskjellige verdier**/
    string[] fixString(string input, string remove)
    {
        string inputCpy = input;
        inputCpy = inputCpy.Replace(remove, "");
        if (remove == "f")
            inputCpy = inputCpy.Replace("/", " ");
        inputCpy = inputCpy.TrimStart();

        int removeComments = inputCpy.IndexOf("#");
        if (remove == "Kd" || remove == "Ks")
            removeComments = inputCpy.IndexOf("/");

        if (removeComments > 0)
            inputCpy = inputCpy.Substring(0, removeComments);

        string[] returnString = inputCpy.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);

        return returnString;

    }

    /**Metoden som kjøres når 3D-modellen har teksturer, grunnet det er lagt i egen metode er fordi behandlig av trekanter er forskjellig**/
    IEnumerator objectReadTexture()
    {
        //sletter innholdet i alle listene for å kunne oprette nytt 3D-modell
        clearLists();
        /**Tellere som brukes underveis**/
        int fValuesCounter = 0;
        int yieldReturnCounter = 0;
        int vListCounter = 0;
        int tListCounter = 0;
        int objectsCounter = 0;
        string objectNameFromObjFile = "";
        /**Åpner .obj filen og starter å lese den linje for linje**/
        using (FileStream file = File.OpenRead(objFileName))
        using (var reader = new StreamReader(file))
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                //øker verdien på telleren for coroutine
                yieldReturnCounter++;
                if (line == "")
                    continue;
                
                if (line[0] == 'v')
                {
                    /**Verdiene av vertices blir lagret her**/
                    if (line[1] == ' ')
                    {
                        #region Vertices
                        //Verdiene fra innholdet i teksten lagres i en tekst-tabell uten nøkkelord
                        string[] test3 = fixString(line, "v");
                        //Innholdet i tekst-tabellen konverteres til en float tabell
                        float[] doubles = Array.ConvertAll(test3, delegate (string s) { return float.Parse(s); });
                        //Verdiene lagres så i en global Vector3-liste
                        Vector3 vertices = new Vector3(doubles[0], doubles[1], doubles[2]);
                        verticesList.Add(vertices);
                        #endregion
                    }

                    /**Verdiene av normaler lagres her**/
                    else if (line[1] == 'n')
                    {
                        #region Normales
                        string[] test3 = fixString(line, "vn");
                        float[] doubles = Array.ConvertAll(test3, delegate (string s) { return float.Parse(s); });
                        Vector3 vertices = new Vector3(doubles[0], doubles[1], doubles[2]);

                        normalesList.Add(vertices);

                        #endregion
                    }

                    /**Tekstur koordinater lagres her**/
                    else if (line[1] == 't')
                    {
                        #region TextureUVS
                        string[] texts = fixString(line, "vt");
                        float[] doubles = Array.ConvertAll(texts, delegate (string s) { return float.Parse(s); });
                        Vector2 vertices = new Vector3(doubles[0], doubles[1]);

                        textureList.Add(vertices);

                        #endregion
                    }
                }

                /**For hver f som forekommer under lesing av .obj filen. f verdien i .obj filen grupperer sammen vertices, normaler og/eller teksturer**/
                else if (line[0] == 'f')
                {

                    int dele = 1;
                    string test3 = line.Replace("f", "").TrimStart();
                    
                    //Løkke som brukes for å bestemme hvliken verdier som er med i f
                    // det kan være v,vn og vt, men må ikke være alle 3
                    for (int i = 0; i < test3.Length; i++)
                    {
                        if (test3[i] == '/')
                        {
                            if (test3[i + 1] == '/')
                                dele = 2;
                            else if (test3[i + 1] != '/')
                                dele = 3;
                            break;
                        }
                    }

                    //fjerner / tegnet, for så deretter lagre alle verdiene i en tekst-tabell
                    test3 = test3.Replace("/", " ").TrimStart();
                    string[] fValuesContent = test3.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    //Konverterer fra tekst til int, og lagrer det i en tabell
                    int[] fValuesContentConverted = Array.ConvertAll(fValuesContent, delegate (string s) { return int.Parse(s); });
                    //Tabeller for lagring av vertice og tekstur indekser som blir lest under f
                    int[] verticeIndexes = new int[fValuesContentConverted.Length / dele];
                    int[] textureIndexes = new int[verticeIndexes.Length];

                    int j = 0;

                    /**Løkke som splitter verdiene og lagrer dem i deres koresponderende tabeller**/
                    for (int i = 0; i < fValuesContentConverted.Length; i += dele)
                    {
                        if (dele == 2)
                        {
                            verticeIndexes[j] = fValuesContentConverted[i] - vListCounter;
                            j++;
                        }
                        if (dele == 3)
                        {
                            verticeIndexes[j] = fValuesContentConverted[i] - vListCounter;
                            textureIndexes[j] = fValuesContentConverted[i + 1] - tListCounter;
                            j++;
                        }
                    }

                    /**Oppretelse av trekanter for objekter med teksturer**/
                    for (int k = 0; k < verticeIndexes.Length - 2; k++)
                    {
                        triangles.Add(fValuesCounter);
                        triangles.Add(fValuesCounter + 1 + k);
                        triangles.Add(fValuesCounter + 2 + k);
                    }

                    //teller som holder oversikt hvor mange indekser det har blitt gått gjennom
                    fValuesCounter += verticeIndexes.Length;
                    //lagrer vertices indekser i global liste som brukes for å oprette mesh
                    globalVerticeIndexes.Add(verticeIndexes);
                    
                    if (textureIndexes[0] != 0)
                        globalTextureIndexes.Add(textureIndexes);
                }

                /**o/g verdiene brukes i .obj for å merkere at det er nytt objekt i 3D-modellen**/
                else if (line[0] == 'o' || line[0] == 'g')
                {
                    objectsCounter++;
                    //navn som gameobjekter får
                    objectName = line.Replace("o", "").TrimStart();
                    //Brukes for å holde oversikt på fVerdiene for hver gameobject slikt alle resetes til 1 som start posisjon
                    vListCounter += verticesList.Count;
                    tListCounter += textureList.Count;

                    //siden det startes med o eller g, så må det holdes variabel fram til neste o kommer 
                    //slikt at vertice, normaler og/eller teksturer deles til riktige objekter
                    if (objectsCounter == 1)
                    {
                        objectNameFromObjFile = objectName;
                        continue;
                    }

                    //Lagrer trekantverdier i et liste som ble tatt vare på under forekomst av f
                    fixedTrianglesList.Add(triangles.ToArray());
                    //utfører metoder som opretter nye vertices og tekstur verdier basert på f verdiene
                    //må brukes fordi Unity krever like store tabeller for teksturer, normaler og vertices
                    //mens .obj-filer bruker f verdier for å holde de sammen.
                    fixedTextures();
                    fixedVertices();
                    //kaller på hovedfunksjonen opretter mesh basert på verdiene til vertices og teksturer
                    addOBJFile(objectNameFromObjFile);
                    //tildeler materialer til meshet som oprettes
                    meshRenderer.materials = materials();
                    //sletter innholdet i listene, for å kunne lagre informasjon til nytt objekt i 3D-modellen
                    verticesList.Clear();
                    fixedVerticeList.Clear();
                    textureList.Clear();
                    fixedTextureList.Clear();
                    globalVerticeIndexes.Clear();
                    globalTextureIndexes.Clear();
                    materialNameOBJ.Clear();
                    fixedTrianglesList.Clear();
                    //resete antall submesh
                    subMeshCounter = -1;
                    objectNameFromObjFile = objectName;
                    fValuesCounter = 0;
                }

                /**u i .obj filer brukes for å gi riktig materialet til 3D-modellen basert på materialnavnet**/
                else if (line[0] == 'u')
                {
                    //navnent for materialet fra obj filen
                    string mN = line.Replace("usemtl", "");
                    mN = mN.TrimStart();
                    //legges til materialNameOBJ listen som sener brukes til å tildele materialer til hver mesh/submesh
                    materialNameOBJ.Add(mN);

                    fixedVertices();
                    fixedTrianglesList.Add(triangles.ToArray());
                    triangles.Clear();
                    globalVerticeIndexes.Clear();
                    globalTextureIndexes.Clear();
                }

                else if (line[0] == '#')
                    continue;

                /**brukes for coroutine som tillater brukeren å se modellen lastes inn uten at applikasjonen fryser**/
                if (yieldReturnCounter > linesPerFrame)
                {
                    yield return null;
                    yieldReturnCounter = 0;
                }
            }

            //for siste objektet i .obj filen
            fixedTrianglesList.Add(triangles.ToArray());
            fixedTextures();
            fixedVertices();
            addOBJFile(objectName);
            meshRenderer.materials = materials();
            triangles.Clear();
            fixedTrianglesList.Clear();
            fixedTextureList.Clear();
            clearStuff();
        }
    }

    /**Metode som opretter nye vertice verdier basert på globalVerticeIndexes, brukes når teksturer eksisterer i modellen**/
    void fixedVertices()
    {
        for (int i = 0; i < globalVerticeIndexes.Count; i++)
        {
            for (int j = 0; j < globalVerticeIndexes[i].Length; j++)
            {
                fixedVerticeList.Add(verticesList[globalVerticeIndexes[i][j] - 1]);
            }
        }
    }

    /**Metode som opretter nye tekstur verdier basert på globalTextureIndexes, brukes når teksturer eksisterer i modellen**/
    void fixedTextures()
    {

        for (int i = 0; i < globalTextureIndexes.Count; i++)
        {
            for (int j = 0; j < globalTextureIndexes[i].Length; j++)
            {
                fixedTextureList.Add(textureList[globalTextureIndexes[i][j] - 1]);
            }
        }
    }

    /**Metode som kjøres når det ikke eksisterer teksturer i 3D-modellen**/
    IEnumerator objectRead()
    {
        //sletter innholdet i alle listene for å kunne oprette nytt 3D-modell
        clearLists();
        /**Tellere som brukes underveis**/
        int yieldReturnCounter = 0;
        int vListCounter = 0;
        int objectsCounter = 0;
        string objectNameFromObjFile = "";
        /**Åpner .obj filen og starter å lese den linje for linje**/
        using (FileStream file = File.OpenRead(objFileName))
        using (var reader = new StreamReader(file))
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                //øker verdien på telleren for coroutine
                yieldReturnCounter++;
                if (line == "")
                    continue;

                if (line[0] == 'v')
                {
                    /**Verdiene av vertices blir lagret her**/
                    if (line[1] == ' ')
                    {
                        #region Vertices
                        //Verdiene fra innholdet i teksten lagres i en tekst-tabell uten nøkkelord
                        string[] test3 = fixString(line, "v");
                        //Innholdet i tekst-tabellen konverteres til en float tabell
                        float[] doubles = Array.ConvertAll(test3, delegate (string s) { return float.Parse(s); });
                        //Verdiene lagres så i en global Vector3-liste
                        Vector3 vertices = new Vector3(doubles[0]*scale, doubles[1]*scale, doubles[2]*scale);
                        verticesList.Add(vertices);
                        #endregion
                    }

                    /**Verdiene av normaler lagres her**/
                    else if (line[1] == 'n')
                    {
                        #region Normales
                        string[] test3 = fixString(line, "vn");
                        float[] doubles = Array.ConvertAll(test3, delegate (string s) { return float.Parse(s); });
                        Vector3 vertices = new Vector3(doubles[0]*scale, doubles[1]*scale, doubles[2]*scale);

                        normalesList.Add(vertices);

                        #endregion
                    }
                }

                /**For hver f som forekommer under lesing av .obj filen. f verdien i .obj filen grupperer sammen vertices og normaler**/
                else if (line[0] == 'f')
                {
                    int dele = 1;
                    string test3 = line.Replace("f", "").TrimStart();
                    //Løkke som brukes for å bestemme hvliken verdier som er med i f
                    // det kan være v og vn.
                    for (int i = 0; i < test3.Length; i++)
                    {
                        if (test3[i] == '/')
                        {
                            if (test3[i + 1] == '/')
                                dele = 2;
                            else if (test3[i + 1] != '/')
                                dele = 3;
                            break;
                        }
                    }

                    //fjerner / tegnet, for så deretter lagre alle verdiene i en tekst-tabell
                    test3 = test3.Replace("/", " ").TrimStart();
                    /**Konverterer verdiene fra string til en string array for så konvertere fra string array til int array**/
                    string[] allFValues = test3.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    int[] allFValuesConverted = Array.ConvertAll(allFValues, delegate (string s) { return int.Parse(s); });
                    //Tabell for lagring av vertice indekser som blir lest under f
                    int[] verticesLength = new int[allFValuesConverted.Length / dele];

                    /**Fyller inn verdiene til vertices og textures**/
                    int j = 0;

                    for (int i = 0; i < allFValuesConverted.Length; i += dele)
                    {
                        if (dele == 2 || dele == 1)
                        {
                            verticesLength[j] = allFValuesConverted[i] - vListCounter;
                            j++;
                        }

                        if (dele == 3)
                        {
                            verticesLength[j] = allFValuesConverted[i] - vListCounter;
                            j++;
                        }
                    }

                     // lagrer vertices indekser i global liste som brukes for å oprette mesh
                     globalVerticeIndexes.Add(verticesLength);
                }

                /**o/g verdiene brukes i .obj for å merkere at det er nytt objekt i 3D-modellen**/
                else if (line[0] == 'o' || line[0] == 'g')
                {
                    objectsCounter++;
                    //navn som gameobjekter får
                    objectName = line.Replace("o", "").TrimStart();
                    //Brukes for å holde oversikt på fVerdiene for hver gameobject slikt alle resetes til 1 som start posisjon
                    vListCounter += verticesList.Count;
                    //siden det startes med o eller g, så må det holdes variabel fram til neste o kommer 
                    //slikt at vertice, normaler og/eller teksturer deles til riktige objekter
                    if (objectsCounter == 1)
                    {
                        objectNameFromObjFile = objectName;
                        continue;
                    }

                    // Kaller metoden som lager trekanter basert på fVerdier
                    fixedTriangles();

                    //kaller på hovedfunksjonen opretter mesh basert på verdiene til vertices
                    addOBJFile(objectNameFromObjFile);

                    //tildeler materialer til meshet som oprettes
                    meshRenderer.materials = materials();

                    //sletter innholdet i listene, for å kunne lagre informasjon til nytt objekt i 3D-modellen
                    verticesList.Clear();
                    textureList.Clear();
                    clearStuff();
                    materialNameOBJ.Clear();
                    fixedTrianglesList.Clear();

                    //resete antall submesh
                    subMeshCounter = -1;

                    objectNameFromObjFile = objectName;
                }

                /**u i .obj filer brukes for å gi riktig materialet til 3D-modellen basert på materialnavnet**/
                else if (line[0] == 'u')
                {
                    //navnet for materialet fra obj filen
                    string mN = line.Replace("usemtl", "");
                    mN = mN.TrimStart();
                    //legges til materialNameOBJ listen som sener brukes til å tildele materialer til hver mesh/submesh
                    materialNameOBJ.Add(mN);
                    fixedTriangles();
                    clearStuff();
                }

                else if (line[0] == '\n')
                {
                    continue;
                }

                else if (line[0] == '#')
                    continue;

                /**brukes for coroutine som tillater brukeren å se modellen lastes inn uten at applikasjonen fryser**/
                if (yieldReturnCounter > linesPerFrame)
                {
                    yield return null;
                    yieldReturnCounter = 0;
                }
            }

            //for siste objektet i .obj filen
            fixedTriangles();
            addOBJFile(objectName);

            meshRenderer.materials = materials();
            clearStuff();
            fixedTrianglesList.Clear();
        }
    }

    void clearStuff()
    {
        triangles.Clear();
        globalVerticeIndexes.Clear();
        globalTextureIndexes.Clear();
    }

    /**Trekanter som oprettes når det ikke eksisterer noen teksturer på modellen, da slipper man å lage
     * nye vertice verdier, og kan basere seg bare på de gamle siden kun vertices brukes **/
    void fixedTriangles()
    {
        //Metoden går gjennom indeks verdiene til vertices og lager trekanter basert på dem
        //trekanter som oprettes følger klokkerotasjonen, slik at de er synlige utifra
        for (int i = 0; i < globalVerticeIndexes.Count; i++)
        {
            for (int j = 2; j < globalVerticeIndexes[i].Length; j++)
            {
                triangles.Add(globalVerticeIndexes[i][0] - 1);
                triangles.Add(globalVerticeIndexes[i][j - 1] - 1);
                triangles.Add(globalVerticeIndexes[i][j] - 1);
            }

        }
        //Trekantene blir lagret i en liste som brukes til å tildele trekanter til Mesh klassen
        fixedTrianglesList.Add(triangles.ToArray());
    }
 
    /**Metoden som opretter 3D-modeller basert på verdier fra .obj filen*/
    void addOBJFile(string objectName)
    {
        //Dersom det eksisterer flere submesh, så oprettes først en her så deretter modifiseres den utenfor if-setningn
        if (subMeshCounter == -1)
        {
            //opretter nytt spillobjekt
            go = new GameObject();
            //plaserer den under hoved "mappen"
            //go.transform.parent = parent.transform;
            go.transform.SetParent(parent.transform, false);

            //go.transform.position = new Vector3(parent.transform.position.x + go.transform.position.x, parent.transform.position.y + go.transform.position.y, parent.transform.position.z + go.transform.position.z);
            go.transform.rotation = parent.transform.rotation;

            //gir nanvnet til objektet basert på navnet som ble lest under forekomst av o i .obj filen
            go.name = objectName;
            //tildeler meshfilter og meshrenderer for å kunne vise og tildele materialer til 3D-modellen i Unity
            meshFilter = go.AddComponent<MeshFilter>();
            meshRenderer = go.AddComponent<MeshRenderer>();
            mesh = meshFilter.mesh;
            // fra Unity: You should call this function before rebuilding triangles array.
            mesh.Clear();
            //verdien som sier hvor mange submesh objektet i 3D-modellen inneholder
            mesh.subMeshCount = fixedTrianglesList.Count() - 1;
        }

        //hvis det er flere submeshes ikke gå i if()-setningen igjen før hele mesh er ferdig
        subMeshCounter = 0;

        //hvis 3D-modellen har teksturer må vertice og tekstur verdiene ha like størrelse på tabellene
        if(hasTexture==true)
        {
            //tildeler vertices og teksturer til Mesh klassen
            mesh.vertices = fixedVerticeList.ToArray();
            mesh.uv = fixedTextureList.ToArray();
        }

        //dersom det ikke eksisterer teksturer, kan vertice verdiene være så mange det er i .obj filen orginalt
        if(hasTexture!=true)
        {
            //tildeler vertice til mesh klassen
            mesh.vertices = verticesList.ToArray();
        }

        //Funksjonen som utgir trekanter for hver submesh
        for (int i = 1; i < fixedTrianglesList.Count; i++)
        {
            mesh.SetTriangles(fixedTrianglesList[i], i - 1);
        }

        //Innebygde funksjoner som fikser normaler og volum
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        //fjerner innholdet i listene så nytt submesh kan oprettes
        fixedVerticeList.Clear();
        fixedTextureList.Clear();
        verticesList.Clear();
    }

    /**Metode som brukes for å lese gjennom .mtl filer**/
    void mtlRead()
    {
        //fjerner gamle innholdet
        addMaterials.Clear();

        //Hvis det eksisterer .mtl fil
        if (mtlFileName != null)
        {
            Material mat = null;
            using (FileStream file = File.OpenRead(mtlFileName))
            //samme utgangspunt som .obj fil, leser linje for linje og utfører forskjellige kommandoer
            using (var reader = new StreamReader(file))
            {

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "")
                        continue;

                    //når new forekommer kan det antas at det er newmtl nøkkelord siden det ikke eksiseter noe annet 
                    //nøkkelord som begynner med new
                    if (line[0] == 'n')
                    {
                        if (line[1] == 'e')
                        {
                            if (line[2] == 'w')
                            {
                                string s = line.Replace("newmtl", "");
                                s = s.TrimStart();
                                if (mat != null)
                                {
                                    //materialer legges i en global Material liste
                                    addMaterials.Add(mat);
                                }

                                //Oppretter nytt material i Unity, bruker standard shader
                                //fordi den viser seg til å gi best farge 
                                mat = new Material(Shader.Find("Standard"));
                                //navnet på materialet
                                mat.name = s;
                            }
                        }
                    }

                    else if (line[0] == 'K')
                    {
                        /**Specular-farge verdier tildeles her**/
                        if (line[1] == 's')
                        {
                            //lagrer verdiene i en string-tabell først, for så deretter konvertere til float
                            string[] ksValues = fixString(line, "Ks");
                            float[] doubles = Array.ConvertAll(ksValues, delegate (string s) { return float.Parse(s); });
                            //oprette nytt farge basert på verdiene som leses fra .mtl filen
                            Color colors = new Color(doubles[0], doubles[1], doubles[2]);
                            //_SpecColor er specular-farge for standard shader materialet
                            //og her tildeles fargen som ble oprettet overfor
                            mat.SetColor("_SpecColor", colors);
                        }

                        /**Diffuse-farge verdier tildeles her**/
                        else if (line[1] == 'd')
                        {
                            string lineCpy = line;
                            string[] kdValues = fixString(lineCpy, "Kd");
                            float[] doubles = Array.ConvertAll(kdValues, delegate (string s) { return float.Parse(s); });
                            Color colors = new Color(doubles[0], doubles[1], doubles[2]);
                            //_Color er diffuse farge i standard shader
                            mat.SetColor("_Color", colors);
                        }

                        /**Ambient-farge verdier tildeles her **/
                        else if (line[1] == 'a')
                        {
                            string lineCpy = line;
                            string[] kaValues = fixString(lineCpy, "Ka");
                            float[] doubles = Array.ConvertAll(kaValues, delegate (string s) { return float.Parse(s); });
                            Color colors = new Color(doubles[0], doubles[1], doubles[2]);
                            //_EmmisionColor er ambient-farge i standard shader
                            mat.SetColor("_EmmisionColor", colors);
                        }

                    }

                    /**Ns brukes for å beskrive "shines" til et material**/
                    else if (line[0] == 'N')
                    {
                        if (line[1] == 's')
                        {
                            string lineCpy = line.Replace("Ns", "").TrimStart();
                            float nsValue = float.Parse(lineCpy);
                            //.mtl lagrer Ns fra 0 til 1000, mens Unity bruker 0 til 1 så her konverteres det
                            nsValue = nsValue / 1000;
                            //"shines" i standard shader beskrives med _Glossiness
                            mat.SetFloat("_Glossiness", nsValue);
                        }
                    }

                    /**For gjennomsiktighet**/
                    else if (line[0] == 'd')
                    {
                        string lineCpy = line;
                        line = line.Replace("d", "").TrimStart();
                        float alpha = float.Parse(line);
                        Color c = mat.GetColor("_Color");
                        c.a = alpha;
                        //Kode som er nødvednig for å sette gjennomsiktighet på materialet
                        //hentet fra Unity
                        if (alpha < 1)
                        {
                            mat.SetFloat("_Mode", 3);
                            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                            mat.SetInt("_ZWrite", 0);
                            mat.DisableKeyword("_ALPHATEST_ON");
                            mat.EnableKeyword("_ALPHABLEND_ON");
                            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                            mat.renderQueue = 3000;
                            mat.SetColor("_Color", c);
                        }
                    }

                    /**Her velges tekstur som skal benyttes for materialet**/
                    else if (line[0] == 'm')
                    {
                        if (line[1] == 'a')
                        {
                            if (line[2] == 'p')
                            {
                                if (line[4] == 'K')
                                {
                                    if (line[5] == 'd')
                                    {
                                        hasTexture = true;
                                        string str = line;
                                        //klipper ut map_Kd fra .mtl filen slik at bare
                                        //filstien står igjen, som brukes for å hente
                                        //bildet til teksturen
                                        str = str.Replace("map_Kd", "").TrimStart();
                                        string path = objFileName;
                                        path = path.Substring(0, path.LastIndexOf("/"));
                                        //Legger til hele filstien til texturen
                                        path = path + "/" + str;
                                        //Oppretter ny texture
                                        Texture2D tex = new Texture2D(2, 2);
                                        //Henter bildet og setter den til texture
                                        tex.LoadImage(File.ReadAllBytes(path));
                                        //Setter texture i material listen
                                        mat.SetTexture("_MainTex", tex);
                                        mat.SetColor("_Color", Color.white);
                                    }
                                }
                            }
                        }
                    }
                }

                if (mat != null)
                {
                    addMaterials.Add(mat);
                }
            }
        }
    }

    /**Metoden kjøres når Apply knappen i filebrowseren trykkes inn**/
    public void objectImport()
    {
        inProgress = true;

        clearLists();
        //opretter "mappe" for sammling av objekter som er en del av 3D-modellen
        parent = new GameObject();

        //henter filstien fra Filebrowseren
        Path(FileBrowser.FullFilePath);
        string output = objFileName.Split('/').Last();
        output = output.Split('.').First();
        //mappenavnet blir navnet på 3D-modellen
        parent.name = output;
        //legger til informasjon slik at modelbrowser kan finne 3D-modellene
        parent.tag = "3d-modell";

        ModelBrowser.currentActiveGameObject = parent;
        ModelBrowser.currentActiveModel = parent.transform.name;

        hasTexture = false;
        //starter å lese .mtl filen først
        mtlRead();
        //basert på om modellen bruker tekstur eller ikke kjøres ulike metoder
        //med coroutines
        if (hasTexture == false)
            StartCoroutine(objectRead());
        if (hasTexture == true)
            StartCoroutine(objectReadTexture());

        inProgress = false;
    }

    /**Metode som tildeler riktige materialer til hver mesh**/
    Material[] materials()
    {
        //oppretter Material-tabell basert på hvor mange usemtl det finnes i .obj filen
        Material[] mats = new Material[materialNameOBJ.Count];

        //dersom mtlfilen ikke eksisterer så settes det bare til en hvit gråttmaterial, så 3D-modellen
        //blir synlig men inneholder ikke noen farger eller teksturer
        if (mtlFileName == null)
            mats[0] = new Material(Shader.Find("Standard"));

        int k = 0;
        //starter løkke som går gjennom alle materialer i den globale Material listen 
        //som ble oprettet under .mtl lesning
        for (int i = 0; i < materialNameOBJ.Count; i++)
        {
            for (int j = 0; j < addMaterials.Count; j++)
            {
                //hvis navnet i tekst-listen som ble lagret under forekomst av usemtl ved lesing av .obj
                //er lik med innholdet i navnet på materialet i materiallisten så lagres materialet i et 
                //tabell som brukes til å tildele materialer til mesh
                if (materialNameOBJ[i] == addMaterials[j].name)
                {
                    mats[k] = addMaterials[j];
                    k++;
                }
            }
        }

        //tabellen som inneholder material info hver mesh får
        return mats;
    }
}