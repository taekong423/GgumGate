using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHPBar : MonoBehaviour {

    static BossHPBar _instance;

    IEnemy _currentBoss;

    bool _isDisplay = false;

    public Image _hpImg;

    public static BossHPBar Instance
    {
        get
        {
            //if (_instance == null)
            //    _instance = this;
            return _instance;
        }
    }

    BossHPBar()
    {
        _instance = this;
    }

    IEnumerator Display()
    {
        while (_isDisplay)
        {
            _hpImg.fillAmount = (float)_currentBoss.CurrentHP / _currentBoss.MaxHP;

            yield return null;
        }
    }

    public static void Display(IEnemy boss)
    {
        _instance._currentBoss = boss;
        _instance._isDisplay = true;

        _instance.gameObject.SetActive(true);

        _instance.StartCoroutine(_instance.Display());
    }

    public static void Conceal()
    {
        _instance._currentBoss = null;
        _instance._isDisplay = false;

        _instance.gameObject.SetActive(false);
    }

}
