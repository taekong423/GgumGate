using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Tip : MonoBehaviour {

    public float nextTime;
    public float fadeTime;
    public List<string> tipList;

    private Text tipText;
    private Image background;
    private bool tipCheck;
    private bool isFading;
    private float alphaText;
    private float alphaBackground;

	// Use this for initialization
	void Awake () {
        tipList = new List<string>();
        tipText = GetComponentInChildren<Text>();
        background = GetComponent<Image>();
        tipCheck = true;
        isFading = false;
        alphaText = 1.0f;
        alphaBackground = 0.666f;
    }
	
	// Update is called once per frame
	void Update () {
        if (tipList.Count > 0 && tipCheck)
        {
            tipCheck = false;
            StartCoroutine(StartTip());
        }

	    if (isFading)
        {
            TipFadeOut();
        }
	}

    private void TipFadeOut()
    {
        alphaText -= 1.0f / fadeTime * Time.deltaTime;
        alphaBackground -= 0.666f / fadeTime * Time.deltaTime;

        alphaText = Mathf.Clamp01(alphaText);
        alphaBackground = Mathf.Clamp01(alphaBackground);

        if (alphaBackground <= 0)
        {
            isFading = false;
            tipText.color = new Color(1, 1, 1, 0);
            background.color = new Color(0, 0, 0, 0);
            tipCheck = true;
            return;
        }

        tipText.color = new Color(1, 1, 1, alphaText);
        background.color = new Color(0, 0, 0, alphaBackground);
    }

    IEnumerator StartTip()
    {
        isFading = false;
        tipText.text = string.Empty;
        tipText.color = new Color(1, 1, 1, 1);
        background.color = new Color(0, 0, 0, 0.666f);

        yield return null;
        
        for (int i = 0; i < tipList.Count; i++)
        {
            tipText.text = tipList[i];
            yield return new WaitForSeconds(nextTime);
        }

        tipList.Clear();
        isFading = true;
    }

    public void ShowTip(string text)
    {
        tipList.Add(text);
    }

    public void ShowTip(List<string> textList)
    {
        tipList.AddRange(textList);
    }
}
