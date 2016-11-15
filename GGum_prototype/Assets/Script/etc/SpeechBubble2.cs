using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeechBubble2 : MonoBehaviour {

    bool _isDisplaying = false;

    public Text _speechText;
    public Image _speechBubbleImg;

    public Transform _contaner;

    [Header("Appear")]
    int _appearIndex = 0;
    public bool _isAppearRandom = false;
    [TextArea(3, 5)]
    public string[] _appearContent;

    [Header("Pattern1")]
    int _pattern1Index = 0;
    public bool _isPattern1Random = false;
    [TextArea(3, 5)]
    public string[] _pattern1Content;

    [Header("Pattern2")]
    int _pattern2Index = 0;
    public bool _isPattern2Random = false;
    [TextArea(3, 5)]
    public string[] _pattern2Content;

    [Header("Exhaustion")]
    int _ExhaustionIndex = 0;
    public bool _isExhaustionRandom = false;
    [TextArea(3, 5)]
    public string[] _exhaustionContent;

    [Header("OnPattern3")]
    int _onPattern3Index = 0;
    public bool _isOnPattern3Random = false;
    [TextArea(3, 5)]
    public string[] _onPattern3Content;

    [Header("Pattern3")]
    int _pattern3Index = 0;
    public bool _isPattern3Random = false;
    [TextArea(3, 5)]
    public string[] _pattern3Content;

    [Header("Dead")]
    int _deadIndex = 0;
    public bool _isDeadRandom = false;
    [TextArea(3, 5)]
    public string[] _deadContent;

    void SpeechBubbeleActive(bool active)
    {
        _speechText.gameObject.SetActive(active);
        _speechBubbleImg.gameObject.SetActive(active);
    }

    IEnumerator Display(string content, float time)
    {
        _speechText.rectTransform.localEulerAngles = new Vector3(0, _contaner.localEulerAngles.y, 0);
        _speechText.text = "";
        _speechText.text = content;
        SpeechBubbeleActive(true);

        yield return new WaitForSeconds(time);

        SpeechBubbeleActive(false);
    }

    public void Stop()
    {
        SpeechBubbeleActive(false);
        StopAllCoroutines();
    }

    public void AppearDisplay(float time)
    {
        if (_isAppearRandom)
        {
            _appearIndex = Random.Range(0, _appearContent.Length);
        }
        else
        {
            if (_appearIndex >= _appearContent.Length)
                _appearIndex = 0;

            _appearIndex++;
        }

        Stop();
        StartCoroutine(Display(_appearContent[_appearIndex], time));

    }

    public void Pattern1Display(float time)
    {
        if (_isPattern1Random)
        {
            _pattern1Index = Random.Range(0, _pattern1Content.Length);
        }
        else
        {
            if (_pattern1Index >= _pattern1Content.Length)
                _pattern1Index = 0;

            _pattern1Index++;
        }

        Stop();
        StartCoroutine(Display(_pattern1Content[_pattern1Index], time));
    }

    public void Pattern2Display(float time)
    {
        if (_isPattern2Random)
        {
            _pattern2Index = Random.Range(0, _pattern2Content.Length);
        }
        else
        {
            if (_pattern2Index >= _pattern2Content.Length)
                _pattern2Index = 0;

            _pattern2Index++;
        }

        Stop();
        StartCoroutine(Display(_pattern2Content[_pattern2Index], time));
    }

    public void ExhaustionDisplay(float time)
    {
        if (_isExhaustionRandom)
        {
            _ExhaustionIndex = Random.Range(0, _exhaustionContent.Length);
        }
        else
        {
            if (_ExhaustionIndex >= _exhaustionContent.Length)
                _ExhaustionIndex = 0;

            _ExhaustionIndex++;
        }

        Stop();
        StartCoroutine(Display(_exhaustionContent[_ExhaustionIndex], time));
    }

    public void OnPattern3Display(float time)
    {
        if (_isOnPattern3Random)
        {
            _onPattern3Index = Random.Range(0, _onPattern3Content.Length);
        }
        else
        {
            if (_onPattern3Index >= _onPattern3Content.Length)
                _onPattern3Index = 0;

            _onPattern3Index++;
        }

        Stop();
        StartCoroutine(Display(_onPattern3Content[_onPattern3Index], time));
    }

    public void Pattern3Display(float time)
    {
        if (_isPattern3Random)
        {
            _pattern3Index = Random.Range(0, _pattern3Content.Length);
        }
        else
        {
            if (_pattern3Index >= _pattern3Content.Length)
                _pattern3Index = 0;

            _pattern3Index++;
        }

        Stop();
        StartCoroutine(Display(_pattern3Content[_pattern3Index], time));
    }

    public void DeadDisplay(float time)
    {
        if (_isDeadRandom)
        {
            _deadIndex = Random.Range(0, _deadContent.Length);
        }
        else
        {
            if (_deadIndex >= _deadContent.Length)
                _deadIndex = 0;

            _deadIndex++;
        }

        Stop();
        StartCoroutine(Display(_deadContent[_deadIndex], time));
    }

}
