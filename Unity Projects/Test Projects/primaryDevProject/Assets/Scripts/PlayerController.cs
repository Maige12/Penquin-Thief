using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Player Controller Script (VER 1, 24-05-2020) 
 *  
 *  ATTACH TO PLAYER OBJECT IN SCENE HIERARCHY
 * 
 *  Description:
 *      - This script controls the Player's Movements and Key Inputs
 *          - Running, Walking
 *      
 *      Can be activated/deactivated by pressing the 'ESC' key.
 *      
 *  Changelog:
 *      - 24-05-2020 (2.01pm) (Darcy Wilson, 32926762):
 *          - Created the Script File
 *          - Added PUBLIC walkSpeed, runSpeed, turnSmoothTime, speedSmoothTime variables
 *          - Added PRIVATE turnSmoothVelocity, speedSmoothVelocity, currentSpeed, input, inputDir, targetRotation, running, targetSpeed variables
 *          - Added new information to REFERENCES section (Bottom of Script File)
 *          - Script confirmed to work
 *      - 24-05-2020 (2.54pm) (Darcy Wilson, 32926762):
 *          - Added cameraTransform.eulerAngles.y to targetRotation value
 *              - Moves the player in the direction the Camera is facing
 *      - 24-05-2020 (5.05pm) (Darcy Wilson, 32926762):
 *          - Added GetModifiedSmoothTime, Move and Jump Functions
 *          - Added Collision & Jumping functionality
 *          - Script confirmed to work
 *              
 *  Known Bugs:
 *      - N/A
*/

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2.0f; //Player Walking Speed
    public float runSpeed = 6.0f; //Player Running Speed
    public float turnSmoothTime = 0.2f; //Approximate number of seconds for SmoothDampAngle to go from current value to target value (TURNING)
    public float speedSmoothTime = 0.1f; //Approximate number of seconds for SmoothDampAngle to go from current value to target value (SPEED)
    public float gravity = -12.0f; //Gravity Scale
    public float jumpHeight = 0.45f; //Height at which the player can jump (0.05 Units higher then Basic Create height)
    [Range(0.0f, 1.0f)] //Clamps value of airControlPercent in a range of 0.0f to 1.0f
    public float airControlPercent; //A value from 1.0f to 0.0f to control how well the player can control themselves in a jump (Modifies Smooth Time for Character Rotation/Speed)

    float turnSmoothVelocity; //DONT MODIFY OURSELVES, used for SmoothDampAngle calculations
    float speedSmoothVelocity; //DONT MODIFY OURSELVES, used for SmoothDampAngle calculations
    float currentSpeed; //Current speed of Player
    float velocityY; //Starts the player's Velcoity on the Y-Axis
    Transform cameraTransform; //Reference to the Camera Transform
    CharacterController controller; //Gets a reference to the Character Controller Component

    // Start is called before the first frame update
    void Start()
    {
        //ADD GET COMPONENT FOR ANIMATOR ONCE ANIMATIONS ARE IMPLEMENTED
        cameraTransform = Camera.main.transform; //Sets cameraTransform to the Tarnsform of the Main Camera
        controller = GetComponent<CharacterController>(); //Gets the Character Controller attached to the Player
    }

    // Update is called once per frame
    void Update()
    {
        //KEY INPUT SECTION
        if (Input.GetKeyDown(KeyCode.Escape) && PauseMenuUI.pause == false && EndScreenScript.gameEnd == false) //Checks to see if the player has pressed down Escape and if the Pause UI is OFF
        {
            PauseMenuUI.OpenPauseMenu(); //Opens the Pause Menu
        }
        else
            if (Input.GetKeyDown(KeyCode.Escape) && PauseMenuUI.pause == true && EndScreenScript.gameEnd == false) //Checks to see if the player has pressed down Escape and if the Pause UI is ON
            {
                PauseMenuUI.ContinueGame(); //Continues the Game
            }

        if (Input.GetKeyDown(KeyCode.P)) //USED FOR DEBUGGING PURPOSES ONLY, REMOVE LINE OF CODE ONCE PROPER SYSTEM IS IMPLEMENTED AND HAVE THIS RUN IN EndScreenScript.cs
        {
            UIInitialiserScript.GetEndScreenObj.SetActive(true); //Sets the End Screen to True

            if(UIInitialiserScript.GetPauseMenuObj.activeInHierarchy || UIInitialiserScript.GetPlayerUI.activeInHierarchy) //Checks if Pasue Menu/Player UI is Active
            {
                UIInitialiserScript.GetPauseMenuObj.SetActive(false); //Turns off Pause Menu if it's on
                UIInitialiserScript.GetPlayerUI.SetActive(false); //Sets the Pause Menu object to active so it can be interacted with
            }

            EndScreenScript.gameEnd = true;
            Time.timeScale = 0; //Freezes time so game freezes
            Cursor.lockState = CursorLockMode.Confined; //Locks cursor to the game view (Cannot move mouse outside of it, only works in BUILD, not EDITOR)
            Cursor.visible = true; //Shows Cursor
        }

        //INPUT SECTION
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //Keyboard Inputs
        Vector2 inputDir = input.normalized; //Takes input Vector2 and turns it into a direction
        bool running = Input.GetKey(KeyCode.LeftShift); //Will only turn running bool to True if Key is being pressed

        Move(inputDir, running); //Calls the Move fucntion to let the player move (Passes in inputDir and running)

        if (Input.GetKeyDown(KeyCode.Space)) //Checks to see if the Space Key has been pushed down
        {
            Jump();
        }

        //ANIMATION SECTION
        //ADD ANIMATION CONTROL HERE ONCE ANIMATIONS ARE IMPLEMENTED
    }

    void Move(Vector2 inputDir, bool running)
    {
        //Use Trigonometry to find out the direction at which the character is moving (Using the Horizontal (X) and Verticle (Y)
        //inputs to find the direction angle (Theta (Θ)). Opposite = Y, Adjacent = X, Tan Θ = arctan(opposite/adjacent). Facing
        // Forward = 0 Degrees, Facing Right = 90 Degrees, Facing Down = 180 Degrees, Facing Left = 270 Degrees.

        if (inputDir != Vector2.zero) //Only calculates the rotation if inputDir is not 0, 0
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y; //Target Rotation

            //Sets characters rotation (Returns in Radians and is converted to degrees using Mathf.Rad2Deg)
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime)); //ref lets SmoothDampAngle modify turnSmoothVelocity
        }

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude; //Sets targetSpeed to run/walk, multiplies by inputDir magnitude
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime)); //Smooths out the current player speed

        velocityY += Time.deltaTime * gravity; //Calculates the velocityY value using deltaTime and gravity

        Vector3 velocity = (transform.forward * currentSpeed) + (Vector3.up * velocityY); //Gets the player's Velocity and adjusts for Gravity

        controller.Move(velocity * Time.deltaTime); //Moves the character controller based on their current velocity and the Time
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude; //Updates currentSpeed to what the controllers actual speed is (Stops Velocity if running into object)

        if (controller.isGrounded) //Checks to see if they player is on the ground
        {
            velocityY = 0.0f; //Resests the velocityY so player doesn't keep falling
        }
    }

    void Jump() //Gives the player the ability to Jump
    {
        if(controller.isGrounded) //Checks to see if they player is on the ground
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight); //Calculate Jump Velocity (Using Kinematic Equations)
            velocityY = jumpVelocity; //Makes the velocityY be equal to the Velocity of the Jump
        }
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded) //Checks to see if they player is on the ground
        {
            return smoothTime; //Return smoothTime without modification
        }

        if(airControlPercent == 0.0f) //Checks to see if airControlPercent = 0 (Does this to prevent an error)
        {
            return float.MaxValue; //Returns maximum possible value if airControlPercent is 0
        }

        return smoothTime / airControlPercent; //Returns a modified smoothTime variable with airControlPercent added (If airControlPercent = 1, No Modiifcation)
    }
}

/*
 * References:
 *      - https://youtu.be/ZwD1UHNCzOc
 *          - Video used to create this Character Controller. Use this to create a basic controller and add our own custom functions
 *          where needed.
 *      - https://youtu.be/sNmeK3qK7oA
 *          - Used to Updatet the Player Controller with 3rd Person Camera functionality
 *       - https://youtu.be/qITXjT9s9do
 *          - Used to add Collisions and Jumping to the Character Controller
 *      - https://docs.unity3d.com/ScriptReference/Input.GetAxisRaw.html
 *          - Returns the value of the virtual axis identified by axisName with no smoothing filtering applied. The value will be 
 *          in the range -1...1 for keyboard and joystick input. Since input is not smoothed, keyboard input will always be either 
 *          -1, 0 or 1. This is useful if you want to do all smoothing of keyboard input processing yourself.
 *      - https://docs.unity3d.com/ScriptReference/Vector2-normalized.html
 *          - Returns this vector with a magnitude of 1 (Read Only). When normalized, a vector keeps the same direction but its 
 *          length is 1.0. If the vector is too small to be normalized a zero vector will be returned.
 *      - https://www.mathopenref.com/arctan.html
 *          - The arctan function is the inverse of the tangent function. It returns the angle whose tangent is a given number. 
 *      - https://byjus.com/tan-theta-formula/
 *          - The Tan Θ is the ratio of the Opposite side to the Adjacent, where (Θ) is one of the acute angles.
 *          - Tan Θ = arctan(opposite/adjacent)
 *      - https://docs.unity3d.com/2017.3/Documentation/ScriptReference/Transform-eulerAngles.html
 *          - The rotation as Euler angles in degrees. The x, y, and z angles represent a rotation z degrees around the z axis, 
 *          x degrees around the x axis, and y degrees around the y axis. Only use this variable to read and set the angles to 
 *          absolute values. Don't increment them, as it will fail when the angle exceeds 360 degrees. Use Transform.Rotate 
 *          instead.
 *      - https://mathworld.wolfram.com/EulerAngles.html
 *          - According to Euler's rotation theorem, any rotation may be described using three angles. If the rotations are 
 *          written in terms of rotation matrices D, C, and B, then a general rotation A can be written as 
 *      - https://docs.unity3d.com/ScriptReference/Mathf.Atan2.html
 *          - Return value is the angle between the x-axis and a 2D vector starting at zero and terminating at (x,y). This 
 *          function takes account of the cases where x is zero and returns the correct angle rather than throwing a division by 
 *          zero exception.
 *      - https://www.mathopenref.com/radians.html
 *          - A unit of measure for angles. One radian is the angle made at the center of a circle by an arc whose length is equal 
 *          to the radius of the circle. 
 *      - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator
 *          - The conditional operator ?:, also known as the ternary conditional operator, evaluates a Boolean expression and 
 *          returns the result of one of the two expressions, depending on whether the Boolean expression evaluates to true or 
 *          false.
 *      - https://docs.unity3d.com/ScriptReference/Vector2-magnitude.html
 *          - Returns the length of this vector (Read Only). The length of the vector is square root of (x*x+y*y).
 *      - https://docs.unity3d.com/ScriptReference/CharacterController-isGrounded.html
 *          - Was the CharacterController touching the ground during the last move? (Boolean Value)
 *      - https://www.physicsclassroom.com/class/1DKin/Lesson-6/Kinematic-Equations
 *          - The kinematic equations are a set of four equations that can be utilized to predict unknown information about an object's 
 *          motion if other information is known. The equations can be utilized for any motion that can be described as being either a 
 *          constant velocity motion (an acceleration of 0 m/s/s) or a constant acceleration motion.
*/
