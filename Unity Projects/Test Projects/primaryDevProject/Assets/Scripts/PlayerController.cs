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

    float turnSmoothVelocity; //DONT MODIFY OURSELVES, used for SmoothDampAngle calculations
    float speedSmoothVelocity; //DONT MODIFY OURSELVES, used for SmoothDampAngle calculations
    float currentSpeed; //Current speed of Player
    Transform cameraTransform; //Reference to the Camera Transform

    // Start is called before the first frame update
    void Start()
    {
        //ADD GET COMPONENT FOR ANIMATOR ONCE ANIMATIONS ARE IMPLEMENTED
        cameraTransform = Camera.main.transform; //Sets cameraTransform to the Tarnsform of the Main Camera
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //Keyboard Inputs
        Vector2 inputDir = input.normalized; //Takes input Vector2 and turns it into a direction

        //Use Trigonometry to find out the direction at which the character is moving (Using the Horizontal (X) and Verticle (Y)
        //inputs to find the direction angle (Theta (Θ)). Opposite = Y, Adjacent = X, Tan Θ = arctan(opposite/adjacent). Facing
        // Forward = 0 Degrees, Facing Right = 90 Degrees, Facing Down = 180 Degrees, Facing Left = 270 Degrees.

        if(inputDir != Vector2.zero) //Only calculates the rotation if inputDir is not 0, 0
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y; //Target Rotation

            //Sets characters rotation (Returns in Radians and is converted to degrees using Mathf.Rad2Deg)
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime); //ref lets SmoothDampAngle modify turnSmoothVelocity
        }

        bool running = Input.GetKey(KeyCode.LeftShift); //Will only turn running bool to True if Key is being pressed
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude; //Sets targetSpeed to run/walk, multiplies by inputDir magnitude
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime); //Smooths out the current player speed

        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World); //Moving the character

        //ADD ANIMATION CONTROL HERE ONCE ANIMATIONS ARE IMPLEMENTED
    }
}

/*
 * References:
 *      - https://youtu.be/ZwD1UHNCzOc
 *          - Video used to create this Character Controller. Use this to create a basic controller and add our own custom functions
 *          where needed.
 *      - https://youtu.be/sNmeK3qK7oA
 *          - Used to Updatet the Player Controller with 3rd Person Camera functionality
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
*/
