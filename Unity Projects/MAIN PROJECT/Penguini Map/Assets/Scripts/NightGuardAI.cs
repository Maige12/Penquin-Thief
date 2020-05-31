
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class HelperMethods
{
    public static List<GameObject> GetChildren(this GameObject go)
    {
        List<GameObject> children = new List<GameObject>(); // temp memory
                                                            // on the stack
                                                            // ready to get
                                                            // all the path points
        foreach (Transform tranIterator in go.transform)
        {
            children.Add(tranIterator.gameObject);
        }
        return children;
    }
}


public class NightGuardAI : MonoBehaviour
{
    Transform player;

    NavMeshAgent nav;

    public bool canSeePlayer = false;
    public LayerMask layerMask;
    public float fieldOfViewRange;
    public float hearingRange;


    public bool isPatrolling = true;
    public List<GameObject> myPathPoints;
    public GameObject myPath;
    public int currentPoint = 0;
    public Vector3 lastKnownPosition;
    public bool hasSeenPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        nav = GetComponent<NavMeshAgent>();

        //initialise the path points
        myPathPoints = new List<GameObject>();

        //find path points as children
        myPathPoints = myPath.GetChildren();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("GAME OVER");

        }



    }


    // Update is called once per frame
    void Update()
    {
        // Can the player be seen?
        CheckPOV();


        if (canSeePlayer == true)
        {
            nav.isStopped = false; 
            nav.SetDestination(player.position);
            lastKnownPosition = player.position;
        }
        else
        {
            if (isPatrolling)  
            {
                //Head to next path point
                nav.isStopped = false;
                nav.SetDestination(myPathPoints[currentPoint].transform.position);

                // Check range  to current path point and jump to next if close enough
                if (Vector3.Distance(transform.position, myPathPoints[currentPoint].transform.position) < 2)
                    currentPoint += 1;

                // Is it back at the start? If so, loop
                if (currentPoint > myPathPoints.Count - 1)
                    currentPoint = 0;

                

            }

            else
            {
                nav.isStopped = true; // Same as nav.enabled = false;
            }

           
        }

     
    }


    void CheckPOV()
    {
        //Direction to Player
        Vector3 direction = player.position - transform.position;
        //ray to player
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        //distance to the player
        float distance = Vector3.Distance(player.position, transform.position);
        //draw a line to the player
        Debug.DrawRay(transform.position, direction);
        //If the line is not hitting the wall - the player must be visible
        if (!Physics.Raycast(ray, distance, layerMask))
        {
            if ((Vector3.Angle(ray.direction, transform.forward)) < fieldOfViewRange)
            {


                canSeePlayer = true;
            }
        }

        else
        {
           canSeePlayer = false;
        }
        //Can the enemy hear the player?
        if (canSeePlayer == false)
            {
            if (Vector3.Distance(transform.position, player.position) < hearingRange)
                canSeePlayer = true;
            }
    }
 


    
}
