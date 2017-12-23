using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextImporter : MonoBehaviour {

    [SerializeField] private TextAsset[] textFile;
    [SerializeField] private string[] textLines;

	// Use this for initialization
	void Start () {
	    
        if (textFile[0] != null)
        {

            textLines = (textFile[0].text.Split('\n'));

        }

	}
	
}
