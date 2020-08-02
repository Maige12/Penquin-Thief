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
    Vector3 acceleration; //The accelaration which is calculated, and then added to the Velocity
    Vector3 velocity; //The current velocity of the player

    [SerializeField, Range(0f, 100f)] //Clamps the range of the speed from 0 to 100
    float maxSpeed = 10f; //The maximum speed that the player can move at
    [SerializeField, Range(0f, 100f)] //Clamps the range of the acceleration from 0 to 100
    float maxAcceleration = 10f; //The maximum acceleration that the player can achieve
    [SerializeField, Range(0f, 10f)] //Clamps the range of the players Jump Height from 0 to 10
    float jumpHeight = 2f; //The maximum height that the player can jump

    float maxSpeedChange; //Amount that the velocity will change per update
    bool desiredJump; //A boolean to detect whether or not the player wants to jump
    bool onGround; //A boolean to check whether the player is on the ground or not

    Rigidbody playerRigid; //The player's Rigidbody component

    void Start()
    {
        playerRigid = GetComponent<Rigidbody>(); //Finds the Rigidbody Component attached to the player

        playerInput.x = 0.0f; //Left/Right (X) Movement
        playerInput.y = 0.0f; //Forward/Back (Y) Movement
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
        maxSpeedChange = maxAcceleration * Time.deltaTime; //How much the velocity should change each update

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange); //Ensures that the current X velocity is moved to the desired velocity, being constrained to the maximum speed change
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange); //Ensures that the current Z velocity is moved to the desired velocity, being constrained to the maximum speed change

        if (desiredJump) //Checks to see if the player wants to jump
        {
            desiredJump = false; //Sets the Jump back to False
            Jump(); //Moves to the Jump Function to perform the jump
        }

        playerRigid.velocity = velocity; //Changes the Rigidbodies velocity to be equal to that of the now filtered velocity
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
 *      - https://docs.unity3d.com/ScriptReference/Mathf.MoveTowards.html
 *      - https://docs.unity3d.com/ScriptReference/Mathf.Sqrt.html
 *      - https://youtu.be/4Qq7d9elXNA
*/
