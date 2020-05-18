using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayreFires : MonoBehaviour
{
    public GameObject bullet;
    public float bulletspeed = 100f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
