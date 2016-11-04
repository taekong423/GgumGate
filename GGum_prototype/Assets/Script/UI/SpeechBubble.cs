using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour {

    public enum SpeechType
    {
        Pet,
        Player,
    }

    [System.Serializable]
    public struct Speech
    {
        public SpeechType type;
        public string text;
    }

    public int currentSpeech;
    public Speech[] speechList;
    
    private Text petText;
    private Text playerText;


    // Use this for initialization
    void Start () {
        petText = GameObject.Find("PetText").GetComponent<Text>();
        playerText = GameObject.Find("PlayerText").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
