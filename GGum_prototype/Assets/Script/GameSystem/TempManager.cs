using UnityEngine;
using System.Collections;

public class TempManager : MonoBehaviour {

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Countinue()
    {
        Time.timeScale = 1;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
