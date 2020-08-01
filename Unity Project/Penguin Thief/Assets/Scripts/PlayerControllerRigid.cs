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
    Vector3 displacement; //The new position of the player
    Vector3 acceleration; //The accelaration which is calculated, and then added to the Velocity
    Vector3 velocity; //The current velocity of the player

    [SerializeField, Range(0f, 100f)] //Clamps the range of the speed from 0 to 100
    float maxSpeed = 10f; //The maximum speed that the player can move at
    [SerializeField, Range(0f, 100f)] //Clamps the range of the acceleration from 0 to 100
    float maxAcceleration = 10f; //The maximum acceleration that the player can achieve
    float maxSpeedChange; //Amount that the velocity will change per update

    Rigidbody playerRigid; //The player's Rigidbody component

    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();

        playerInput.x = 0.0f; //Left/Right (X) Movement
        playerInput.y = 0.0f; //Forward/Back (Y) Movement
    }

    void Update()
    {
        playerInput.x = Input.GetAxis("Horizontal"); //Gets the Input Axis from the player (Horizontal (A, D Keys))
        playerInput.y = Input.GetAxis("Vertical"); //Gets the Input Axis from the player (Vertical (W, S Keys))

        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        desiredVelocity = new Vector3(playerInput.x, 0.0f, playerInput.y) * maxSpeed; //Gets the player's position and adds it to the desiredVelocity Vector, multiplied by the Maximum Speed value
        maxSpeedChange = maxAcceleration * Time.deltaTime; //How much the velocity should change each update

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange); //Ensures that the current X velocity is moved to the desired velocity, being constrained to the maximum speed change
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange); //Ensures that the current Z velocity is moved to the desired velocity, being constrained to the maximum speed change

        displacement = velocity * Time.deltaTime;  //The new position for the player to move to

        transform.localPosition += displacement; //Moves the player's position to a new Vector3 by using the displacement value
    }
}

/*
 * References:
 *      - https://catlikecoding.com/unity/tutorials/movement/sliding-a-sphere/
 *      - https://docs.unity3d.com/ScriptReference/Mathf.MoveTowards.html
 *      - https://youtu.be/4Qq7d9elXNA
*/
