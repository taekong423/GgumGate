using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections;

public class Loader : MonoBehaviour {

    public Image _logo;
    public GameObject _touchMe;

	// Use this for initialization
	void Start () {
        StartCoroutine(Display());
	}

    IEnumerator Display()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("InGame");

        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            _logo.fillAmount = async.progress + 0.1f;

            if (_logo.fillAmount >= 1.0f)
                break;

            yield return null;
        }

        _touchMe.SetActive(true);

        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                async.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
