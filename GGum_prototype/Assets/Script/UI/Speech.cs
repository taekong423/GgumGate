using UnityEngine;
using System.Collections;

public class Speech : MonoBehaviour {

    public int speechNumber;
    public bool playerStop;
    public SpeechData[] speechList;

    [HideInInspector]
    public bool check;

    private SpeechBubble speechBubble;


    // Use this for initialization
    void Start()
    {
        check = true;
        speechBubble = GameObject.Find("SpeechBubble").GetComponent<SpeechBubble>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (check)
        {
            if (other.CompareTag("Player"))
            {
                check = false;
                StartCoroutine(speechBubble.StartSpeech(speechNumber, playerStop, speechList));
            }
        }
    }
}
