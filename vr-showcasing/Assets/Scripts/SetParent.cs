using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//skriptet settes på spillobjekt som skal festes til annet spillobjekt ved oppstart
//for lettere organisering i scehene-hierarkiet

public class SetParent : MonoBehaviour {

    public GameObject parent; //skal ha referanse til "parent" (spillobjektet som det festes til)
    public bool worldPositionStays = true; //bestemmer om posisjon forblir eller endres til lik "parent"

	// Use this for initialization
	void Start () {
        this.transform.SetParent(parent.transform, worldPositionStays);
	}
}
