using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {

    [SerializeField] private GameObject textBox;
    [SerializeField] private Text theText;
    [SerializeField] private TextAsset[] textFile;
    [SerializeField] private string[] textLines;
    
    [SerializeField] private int currentLine;
    [SerializeField] private int endAtLine;
    [SerializeField] private int textFileCounter;

    // Use this for initialization
    void Start()
    {

        if (textFile[textFileCounter] != null)
        {

            textLines = (textFile[textFileCounter].text.Split('\n'));
            theText.text = textLines[0];

        }

        if (endAtLine == 0)
        {

            endAtLine = textLines.Length - 1;

        }

    }

    void Update()
    {

        if (currentLine != endAtLine)
        {

            if (Input.GetKeyUp(KeyCode.Space))
            {

                currentLine++;
                theText.text = textLines[currentLine];

            }

        }

        else if (Input.GetKeyUp(KeyCode.Space))
        {

            currentLine = 0;

            if (textFileCounter != textFile.Length - 1)
            {

                textFileCounter++;
                endAtLine = 0;
                this.Start();

            }
            else
            {

                textBox.SetActive(false);

            }


        }

    }

}
