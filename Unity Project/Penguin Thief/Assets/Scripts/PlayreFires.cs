using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PLEASE COMMENT THIS CODE

public class PlayreFires : MonoBehaviour
{
    public GameObject bullet;
    public float bulletspeed = 100f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown ("Fire1"))
        {
            GameObject bul = Instantiate(bullet, transform.position, transform.rotation);

            bul.GetComponent<Rigidbody>().AddForce(transform.forward * bulletspeed);
        }
    }
}
