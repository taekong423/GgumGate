using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {

    public GameObject textBox;

    public Text theText;

    public TextAsset textfile;
    public string[] textLines;

    public int currentLine;
    public int endAtLine;

    public bool DisplayOn;

    //

    void Awake()
    {
        DisplayOn = false;
    }
    // Use this for initialization
    void Start()
    {
        //

        if (textfile != null)
        {
            textLines = (textfile.text.Split('\n'));
        }

        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }
    }


    void Update()
    {
        if (DisplayOn)
        {
           
            if (Input.GetKeyDown(KeyCode.K))
            {
                NextContent();
               
            }

            if (currentLine > endAtLine)
            {
                DisableDialogu();
            }
        }
    }

    public void EnableDialogu(int StartLine, int EndLine)
    {

        currentLine = StartLine;
        endAtLine = EndLine;
        DisplayOn = true;
        textBox.SetActive(true);

        theText.text = textLines[currentLine];
    }
    public void DisableDialogu()
    {
        DisplayOn = false;
        textBox.SetActive(false);
    }

    public void NextContent()
    {
        //이름이뭔지에 따라. 일러변경및 라인 바꾸기.
        currentLine += 1;
        theText.text = textLines[currentLine];
    }

}
