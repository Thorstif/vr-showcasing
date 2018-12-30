/**************************************************
 * Filebrowser klassen brukes for å oprette en
 * filutforsker som lar en brukere travarsere 
 * gjennom mapper og filer, for å velge hvilken 
 * 3D-modell skal importeres i Unity for visning
***************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
public class FileBrowser : MonoBehaviour {
    public GameObject btnDirectory;
    public GameObject btnFile;
    public GameObject directoryPanel, filePanel;
    public Text textObject;
    private string dir = "";
    public static string FullFilePath = "";
	// Use this for initialization
	void Start () {
        //Start Directory
        goHome();
        //Kjører metodene for mapper og filer
        LoadDirectories();
        LoadFiles();
	}
    /**Metode for å oprette knapper og gi dem mappenavn*/
    void LoadDirectories() 
    {
        //Fjerner gamle innholdet 
        DestroyButtons(directoryPanel);
        //Henter alle mapper basert på filstien
        string[] directories = Directory.GetDirectories(dir);
        //Løkke som kjører fram til alle mappene er gått gjennom basert på filstien
        for(int i=0;i<directories.Length;i++)
        {
            //opretter knapper basert på prefabs
            GameObject dirButton = (GameObject)Instantiate(btnDirectory);
            //Gir navn til knappene basert på mappenavn
            dirButton.GetComponentInChildren<Text>().text = Path.GetFileName(directories[i]);
            //Knappene settes i mappelisten
            dirButton.transform.SetParent(directoryPanel.transform, false);
            dirButton.transform.rotation = directoryPanel.transform.rotation;
            //Opretter Eventlistener for å bestemme hva som skal utføres når mappe-knappen trykkes inn
            Button.ButtonClickedEvent e = new Button.ButtonClickedEvent();
            int iCpy = i;
            
            e.AddListener(() =>
            {
                //Når mappe-knappen trykkes inn skal følgende metoder kjøres
                dir = directories[iCpy];
                LoadDirectories();
                LoadFiles();

            });
            //kobler listener til mappe-knappen
            dirButton.GetComponent<Button>().onClick = e;
        }
    }
    /**Metode for å opprette knapper og gi dem filnavn**/
    void LoadFiles()
    {
        
        string extention;
        //Fjerner gamle innholdet
        DestroyButtons(filePanel);
        //Henter alle filer basert på filstien
        string[] fileNames = Directory.GetFiles(dir);
        //Løkke som går gjennom alle filene basert på filstien
        for (int i = 0; i < fileNames.Length; i++)
        {
            //Kun .obj filer vises fram i fil-listeboksen
            extention = Path.GetExtension(fileNames[i]);
            if (String.Compare(extention, ".obj") == 0)
            {
                //Henter navnet til filen
                string selectedFile = Path.GetFileName(fileNames[i]);
                //Hele filstien til filen som brukes for importering
                string selectedFileFullPath = Path.GetFullPath(fileNames[i]);
                //Opretter knapper som får filnavn
                GameObject fileButton = (GameObject)Instantiate(btnFile);
                fileButton.GetComponentInChildren<Text>().text = Path.GetFileName(fileNames[i]);
                fileButton.transform.SetParent(filePanel.transform, false);
                //Setter Event listener på fil-knappene
                Button.ButtonClickedEvent e = new Button.ButtonClickedEvent();
                e.AddListener(() =>
                {
                    //Metoder som utføres når fil-knappen trykkes inn
                    FullFilePath = selectedFileFullPath;
                    textObject.text = selectedFile;
                });
                //Kobler listener til fil-knappen
                fileButton.GetComponent<Button>().onClick = e;
                
            }
            else
                continue;
        }

    }
    /**Metode som brukes for å gå tilbake en mappe**/
    public void goBackDirectory()
    {
        if(Directory.GetParent(dir)!=null)
        {
            //Setter filstien til foreldre mappen så kjøres metodene
            //for å hente mapper og filer
            dir = Directory.GetParent(dir).FullName;
            LoadDirectories();
            LoadFiles();
        }   

    }
    /**Star mappen**/
    public void goHome()
    {
        //dir = @"C:\\Users\\Thor\\Desktop\\EXPO 2017";
        //dir = Directory.GetDirectoryRoot(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)); // setter root directory der programmet er installert
        dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        LoadDirectories();
        LoadFiles();
    }

    /**Metode som sletter knappene for å gi plass til nye mapper og filer**/
    void DestroyButtons(GameObject panel)
    {
        foreach(Transform t in panel.transform)
        {
            Destroy(t.gameObject);
        }
    }
}
