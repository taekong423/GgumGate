using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpeechBubble : MonoBehaviour {

    public GameObject petObject;
    public GameObject playerObject;
    public int currentSpeech;

    private Text petText;
    private Text playerText;
    private Player player;


    // Use this for initialization
    void Start () {
        petText = petObject.GetComponentInChildren<Text>();
        playerText = playerObject.GetComponentInChildren<Text>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClearSpeech()
    {
        petText.text = "";
        playerText.text = "";
    }

    public void HideSpeechBubble()
    {
        petObject.SetActive(false);
        playerObject.SetActive(false);
    }

    public IEnumerator StartSpeech(int number, float speechTime, bool playerStop, SpeechData[] speechData)
    {
        if (playerStop)
            //player.isStop = true;

        ClearSpeech();
        currentSpeech = number;
        yield return null;

        for (int i = 0; i < speechData.Length; i++)
        {
            if (currentSpeech == number)
            {
                ShowSpeechBubble(speechData[i]);
                yield return new WaitForSeconds(speechTime);
            }
            else
            {
                if (playerStop)
                    //player.isStop = false;
                yield break;
            }
        }

        HideSpeechBubble();

        //if (playerStop)
            //player.isStop = false;
    }

    void ShowSpeechBubble(SpeechData data)
    {
        switch (data.type)
        {
            case SpeechType.Pet:
                if (!petObject.activeInHierarchy)
                    petObject.SetActive(true);
                petText.text = data.text;
                break;
            case SpeechType.Player:
                if (!playerObject.activeInHierarchy)
                    playerObject.SetActive(true);
                playerText.text = data.text;
                break;
        }
    }
}
