using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinScript : MonoBehaviour
{
    [SerializeField] //Allows for the variable to be accessable in the inspector
    [Range(0.0f, 1.0f)] //The Minimum and Maximum range the variable can go to in the inspector
    float spinSpeed; //The Speed at which the object will rotate around
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, spinSpeed, 0.0f, Space.World); //This will spin the Cube by a certian speed every frame (Space.World lets it use the Global Axis)

        //transform.localPosition.y += Mathf.Sin();
    }
}
