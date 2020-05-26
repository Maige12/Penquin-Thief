using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public bool lockCursor; //A boolean to say if the cursor needs to be locked (Enabled/Disabled in Inspector)
    public float mouseSensitivity = 5.0f; //A modifier which adjusts the mouse sensitivity
    public float rotationSmoothTime = 0.1f; //Approximate number of seconds for SmoothDampAngle to go from current value to target value (ROTATION)
    public float targetDistance = 2.0f; //How far the camera should be from the target
    public float maxDistance = 2.0f; //Maximum Distance the camera can be from the player
    public float minDistance = 0.8f; //Minimum Distance the camera can be from the player
    public float cameraSphereRadius = 0.2f;
    public float cameraSphereOffset = 0.2f;
    public float minimumCollisionOffset = 0.2f;
    public Transform targetObject; //Object which the camera is tied to (Apply to an Empty Object which is a Child of the Target)
    public Vector2 pitchMinMax = new Vector2(-5, 85); //A Value used to calmp the Maximum and Minimum Pitch Values. X Value is the Min, Y Value is the Max.

    float yaw; //Movement in the X Direction
    float pitch; //Movement in the Y Direction
    Vector3 rotationSmoothVelocity; //DONT MODIFY OURSELVES, used for SmoothDampAngle calculations
    Vector3 currentRotation; //Current rotation of the camera around the target object

    // Start is called before the first frame update
    void Start()
    {
        if(lockCursor) //Checks to see if lockCursor is true
        {
            Cursor.lockState = CursorLockMode.Locked; //Locks the cursor to the centre of the screen
            Cursor.visible = false; //Makes the cursor invisible
        }
    }

    void Update()
    {
        //CollisionDetection();
    }

    //Called after all of the other Update Methods are run
    void LateUpdate()
    {
        //Each frame, Yaw and Pitch will be updated based on Mouse Input

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity; //Changes Yaw based on horizontal mouse input
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity; //Changes Pitch based on Verticle mouse input (Subtracted so Y-Input is not inverted)
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y); //Clamps the value of pitch to the Min/Max values stored in pitchMinMax

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime); //Calculates the current rotation

        transform.eulerAngles = currentRotation; //Sets the eulerAngles of the Camera to the value of targetRotation

        transform.position = targetObject.position - transform.forward * targetDistance; //Moves the transform position of the Camera to follow the player
    }

    /*
    void CollisionDetection()
    {
        Ray ray = new Ray(transform.position, transform.forward); //Ray used to draw a line from the camera position in the Camera's forward direction
        RaycastHit hitInfo; //A variable which stores information of the raycast collision (RaycastHit is struct, need to use 'out' to have Raycast method assign to hitInfo

        //CURRENTLY BUGGY, DISTANCE KEEPS CHANGING BACK AND FORTH. MAY NEED TO IMPLAMENT A NEW SYSTEM USING COLLIDERS INSTEAD

        if(Physics.Raycast(ray, out hitInfo, maxDistance)) //Checks if the ray hit something
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red); //Draws line inside of editor from origin to Hit Point(NOT IN BUILD)

            if(hitInfo.collider.tag != "Player") //If the Raycast doesn't hit the player
            {
                if((targetDistance != targetDistance - hitInfo.distance) && (targetDistance > minDistance)) //Checks if Distance of the camera to the player
                {
                    targetDistance = targetDistance - hitInfo.distance;
                }

                if(targetDistance < minDistance) //Checks if Distance of the camera to the player is lower then the minimum
                {
                    targetDistance = minDistance; //Changes the distance to the minimum amount
                }
            }
            else
            {
                targetDistance = maxDistance; //Changes the distance of the camera to the player to the Maximum amount if player is seen again
            }
        }
        else
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100.0f, Color.blue); //Draws line inside of editor from origin to direction ray is pointing in (NOT IN BUILD)
    }
    */
}

/*
 * References:
 *      - https://youtu.be/sNmeK3qK7oA
 *          - Video used to create this Third Person Camera Controller. Use this to create a basic controller and add our own custom functions
 *          where needed.
 *      - https://youtu.be/fFq5So-UB0E
 *          - Used to implament Raycasting
 *      - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/ref
 *          - The ref keyword indicates a value that is passed by reference. It is used in four different contexts:
 *              - In a method signature and in a method call, to pass an argument to a method by reference. For more information, see Passing an argument by reference.
 *              - In a method signature, to return a value to the caller by reference. For more information, see Reference return values.
 *              - In a member body, to indicate that a reference return value is stored locally as a reference that the caller intends to modify or, in general, a local 
 *              variable accesses another value by reference. For more information, see Ref locals.
 *              - In a struct declaration to declare a ref struct or a readonly ref struct. For more information, see the ref struct section of the Structure types article.
 *          - When used in a method's parameter list, the ref keyword indicates that an argument is passed by reference, not by value.
 *      - https://docs.unity3d.com/ScriptReference/Debug.DrawLine.html
 *          - Draws a line between specified start and end points. The line will be drawn in the Game view of the editor when the game is running and the gizmo drawing is 
 *          enabled. The line will also be drawn in the Scene when it is visible in the Game view. Leave the game running and showing the line. Switch to the Scene view 
 *          and the line will be visible.
 *      - https://docs.unity3d.com/ScriptReference/RaycastHit.html
 *          - Structure used to get information back from a raycast.
 *      - https://docs.unity3d.com/ScriptReference/Physics.Linecast.html
 *          - Returns true if there is any collider intersecting the line between start and end.
 *      - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/out-parameter-modifier
 *          - The out keyword causes arguments to be passed by reference. It makes the formal parameter an alias for the argument, which must be a variable.
 *      - https://www.tutlane.com/tutorial/csharp/csharp-pass-by-reference-ref-with-examples
 *          - In C#, passing a value type parameter to a method by reference means passing a reference of the variable to the method. So the changes made to the parameter 
 *          inside of the called method will have an effect on the original data stored in the argument variable.
 *      - https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
 *          - bool True if the ray intersects with a Collider, otherwise false. 
 *          - Casts a ray, from point origin, in direction direction, of length maxDistance, against all colliders in the Scene.
 *          - You may optionally provide a LayerMask, to filter out any Colliders you aren't interested in generating collisions with.
 *          - Specifying queryTriggerInteraction allows you to control whether or not Trigger colliders generate a hit, or whether to use the global Physics.queriesHitTriggers setting.
*/
