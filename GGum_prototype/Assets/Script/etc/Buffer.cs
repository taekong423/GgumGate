using UnityEngine;
using UnityEngine.UI;

public class Buffer : MonoBehaviour {

    static Buffer _instance;

    public GameObject _noteIcon;
    public GameObject _superArmourIcon;
    public GameObject _invincibleIcon;
    public GameObject _exhaustionIcon;

    public Text _noteText;

    void Awake()
    {
        _instance = this;
    }

    public static void NoteActive(bool active)
    {
        _instance._noteIcon.SetActive(active);
    }

    public static void SuperArmourAcitve(bool active)
    {
        _instance._superArmourIcon.SetActive(active);
    }

    public static void InvincibleActive(bool active)
    {
        _instance._invincibleIcon.SetActive(active);
    }

    public static void Exhaustion(bool active)
    {
        _instance._exhaustionIcon.SetActive(active);
    }

}
