using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFiring : MonoBehaviour {

    NightGuardAI enemyControl;
    public GameObject threeDBullet;
    public float firingTime = 2;
    public float originalFiringTime = 2;
    public int firingSpeed = 500;

    //start is called before the first frame rate

    // This is for initialization
    void Start()
    {
        enemyControl = gameObject.GetComponent<NightGuardAI>();
    }

    // Update is called once per frame
    void Update()
    {
    

        if (enemyControl.canSeePlayer)
        {
            firingTime -= Time.deltaTime;
            if (firingTime <= 0)
            
            {
                GameObject clone = Instantiate(threeDBullet, transform.position, transform.rotation);
                clone.GetComponent<Rigidbody>().AddForce(transform.forward * firingSpeed);
                firingTime = originalFiringTime;
                Destroy(clone, 3);
            }
        }
    }
}