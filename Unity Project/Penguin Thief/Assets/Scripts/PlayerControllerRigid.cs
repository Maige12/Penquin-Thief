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
    float maxSpeed = 4.0f; //The maximum speed that the player can move at
    [SerializeField, Range(0.0f, 100.0f)] //Clamps the range of the acceleration from 0 to 100
    float maxAcceleration = 10.0f; //The maximum acceleration that the player can achieve
    [SerializeField, Range(0.0f, 100.0f)] //Clamps the range of the air acceleration from 0 to 100
    float maxAirAcceleration = 1.0f; //The maximum amount of control that the player has in the air
    [SerializeField, Range(0.0f, 10.0f)] //Clamps the range of the players Jump Height from 0 to 10
    float jumpHeight = 0.5f; //The maximum height that the player can jump
    [SerializeField, Range(0.0f, 90.0f)] //Clamps the range of the degrees maximum scalable ramp from 0 to 90
    float maxGroundAngle = 25.0f; //The maximum angle of a slope that the player can jump from (Degrees)
    [SerializeField, Range(0.0f, 20.0f)] //Clamps the range of the degrees maximum scalable ramp from 0 to 20
    float rotSmoothSpeed = 4.0f; //The speed at which the player character will rotate
    [SerializeField] //Allows it to be seen in the inspector
    Transform playerInputSpace = default; //The transform relative to the player's movements

    bool desiredJump; //A boolean to detect whether or not the player wants to jump
    bool onGround; //A boolean to check whether the player is on the ground or not

    public bool slideActive; //A bool to check of the player is currently sliding
    float maxSpeedChange; //Amount that the velocity will change per update
    float acceleration; //The player's current acceleration 
    float minGroundDotProduct; //The Dot Product of maxGroundAngle
    float jumpSpeed; //The speed at which the player can jump
    float alignedSpeed; //The current speed aligned with the contact normal
    float dynFriction; //The Dynamic Friction of the Physics Material
    float statFriction; //The Static Friction of the Physics Material
    Collider col; //The player's collider
    public float targetTime; //the amount of time that the slide will go for.

    ItemCollectScript itemCollect;

    Vector2 playerInput; //Vector2 to read the player input
    Vector3 desiredVelocity; //The desired velocity of the player
    Vector3 velocity; //The current velocity of the player
    Vector3 contactNormal; //The current Normal Vector of the object the player is in contact with
    Vector3 initPosition; //The inital position of the player at the start of the game

    Quaternion curentRot; //Current rotation of the player character
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
        col = GetComponent<Collider>(); //Finds the collider attached to the GameObject

        itemCollect = GetComponent<ItemCollectScript>(); //Finds the Item Collection script attached to the player

        dynFriction = col.material.dynamicFriction; //Sets dynFriction to be equal to the current Dynamic Friction Value
        statFriction = col.material.staticFriction; //Sets statFriction to be equal to the current Static Friction Value

        playerInput.x = 0.0f; //Left/Right (X) Movement
        playerInput.y = 0.0f; //Forward/Back (Y) Movement

        curentRot = transform.rotation;

        initPosition = transform.position; //Gets the initial position of the player

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
        /*
        targetTime -= Time.deltaTime; //if applicable, the time will count down

        if (targetTime <= 0.0f)
        { 
            slideActive = false;
            maxSpeed = 4.0f;
        } 

        if(slideActive != true)
        {
            playerInput.x = Input.GetAxis("Horizontal"); //Gets the Input Axis from the player (Horizontal (A, D Keys))
            playerInput.y = Input.GetAxis("Vertical"); //Gets the Input Axis from the player (Vertical (W, S Keys))
        }

        if(onGround == true && Input.GetKeyDown(KeyCode.LeftControl)) //Checks to see if the player is on the ground to initiate the slide && Checks to see if the player is holding down the left control button
        {
            slideActive = true;

            if ((slideActive == true) && ((velocity.z > 2.0f) || (velocity.x > 2.0f))) //A secondary check to make sure that the slide is not currently in progress to stop infinite slides from occuring 
            {
                targetTime = 2.0f; //sets the target time, which will stop the slide at the end of the duration
                maxSpeed = 8.0f;

                slideActive = true; //sets slide active to true, which stops the player from stacking slides              
            }

            if ((slideActive == true) && ((velocity.z < -2.0f) || (velocity.x < -2.0f))) //A secondary check to make sure that the slide is not currently in progress to stop infinite slides from occuring 
            {
                targetTime = 2.0f; //sets the target time, which will stop the slide at the end of the duration
                maxSpeed = 8.0f;

                slideActive = true; //sets slide active to true, which stops the player from stacking slides              
            }
        }
        */

        if (slideActive != true) //Checks to see if the player is NOT sliding
        {
            playerInput.x = Input.GetAxis("Horizontal"); //Gets the Input Axis from the player (Horizontal (A, D Keys))
            playerInput.y = Input.GetAxis("Vertical"); //Gets the Input Axis from the player (Vertical (W, S Keys))
        }

        if (onGround == true && Input.GetKey(KeyCode.LeftControl)) //Checks to see if the player is on the ground to initiate the slide && Checks to see if the player is holding down the left control button
        {
            Slide(); //Runs the Slide function
        }
        else
        {
            slideActive = false; //Sets Slide Active to false when not holding in the Left Control Key
            maxSpeed = 4.0f; //Changes the Maximum Speed to 4.0f (Walk Speed)
        }

        playerInput = Vector2.ClampMagnitude(playerInput, 1.0f); //Returns a copy of the playerInput vector with its magnitude clamped to maxLength. Allows for positions inside of a circle to be counted

        if (slideActive != true)
        {
            RotatePlayermodel(); 

            if (Input.GetKey(KeyCode.LeftShift)) //Checks to see if the player is holding Left SHift (Run)
            {
                maxSpeed = 6.0f; //Changes the Maximum Speed to 6.0f (Run Speed)
            }
            else
                if(Input.GetKeyUp(KeyCode.LeftShift)) //Checks to see if the player has released Left SHift (Run)
                {
                    maxSpeed = 4.0f; //Changes the Maximum Speed to 4.0f (Walk Speed)
                }
        }

        if (playerInputSpace) //If a Player Input Space is present, run this If Statement
        {
            Vector3 forward = playerInputSpace.forward; //The forward vector is equal to the forward vector of the Player Input Space

            forward.y = 0.0f; //The Y direction is not needed so it's set to 0
            forward.Normalize(); //When normalized, a vector keeps the same direction but its length is 1.0

            Vector3 right = playerInputSpace.right; //The right vector is equal to the right vector of the Player Input Space

            right.y = 0.0f; //The Y direction is not needed so it's set to 0
            right.Normalize(); //When normalized, a vector keeps the same direction but its length is 1.0

            desiredVelocity = (forward * playerInput.y + right * playerInput.x) * maxSpeed; //The desired velocity is equal to the new forward and right vector, based on the Player Input Space, Player input and speed
        }
        else
            {
                desiredVelocity = new Vector3(playerInput.x, 0.0f, playerInput.y) * maxSpeed; //Gets the player's position and adds it to the desiredVelocity Vector, multiplied by the Maximum Speed value
            }

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

    void Slide() //This function controls the Sliding of the Penguin
    {
        slideActive = true; //Sets the Slideing to be active (Controls some inputs)

        if ((slideActive == true) && ((velocity.z > 2.0f) || (velocity.x > 2.0f))) //A secondary check to make sure that the slide is not currently in progress to stop infinite slides from occuring 
        {
            maxSpeed = 8.0f; //Changes the Maximum Speed to 8.0f (Slide Speed)

            slideActive = true; //sets slide active to true, which stops the player from stacking slides              
        }

        if ((slideActive == true) && ((velocity.z < -2.0f) || (velocity.x < -2.0f))) //A secondary check to make sure that the slide is not currently in progress to stop infinite slides from occuring 
        {
            maxSpeed = 8.0f; //Changes the Maximum Speed to 8.0f (Slide Speed)

            slideActive = true; //sets slide active to true, which stops the player from stacking slides              
        }
    }

    void RotatePlayermodel() //This function handles rotating the Playermodel
    {
        if (Input.GetKey(KeyCode.W)) //This will rotate the player to face away from the camera at all times when moving forward (Only rotates when holding W)
        {
            curentRot = Quaternion.Euler(0.0f, playerInputSpace.eulerAngles.y, 0.0f); //Changes the rotation to be equal to the camera's current rotation

            transform.rotation = Quaternion.Lerp(transform.rotation, curentRot, Time.deltaTime * rotSmoothSpeed); //Lerps from the current rotation to the new rotation by a set speed
        }

        if (Input.GetKey(KeyCode.S)) //This will rotate the player to face towards the camera at all times when moving backwards (Only rotates when holding S)
        {
            if ((-playerInputSpace.eulerAngles.y - 180.0f) < 0.0f) //Checks to see if the inverse if the current camera's y rotation take 180 is less then 0
            {
                curentRot = Quaternion.Euler(0.0f, -playerInputSpace.eulerAngles.y + 180.0f, 0.0f); //Changes the rotation to be equal to the inverse of the camera's current rotation, plus 180 degrees
            }
            else
            {
                curentRot = Quaternion.Euler(0.0f, -playerInputSpace.eulerAngles.y - 180.0f, 0.0f); //Changes the rotation to be equal to the inverse of the camera's current rotation, minus 180 degrees
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Inverse(curentRot), Time.deltaTime * rotSmoothSpeed); //Lerps from the current rotation to the new rotation by a set speed
        }

        if(Input.GetKey(KeyCode.A)) //This will rotate the player to face left from the camera at all times when moving forward (Only rotates when holding A)
        {
            if ((-playerInputSpace.eulerAngles.y - 90.0f) < 0.0f) //Checks to see if the inverse if the current camera's y rotation take 90 is less then 0
            {
                curentRot = Quaternion.Euler(0.0f, -playerInputSpace.eulerAngles.y + 90.0f, 0.0f); //Changes the rotation to be equal to the inverse of the camera's current rotation, plus 90 degrees
            }
            else
            {
                curentRot = Quaternion.Euler(0.0f, -playerInputSpace.eulerAngles.y - 90.0f, 0.0f); //Changes the rotation to be equal to the inverse of the camera's current rotation, minus 90 degrees
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Inverse(curentRot), Time.deltaTime * rotSmoothSpeed); //Lerps from the current rotation to the new rotation by a set speed
        }

        if (Input.GetKey(KeyCode.D)) //This will rotate the player to face right from the camera at all times when moving forward (Only rotates when holding D)
        {
            if ((-playerInputSpace.eulerAngles.y - 90.0f) < 0.0f) //Checks to see if the inverse if the current camera's y rotation take 90 is less then 0
            {
                curentRot = Quaternion.Euler(0.0f, -playerInputSpace.eulerAngles.y - 90.0f, 0.0f); //Changes the rotation to be equal to the inverse of the camera's current rotation, minus 90 degrees
            }
            else
            {
                curentRot = Quaternion.Euler(0.0f, -playerInputSpace.eulerAngles.y + 90.0f, 0.0f); //Changes the rotation to be equal to the inverse of the camera's current rotation, plus 90 degrees
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Inverse(curentRot), Time.deltaTime * rotSmoothSpeed); //Lerps from the current rotation to the new rotation by a set speed
        }
    }

    public void ResetPlayer() //This will reset the players position to the inital spawn position and reset their current inventory
    {
        itemCollect.CaughtReset();

        transform.position = initPosition; //Sets the current position of the player to the player's initial spawn position
    }
}

/*
 * References:
 *      Unity Documentation:
 *          - https://docs.unity3d.com/ScriptReference/Vector3-normalized.html
 *          - https://docs.unity3d.com/ScriptReference/Mathf.MoveTowards.html
 *          - https://docs.unity3d.com/ScriptReference/Mathf.Sqrt.html
 *          - https://docs.unity3d.com/ScriptReference/Transform.TransformDirection.html
 *          - https://docs.unity3d.com/ScriptReference/Vector3.Normalize.html
 *      Catlike Coding (Primary Tutorial):
 *          - https://catlikecoding.com/unity/tutorials/movement/sliding-a-sphere/
 *      C# References:
 *          - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-or-operator-
 *          - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator
 *      Mathematic Terms:
 *          - https://www.mathsisfun.com/algebra/vectors-dot-product.html
 *      Static and Dynamic Friction:
 *          - https://youtu.be/hdKY20z71hs
 *          - https://answers.unity.com/questions/1485212/how-do-i-change-a-physics-material-through-a-scrip.html
 *          - https://www.youtube.com/watch?v=uQ6fGtdERlY
*/
