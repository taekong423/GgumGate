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

        float speed = Random.Range(0.0f, 2.0f);
        float delay = Random.Range(0.0f, 0.1f);

        while (!async.isDone)
        {
            //_logo.fillAmount = async.progress + 0.1f;

            _logo.fillAmount += Time.deltaTime * speed;

            if (_logo.fillAmount >= 1.0f)
                break;

            yield return new WaitForSeconds(delay);

            speed = Random.Range(0.0f, 2.0f);
            delay = Random.Range(0.0f, 0.1f);
        }

        _logo.transform.parent.gameObject.SetActive(false);

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
