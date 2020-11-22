using UnityEngine.Audio; //Adds Audio capabilities
using UnityEngine;

[System.Serializable] //Marked so it can appear in Inspector
public class Sound
{
    public string name; //Name of the clip

    public AudioClip clip; //Reference to Audio Clip being played

    [Range(0.0f, 1.0f)] //Sets a slider for the volume
    public float volume; //Volume Setting
    [Range(0.1f, 3.0f)] //Sets a slider for the pitch
    public float pitch; //Pitch Setting
    public bool loopable; //Boolean to control whether the audio loops
    public bool playAtStart; //Play the Audio at the start of the scene
    public bool bgMusic; //Lets you know if the Audio Source is meant to be the BG Music (ONLY SELECT ONE)

    [HideInInspector] //Hides it from the Inspector
    public AudioSource source; //Audio Source that is stored
}

/*
 * References:
 *      - https://youtu.be/6OT43pvUyfY
*/