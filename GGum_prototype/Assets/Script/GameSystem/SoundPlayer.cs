using UnityEngine;
using System.Collections.Generic;

public class SoundPlayer : MonoBehaviour {

    [System.Serializable]
    public class AudioData
    {
        public string _name;
        public AudioClip _clip;
    }

    public List<AudioData> _audioDates;

    public AudioSource _source;

    void Awake()
    {
        if (_source == null)
            _source = GetComponentInChildren<AudioSource>();

        if (_source == null)
            _source = gameObject.AddComponent<AudioSource>();
    }

    AudioClip GetClip(string dataName)
    {
        return _audioDates.Find(x=>x._name == dataName)._clip;
    }


    public void Play(string dataName)
    {
        _source.clip = GetClip(dataName);

        _source.Play();
    }

}
