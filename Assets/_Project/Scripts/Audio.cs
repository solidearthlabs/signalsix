using UnityEngine;
using System.Collections;
using System.IO;

public class Audio : MonoBehaviour {

    public Audio Instance;
    public AudioClip[] clips;
    AudioSource audioSource;
    AudioSource music;

    //Pre-load clips
	void Start ()
    {
        Instance = this;
        DirectoryInfo d = new DirectoryInfo("Assets/Prefabs/Resources/Audio");

        //counts files for proper initialization
        int i = 0;
        foreach (FileInfo f in d.GetFiles())
        {
            if(f.Extension == ".wav")
                i++;
        }
        clips = new AudioClip[i];

        i = 0;
        foreach (FileInfo f in d.GetFiles())
        {
            if (f.Extension == ".wav")
            {
                string name = Path.GetFileNameWithoutExtension(f.Name);
                clips[i] = Resources.Load("Audio/" + name) as AudioClip;
                clips[i].name = name;
                i++;
            }
        }

        //music = gameObject.AddComponent<AudioSource>().clip = Resources.;

    }

    public AudioSource PlayClip(string name)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        return setPlay(name);
    }
    public AudioSource PlayClip(string name, float volume)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        return setPlay(name);
    }
    public AudioSource PlayClip(string name, float volume, float pitch)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        return setPlay(name);
    }
    public AudioSource PlayClip(string name, GameObject g)
    {
        audioSource = g.AddComponent<AudioSource>();
        return setPlay(name);
    }
    public AudioSource PlayClip(string name, float volume, GameObject g)
    {
        audioSource = g.AddComponent<AudioSource>();
        audioSource.volume = volume;
        return setPlay(name);
    }
    public AudioSource PlayClip(string name, float volume, float pitch, GameObject g)
    {
        audioSource = g.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        return setPlay(name);
    }

    AudioClip findByName(string name)
    {
        foreach(AudioClip ac in clips)
        {
            if (ac.name == name)
                return ac;
        }

        return null;
    }

    AudioSource setPlay(string name)
    {
        audioSource.clip = findByName(name);
        audioSource.spatialBlend = 1f;
        audioSource.Play();
        Destroy(audioSource, audioSource.clip.length);
        return audioSource;
    }
    

    
}
