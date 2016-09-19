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

    private Image blackScreen;
    public FadeState fadeState;
    public float fadeTime;

	// Use this for initialization
	void Start () {
        blackScreen = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (fadeState == FadeState.FadeIn)
        {
            float alpha = Mathf.Lerp(0.0f, 1.0f, fadeTime);
            blackScreen.color = new Color(0, 0, 0, alpha);
        }

	}

    public void FadeIn()
    {
        blackScreen.color = new Color(0, 0, 0, 0);
        fadeState = FadeState.FadeIn;
    }

    public void FadeOut()
    {

    }
}
