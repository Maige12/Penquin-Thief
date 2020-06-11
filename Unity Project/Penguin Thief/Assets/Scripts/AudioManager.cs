using UnityEngine.Audio; //Adds Audio capabilities
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds; //Array of sounds being used

    void Awake()
    {
        foreach(Sound s in sounds) //Adds an Audio SOurce for each sound
        {
            s.source = gameObject.AddComponent<AudioSource>(); //Adds AudioSource component to gameObject
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name) //Public Method which takes the name of a clip and plays it
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); //Finds the sound that needs to be played from the Sound array

        if (s != null)
        {
            Debug.Log("Playing '" + name + "'!");

            s.source.Play();
        }
        else
            Debug.Log("Sound file '" + name + "' not found!");
    }
}
