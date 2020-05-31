using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        // check to see who I hit???
        //if (other.gameObject.tag == "Player")
        if (other.collider.CompareTag("Player"))
        {
            PlayerStats.UpdateHealth(-1); // take one away from health
            Debug.Log("I got hit!!!!");
            // Debug.Log(PlayerStats.health);
            Destroy(gameObject);
        }
        
    }
}
