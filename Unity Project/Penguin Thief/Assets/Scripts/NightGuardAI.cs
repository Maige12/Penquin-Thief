using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class HelperMethods
{
    public static List<GameObject> GetChildren(this GameObject go)
    {
        List<GameObject> children = new List<GameObject>(); // temp memory on the stack ready to get all the path points

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

    public bool canSeePlayer = false; //This detects if the ai is able to detect the player
    public LayerMask layerMask; //The layer that the walls will be on. Ai cannot see player if raycast hits these walls
    public float fieldOfViewRange; //The angle in front of the Ai that can see the player
    public float hearingRange; //A bubble around the Ai that can detect the player

    public bool isPatrolling = true; //Ai will continue following patrol points if ==true
    public List<GameObject> myPathPoints; //These are the Patrol points. Ai will follow them one by one, top to bottom
    public GameObject myPath; 
    public int currentPoint = 0; //Where the first point is
    public Vector3 lastKnownPosition; //Last known position of the player
    

    [SerializeField]
    GameObject visionMesh; //The mesh used for the vision of the Night Guard

    public bool alertSound; //A Boolean to control whether the Nigh Guard can play his Alert/Unalert sound

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform; //Find the location of the player
        nav = GetComponent<NavMeshAgent>(); //Activate Navmesh component

        //initialise the path points
        myPathPoints = new List<GameObject>();

        //find path points as children
        myPathPoints = myPath.GetChildren();

        alertSound = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //This is what happens when the nightguard collides with the player
        if (collision.gameObject.tag == "Player")
        {
            GameOverScript.OpenGameOver(); //Opens Game Over Screen (Controlled by GameOverScript.cs Script)
        }

        if(collision.gameObject.layer == 8) //Checks to see if the Night Guard is colliding with Layer 8 (Locked Doors)
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>()); //Stops collision between the Night Guard and Locked Doors
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Can the player be seen?
        CheckPOV();

        //if the ai can see the player, go to the player's transform position
        if (canSeePlayer == true)
        {
            nav.isStopped = false;
            nav.SetDestination(player.position); //Sets the destination at where the player is
            lastKnownPosition = player.position; //updates the last known location to the players current location
        }
        else
        {
            //If player can't be seen, continue patrolling
            if (isPatrolling)
            {
                //Head to next path point
                nav.isStopped = false;
                nav.SetDestination(myPathPoints[currentPoint].transform.position); //sets the destination to the pathpoints

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


    

    void CheckPOV() //Checks the detection range of the Ai
    {
        //Direction to Player
        Vector3 direction = player.position - transform.position;

        // ------------------------
        //Grab the cone mesh
        //Is the player inside the cone?
        // ------------------------

        //ray to player
        Ray ray = new Ray(transform.position, direction);

        //distance to the player
        float distance = Vector3.Distance(player.position, transform.position);

        //draw a line to the player
        Debug.DrawRay(transform.position, direction);

        //If the line is not hitting the wall - the player must be visible
        if (!Physics.Raycast(ray, distance, layerMask))
        {
            if ((Vector3.Angle(ray.direction, transform.forward)) < fieldOfViewRange) //if the player is not obstructed by a wall, canSeePlayer = true
            {
                canSeePlayer = true;

                if(alertSound == true)
                {
                    alertSound = false;
                }
            }
        }
        else
        {
            canSeePlayer = false;
        }

        //Can the enemy hear the player?
        if (canSeePlayer == false)
        {
            if (Vector3.Distance(transform.position, player.position) < hearingRange) //If the ai can hear the player, sai will chase the player
                canSeePlayer = true; //This changes destination to player transform

            if(alertSound == false) //If ai cant hear the player, continue patrolling
            {
                alertSound = true;
            }
        }
    }
}