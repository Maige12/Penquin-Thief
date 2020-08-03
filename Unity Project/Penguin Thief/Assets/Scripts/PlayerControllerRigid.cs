using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Player Controller Script (RIGID) (VER 1, 31-07-2020) 
 *  
 *  ATTACH TO PLAYER OBJECT IN SCENE HIERARCHY
 * 
 *  Description:
 *      - This script controls the Player's Movements and Key Inputs. This is done through the Rigidbody as opposed to using the Character Controller
 *          component for more control.
 *      
 *  Changelog:
 *      - 31-05-2020 (Darcy Wilson, 32926762):
 *          - Created the Script File
 *          - Removed references to Character Controller
 *          - Began working on getting it funtioning
 *              
 *  Known Bugs:
 *      - N/A
*/

public class PlayerControllerRigid : MonoBehaviour
{
    Vector2 playerInput; //Vector2 to read the player input

    Vector3 desiredVelocity; //The desired velocity of the player
    Vector3 velocity; //The current velocity of the player

    [SerializeField, Range(0.0f, 100.0f)] //Clamps the range of the speed from 0 to 100
    float maxSpeed = 10.0f; //The maximum speed that the player can move at
    [SerializeField, Range(0.0f, 100.0f)] //Clamps the range of the acceleration from 0 to 100
    float maxAcceleration = 10.0f; //The maximum acceleration that the player can achieve
    [SerializeField, Range(0.0f, 100.0f)] //Clamps the range of the air acceleration from 0 to 100
    float maxAirAcceleration = 1.0f; //The maximum amount of control that the player has in the air
    [SerializeField, Range(0.0f, 10.0f)] //Clamps the range of the players Jump Height from 0 to 10
    float jumpHeight = 2.0f; //The maximum height that the player can jump
    [SerializeField, Range(0.0f, 90.0f)] //Clamps the range of the degrees maximum scalable ramp from 0 to 90
    float maxGroundAngle = 25.0f; //The maximum angle of a slope that the player can jump from (Degrees)

    bool desiredJump; //A boolean to detect whether or not the player wants to jump
    bool onGround; //A boolean to check whether the player is on the ground or not
    float maxSpeedChange; //Amount that the velocity will change per update
    float acceleration; //The player's current acceleration 
    float minGroundDotProduct; //The Dot Product of maxGroundAngle

    Rigidbody playerRigid; //The player's Rigidbody component

    void OnValidate() //This function is called when the script is loaded or a value is changed in the Inspector (Called in the editor only).
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad); //Calculates the Dot Product of maxGroundAngle by multiplying maxGroundAngle by Mathf.Deg2Rad (Converts to Radians)
    }

    void Awake()
    {
        playerRigid = GetComponent<Rigidbody>(); //Finds the Rigidbody Component attached to the player

        playerInput.x = 0.0f; //Left/Right (X) Movement
        playerInput.y = 0.0f; //Forward/Back (Y) Movement

        OnValidate(); //Calls the OnValidate() Function
    }

    void Update()
    {
        playerInput.x = Input.GetAxis("Horizontal"); //Gets the Input Axis from the player (Horizontal (A, D Keys))
        playerInput.y = Input.GetAxis("Vertical"); //Gets the Input Axis from the player (Vertical (W, S Keys))

        playerInput = Vector2.ClampMagnitude(playerInput, 1.0f); //Returns a copy of the playerInput vector with its magnitude clamped to maxLength. Allows for positions inside of a circle to be counted

        desiredVelocity = new Vector3(playerInput.x, 0.0f, playerInput.y) * maxSpeed; //Gets the player's position and adds it to the desiredVelocity Vector, multiplied by the Maximum Speed value

        desiredJump |= Input.GetButtonDown("Jump"); //Turns the Boolean to True if the player presses the 'Jump' button
    }

    void FixedUpdate()
    {
        velocity = playerRigid.velocity; //Changes the velocity to be equal to that of the Rigidbodies current velocity
        acceleration = onGround ? maxAcceleration : maxAirAcceleration; //Chooses what acceleration to use based on whether the player is on the ground or not
        maxSpeedChange = acceleration * Time.deltaTime; //How much the velocity should change each update

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange); //Ensures that the current X velocity is moved to the desired velocity, being constrained to the maximum speed change
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange); //Ensures that the current Z velocity is moved to the desired velocity, being constrained to the maximum speed change

        if (desiredJump) //Checks to see if the player wants to jump
        {
            desiredJump = false; //Sets the Jump back to False
            Jump(); //Moves to the Jump Function to perform the jump
        }

        playerRigid.velocity = velocity; //Changes the Rigidbodies velocity to be equal to that of the now filtered velocity

        onGround = false;
    }

    void OnCollisionEnter(Collision collision) //Checks to see if the player has entered a collider
    {
        EvaluateCollision(collision); //Runs the EvaluateCollision Function
    }

    void OnCollisionStay(Collision collision) //Checks to see if the player is touching a collider
    {
        EvaluateCollision(collision); //Runs the EvaluateCollision Function
    }

    void EvaluateCollision(Collision collision) //Checks to see the direction of the Normal Vector that is touching the player
    {
        for (int i = 0; i < collision.contactCount; i++) //contactCount finds the total amount of contacts the player is making
        {
            Vector3 normal = collision.GetContact(i).normal; //collision.GetContact gets the contact point at the specified index and adds it to a Vector called normal
            onGround |= normal.y >= minGroundDotProduct; //If the Y-Component of the Vector is greater than or equal to the minGroundDotProduct, then the player is toucing the ground
        }
    }

    void Jump()
    {
        if(onGround) //Runs if the player is currently on the ground
        {
            velocity.y += Mathf.Sqrt(-2.0f * Physics.gravity.y * jumpHeight); //Returns the Square Root of -2, multiplied by gravity, multiplied by the maximum height (Currently matches Earth's gravity)
        }
    }
}

/*
 * References:
 *      - https://catlikecoding.com/unity/tutorials/movement/sliding-a-sphere/
 *      - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-or-operator-
 *      - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator
 *      - https://docs.unity3d.com/ScriptReference/Mathf.MoveTowards.html
 *      - https://docs.unity3d.com/ScriptReference/Mathf.Sqrt.html
 *      - https://youtu.be/4Qq7d9elXNA
*/
