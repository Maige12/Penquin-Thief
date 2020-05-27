using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PLEASE COMMENT THIS CODE

public class GrappleDetect : MonoBehaviour
{
    public GameObject player;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hookable")
        {
            player.GetComponent<Grapple>().hooked = true;
            player.GetComponent<Grapple>().hookedObj = other.gameObject;
        }
    }
}
