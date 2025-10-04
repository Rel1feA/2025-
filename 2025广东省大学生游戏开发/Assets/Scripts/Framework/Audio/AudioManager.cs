using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioType
{
    BGM,
    SE
}

public class AudioManager : Singleton<AudioManager>
{
    public List<Sound> sounds;
    public AudioMixer mixer;
    private Dictionary<string, AudioSource> audioDics=new Dictionary<string, AudioSource>();
    private Sound currentBGM;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        foreach(Sound sound in sounds)
        {
            GameObject obj = new GameObject(sound.clip.name);
            obj.transform.SetParent(transform);
            AudioSource source= obj.AddComponent<AudioSource>();
            source.clip=sound.clip;
            source.playOnAwake = sound.playOnAwake;
            source.loop=sound.loop;
            source.volume=sound.volume;
            source.outputAudioMixerGroup=sound.mixerGroup;
            if(source.playOnAwake)
            {
                source.Play();
            }
            audioDics.Add(sound.clip.name, source);
        }
    }

    public void PlayAudio(string name)
    {
        if(!audioDics.ContainsKey(name))
        {
            return;
        }
        else
        {
            if(FindSoundByName(name).audioType==AudioType.BGM)
            {
                if(currentBGM!=null)
                {
                    StopAudio(currentBGM.clip.name);
                }
                currentBGM = FindSoundByName(name);
            }
            audioDics[name].Play();
        }
    }

    public void StopAudio(string name)
    {
        if(!audioDics.ContainsKey(name))
        {
            return;
        }
        else
        {
            audioDics[name].Stop();
        }
    }

    public Sound FindSoundByName(string name)
    {
        foreach(Sound sound in sounds)
        {
            if(sound.clip.name==name)
            {
                return sound;
            }
        }
        return null;
    }

    public void ChangeBGMValue(float value)
    {
        mixer.SetFloat("BGM", value);
    }

    public void ChangeSEValue(float value)
    {
        mixer.SetFloat("SE", value);
    }
}

[System.Serializable]
public class Sound
{
    public AudioClip clip;

    public AudioMixerGroup mixerGroup;

    public AudioType audioType;

    [Range(0f, 1f)]
    public float volume=0.5f;

    public bool playOnAwake;

    public bool loop;
}

