using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PLEASE COMMENT THIS CODE

public class Grapple : MonoBehaviour
{
    public GameObject Player;
    public GameObject hook;
    public GameObject grappleHolder;
    public GameObject hookedObj;
    public GameObject FirstPersonCharacter;
    public GameObject HookReset;

    public float hookTravelSpeed;
    public float playerTravelSpeed;
    public float maxDistance;
    private float currentDistance;

    public static bool fired;
    public bool hooked;
    private bool grounded;

    void Update()
    {
        //Using the hook
        if(Input.GetMouseButtonDown(0) && fired == false)
        {
            fired = true;
        }

        if(fired == true && hooked == false)
        {
            hook.transform.Translate(Vector3.forward * Time.deltaTime * hookTravelSpeed);
            currentDistance = Vector3.Distance(transform.position, hook.transform.position);

            if(currentDistance >= maxDistance)
                ReturnHook();
        }

        if (hooked == true && fired == true)
        {
            hook.transform.parent = hookedObj.transform;
            transform.position = Vector3.MoveTowards(transform.position, hook.transform.position, Time.deltaTime * playerTravelSpeed);
            float distanceToHook = Vector3.Distance(transform.position, hook.transform.position);

            this.GetComponent<Rigidbody>().useGravity = false;

            if(distanceToHook < 3)
            {
                if (grounded == false)
                {
                    this.transform.Translate(Vector3.forward * Time.deltaTime * 13f);
                    this.transform.Translate(Vector3.up * Time.deltaTime * 28f);
                }
                StartCoroutine("Climb");
            } 
        }
        else 
        {
            hook.transform.parent = FirstPersonCharacter.transform;
            this.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    IEnumerator Climb()
    {
        yield return new WaitForSeconds(0.1f);
        ReturnHook();
    }

    void ReturnHook()
    {
        hook.transform.rotation = Player.transform.rotation;
        hook.transform.position = grappleHolder.transform.position;
        fired = false;
        hooked = false;
    }

    void CheckIfGrounded()
    {
        RaycastHit gHit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1);

        if(Physics.Raycast(transform.position, dir, out gHit, distance))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
}
