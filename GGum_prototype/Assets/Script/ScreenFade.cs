using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour {

    public enum FadeState
    {
        Idle,
        FadeIn,
        FadeOut,
    }

    public Image blackScreen;
    public FadeState fadeState;
    public float fadeTime;

    float alpha;

	// Use this for initialization
	void Start () {
        alpha = 1;
        blackScreen = GetComponent<Image>();
        blackScreen.color = new Color(0, 0, 0, 1);
    }
	
	// Update is called once per frame
	void Update () {
	    if (fadeState == FadeState.FadeIn)
        {
            Fade(1);
        }
        else if (fadeState == FadeState.FadeOut)
        {
            Fade(-1);
        }
	}

    void Fade(int fadeDir)
    {
        alpha += fadeDir * 1/fadeTime * Time.deltaTime;

        alpha = Mathf.Clamp01(alpha);

        blackScreen.color = new Color(0, 0, 0, alpha);

        if (alpha <= 0)
        {
            fadeState = FadeState.Idle;
        }
        else if (alpha >= 1)
        {
            fadeState = FadeState.Idle;
        }
    }

    public void FadeIn()
    {
        fadeState = FadeState.FadeIn;
    }

    public void FadeOut()
    {
        fadeState = FadeState.FadeOut;
    }
}
