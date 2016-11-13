using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Buffer : MonoBehaviour {

    public Image _noteIcon;
    public Image _superArmourIcon;
    public Image _invincibleIcon;
    public Image _exhaustionIcon;

    public Text _noteText;

    public void NoteActive(bool active)
    {
        _noteIcon.gameObject.SetActive(active);
    }
     
}
