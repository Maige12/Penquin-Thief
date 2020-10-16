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

    private Vector3 origin;//A private Vector3 which is set to the players position at all time, this allows the sphere to follow them
    public float sphereRadius;// The size of the sphere which is attached to the player, this is public to allow for balance testing and tweaking without having to manually access the code
    public float maxDistance;// The max distance script allows for the sphere to cast in a direction, in which case this variable decides how far it is cast, this can have potential uses later on, but is an artifact from the tutorial I followed
    private Vector3 direction;// This Vector 3 is used for orientation, it has no current use in the current iteraction of our sphere and is an artefact
    public LayerMask layermask;//This layermask allows us to hide certain objects from the raycast and make it so we only collide with certain items within the map.
    public GameObject CurrentObject;// A public "GameObject" using for testing and debugging which allows us to see what we are currently colliding with

    private float currentHitDistance;//The distance between the the raycast hit and the object it is colliding with, this is partially inused with our current iteracion

    public NightGuardAI _nightguardAI;
    // Start is called before the first frame update
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

        origin = transform.position;// Sets the origin to the players current transform
        direction = transform.forward;//sets the transform of the sphere to the players direction
        RaycastHit hit;//the structure used to get information back from the spherecast
        // Can the player be seen?
        
        
        
        CheckPOV();

        //if the ai can see the player, go to the player's transform position
        if (canSeePlayer == true)
        {
            GetComponent<NavMeshAgent>().speed = 10.0f;
            nav.isStopped = false;
            nav.SetDestination(player.position); //Sets the destination at where the player is
            lastKnownPosition = player.position; //updates the last known location to the players current location
        }
        else
        {
            //If player can't be seen, continue patrolling
            if (isPatrolling)
            {
                GetComponent<NavMeshAgent>().speed = 3.5f;
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




        if(Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, layermask, QueryTriggerInteraction.UseGlobal))//the main code used to cast the ray and give it the properties we want, the "Use Global", allows us to create collisions globally.
        {
            CurrentObject = hit.transform.gameObject;//Sets the window in the menu to current object
            currentHitDistance = hit.distance;//updates current hit distance to be as accurate as possible    
            if(CurrentObject.tag == "Player")
            {
                canSeePlayer = true;       
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else
        {
            currentHitDistance = maxDistance;//if the current hitdistance doesnt return anything, then it will display a default value set to the max value
            CurrentObject = null;//if the raycast doesn't hit anything, then the menu will return a null state

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








   




    private void OnDrawGizmosSelected()//this void creates the physical representation of the sphere we can see, this will be turned off outside of testing
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin +direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }
}









