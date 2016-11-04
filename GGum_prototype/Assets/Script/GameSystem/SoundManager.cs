using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioMixer mixer;

    public SoundPlayer BGM;

    void Start()
    {
        Global.shared<SoundManager>(this);
    }

    public void ChangeBGM(string clipName)
    {
        if(BGM != null)
            BGM.Play(clipName);
    }

    public void SetMasterVolume(float volume)
    {
        mixer.SetFloat("Master", volume);
    }
    
    public void SetBGMVolume(float volume)
    {
        mixer.SetFloat("BGM", volume);
    }

    public void SetEffectVolume(float volume)
    {
        mixer.SetFloat("Effect", volume);
    }

    public void ToggleMasterVolume()
    {
        float volume;

        mixer.GetFloat("Master", out volume);

        volume = volume == 0 ? -80 : 0;

        SetMasterVolume(volume);
    }

    public void ToggleBGMVolume()
    {
        float volume;

        mixer.GetFloat("BGM", out volume);

        volume = volume == 0 ? -80 : 0;

        SetBGMVolume(volume);
    }

    public void ToggleEffectVolume()
    {
        float volume;

        mixer.GetFloat("Effect", out volume);

        volume = volume == 0 ? -80 : 0;

        SetEffectVolume(volume);
    }

}
