using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Player Controller Script (RIGID) 
 *  
 *  ATTACH TO PLAYER OBJECT IN SCENE HIERARCHY
 *  
 *  NOTE: This is based off of a tutorial from Catlike Coding, please see below references for links.
 * 
 *  Description:
 *      - This script controls the Player's Movements and Key Inputs. This is done through the Rigidbody as opposed to using the Character Controller
 *          component for more control.
 *              
 *  Known Bugs:
 *      - N/A
*/

public class PlayerControllerRigid : MonoBehaviour
{
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
    float jumpSpeed; //The speed at which the player can jump
    float alignedSpeed; //The current speed aligned with the contact normal

    Vector2 playerInput; //Vector2 to read the player input
    Vector3 desiredVelocity; //The desired velocity of the player
    Vector3 velocity; //The current velocity of the player
    Vector3 contactNormal; //The current Normal Vector of the object the player is in contact with
    Vector3 ProjectOnContactPlane(Vector3 vector) //This is to store a Vector projected along the plane that the player is walking along
    {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal); //This returns a Vector which is equal to the current Contactr Normal Vector, multiplied by the dot product of an inputted Vector and Contact Normal
    }

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

    void AdjustVelocity() //The velocity adjusted for the Vector of the current plane the player is in contact with
    {
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized; //xAxis is qual to the Vector3's Right position (X Direction), which gives the vector aligned with the ground
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized; //zAxis is qual to the Vector3's forward position (Z Direction), which gives the vector aligned with the ground

        float currentX = Vector3.Dot(velocity, xAxis); //Finds the current X Directional Speed by finding the Dot Product of Velocity and xAxis
        float currentZ = Vector3.Dot(velocity, zAxis); //Finds the current Z Directional Speed by finding the Dot Product of Velocity and ZAxis

        acceleration = onGround ? maxAcceleration : maxAirAcceleration; //Chooses what acceleration to use based on whether the player is on the ground or not
        maxSpeedChange = acceleration * Time.deltaTime; //How much the velocity should change each update

        float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange); //Ensures that the current X velocity is moved to the desired velocity, being constrained to the maximum speed change
        float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange); //Ensures that the current X velocity is moved to the desired velocity, being constrained to the maximum speed change

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
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

        if (onGround) //Checks to see if the player is on the ground
        {
            contactNormal.Normalize(); //Normalizes the contactNormal to make it a proper normal
        }
        else
        {
            contactNormal = Vector3.up; //If the player isn't touching the ground, the contact normal vector will point up
        }

        AdjustVelocity(); //Runs the AdjustVelocity() function to adjust the valocity based on different factors

        if (desiredJump) //Checks to see if the player wants to jump
        {
            desiredJump = false; //Sets the Jump back to False
            Jump(); //Moves to the Jump Function to perform the jump
        }

        playerRigid.velocity = velocity; //Changes the Rigidbodies velocity to be equal to that of the now filtered velocity

        ClearState(); //Runs the ClearState function
    }

    void OnCollisionEnter(Collision collision) //Checks to see if the player has entered a collider
    {
        EvaluateCollision(collision); //Runs the EvaluateCollision Function
    }

    void OnCollisionStay(Collision collision) //Checks to see if the player is touching a collider
    {
        EvaluateCollision(collision); //Runs the EvaluateCollision Function
    }

    void ClearState() //Clears the onGround parameter and the contactNormal after each Physics Frame
    {
        onGround = false;
        contactNormal = Vector3.zero;
    }

    void EvaluateCollision(Collision collision) //Checks to see the direction of the Normal Vector that is touching the player
    {
        for (int i = 0; i < collision.contactCount; i++) //contactCount finds the total amount of contacts the player is making
        {
            Vector3 normal = collision.GetContact(i).normal; //collision.GetContact gets the contact point at the specified index and adds it to a Vector called normal

            if(normal.y >= minGroundDotProduct) //If the Y-Component of the Vector is greater than or equal to the minGroundDotProduct, then the player is toucing the ground
            {
                onGround = true; //The player is on the ground
                contactNormal += normal; //The Normal Vector of the contact surface is set to the current collision normal (Accumulates the normals of the touching colliders)
            }
        }
    }

    void Jump()
    {
        if(onGround) //Runs if the player is currently on the ground
        {
            jumpSpeed = Mathf.Sqrt(-2.0f * Physics.gravity.y * jumpHeight); //Returns the Square Root of -2, multiplied by gravity, multiplied by the maximum height (Currently matches Earth's gravity)
            alignedSpeed = Vector3.Dot(velocity, contactNormal); //Gets the current jump speed aligned with the Normal Vector of the Contact Normal

            if (alignedSpeed > 0.0f) //Checks to see if the Y Velocity is higher then 0
            {
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0.0f); //Returns largest of two or more values (Makes sure the modified Jump Speed never becomes a negative)
            }

            velocity += contactNormal * jumpSpeed; //The Y Velocity is set to the current contact normal scaled by the Jump Speed
        }
    }
}

/*
 * References:
 *      Unity Documentation:
 *          - https://docs.unity3d.com/ScriptReference/Vector3-normalized.html
 *          - https://docs.unity3d.com/ScriptReference/Mathf.MoveTowards.html
 *          - https://docs.unity3d.com/ScriptReference/Mathf.Sqrt.html
 *      Catlike Coding (Primary Tutorial):
 *          - https://catlikecoding.com/unity/tutorials/movement/sliding-a-sphere/
 *      C# References:
 *          - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-or-operator-
 *          - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator
 *      Mathematic Terms:
 *          - https://www.mathsisfun.com/algebra/vectors-dot-product.html
*/
