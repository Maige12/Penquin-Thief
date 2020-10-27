using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Orbit Camera Controller Script (RIGID) 
 *  
 *  ATTACH TO CAMERA OBJECT IN SCENE HIERARCHY
 *  
 *  NOTE: This is based off of a tutorial from Catlike Coding, please see below references for links.
 * 
 *  Description:
 *      - This script controls the camera which orbits around the player.
 *              
 *  Known Bugs:
 *      - N/A
*/

[RequireComponent(typeof(Camera))] //OrbitCameraController requires that the GameObject attached has a Camera component
public class OrbitCameraController : MonoBehaviour
{
    [SerializeField] //Allows it to show up in the inspector
    Transform focus = default; //The object that the camera will focus on
    [SerializeField, Range(1f, 20f)] //Clamps the camera distance to the player from 1 to 20
    float maxDistance = 3.0f; //The Maximum distance the camera can be from the player 
    [SerializeField, Min(0.0f)] //Clamps the value so it cannot go below 0
    float focusRadius = 1.0f; //The radius in which the camera will start moving if its focus point differs from the radius
    [SerializeField, Range(0.0f, 1.0f)] //Clamps the value from a range of 0 to 1
    float focusCentering = 0.75f; //The speed at which the camera will center itself abck to the player
    [SerializeField, Range(1.0f, 360.0f)] //Clamps the value from a range of 1 to 360
    float rotationSpeed = 90.0f; //The speed at which the camera rotates around the player (In degrees per-second)
    [SerializeField, Range(-89.0f, 89.0f)] //Clamps the value from a range of -89 to 89
    float minVerticalAngle = -10.0f, maxVerticalAngle = 60f; //The minimum and maximum values that the camera can rotate vertically
    [SerializeField, Range(0.0f, 1.0f)] //Clamps the value from a range of 0 to 1
    float smoothTime; //Used to smooth out the movement of the camera during raycast
    [SerializeField]
    bool invertX; //Inverts the X Axis of lookRotation
    [SerializeField]
    bool invertY; //Inverts the Y Axis of lookRotation
    [SerializeField]
    LayerMask interactMask; //This layer mask is used to set the specific layer that the Raycast will interact with 

    Vector2 orbitAngles = new Vector2(25f, 180f); //Vertical (Pitch) angle, Horizontal (Yaw) angle
    Vector3 focusPoint; //The point that the camera will focus on

    float distance; //The distance the camera is currently from the player

    PlayerControllerRigid playerControl; //An object representing the PlayerControllerRigid.cs script
    MenuManager menuManage; //An object representing the MenuManager.cs script

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; //Locks the cursor to the centre of the screen
        Cursor.visible = false; //Makes the cursor invisible

        invertX = false; //Sets the Invert value to False (Default State of camera's X-Rotation)
        invertY = false; //Sets the Invert value to False (Default State of camera's Y-Rotation)

        distance = maxDistance;

        GameObject player = GameObject.FindGameObjectWithTag("Player"); //Finds a GameObject with the tag 'Player'
        GameObject canvas = GameObject.Find("Canvas"); //Finds a GameObject with the name 'Canvas'

        playerControl = player.GetComponent<PlayerControllerRigid>(); //Sets playerControl Equal to the Script attached to the 'player' GameObject
        menuManage = canvas.GetComponent<MenuManager>(); //Sets menuManage Equal to the Script attached to the 'canvas' GameObject
    }

    void OnValidate() //This function is called when the script is loaded or a value is changed in the Inspector (Called in the editor only).
    {
        if (maxVerticalAngle < minVerticalAngle) //If the Maximum certical angle is less then the minimum, this will run
        {
            maxVerticalAngle = minVerticalAngle; //The maximum will be set to the value in minimum
        }
    }

    void LateUpdate() //LateUpdate is called after all Update functions have been called.
    {
        UpdateFocusPoint(); //Runs the function to adjust the focus point of the camera
        ManualRotation(); //Runs the function to adjust the cameras rotation manually
        ClearVision();

        Quaternion lookRotation; //The current rotation the camera is at from the focus point

        if(ManualRotation()) //If Manual Rotation or Automatic Rotation outputs true, run this if statement, otherwise, run the else section
        {
            ConstrainAngles(); //Constrains the angles to their limits

            lookRotation = Quaternion.Euler(orbitAngles.x, orbitAngles.y, 0); //Sets the current look rotation to the orbit angles
        }
        else
            {
                lookRotation = transform.localRotation; //Sets the look rotation to the current local rotation
            }

        Vector3 lookDirection = lookRotation * Vector3.forward; //The direction the camera is looking in
        Vector3 lookPosition = focusPoint - lookDirection * distance; //The position that the camera is in, relative to the focus point, the look at direction and the distance

        transform.SetPositionAndRotation(lookPosition, lookRotation); //Sets the world space position and rotation of the Transform component.
    }

    void UpdateFocusPoint() //This function is used to adjust the position that the camera is focusing on
    {
        Vector3 targetPoint = focus.position; //Sets the target point to the position of the target object

        if(focusRadius > 0.0f) //If the focus radius is higher then 0, then it will lag the camera slightly behind the player movement
        {
            float distance = Vector3.Distance(targetPoint, focusPoint); //The distance between the target point and current focus point
            float t = 1.0f;

            if (distance > 0.01f && focusCentering > 0.0f) //Only sets t to the needed value if the centered distance is greater than 0.01 and focusCentering is higher than 0
            {
                t = Mathf.Pow(1.0f - focusCentering, Time.unscaledDeltaTime); //Returns the first parameter raised to power of the second (Uses unscaledDeltaTime as deltaTime would be too slow)
            }

            if (distance > focusRadius) //Runs if the distance between the target point and current focus point is greater then the focus radius
            {
                t = Mathf.Min(t, focusRadius / distance); //Mathf.Min returns the smallest of two or more values.
            }

            focusPoint = Vector3.Lerp(targetPoint, focusPoint, t); //Interpolates between the points a and b by the interpolant (t is clamped to a range of 0 to 1)
        }
        else
        {
            focusPoint = targetPoint; //Sets the focus point to the position of the target point
        }
    }

    bool ManualRotation() //Handles the manual rotation of the camera (Outputs a Boolean value)
    {
        Vector2 input = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")); //This Vector2 handles the inputs from the player (Default)

        if(invertX == true) //Checks to see if the player wishes to invert the X-Axis
        {
            input.y = -Input.GetAxis("Mouse X"); //This Vector2 handles the inputs from the player (Invert X)
        }

        if (invertY == true) //Checks to see if the player wishes to invert the Y-Axis
        {
            input.x = Input.GetAxis("Mouse Y"); //This Vector2 handles the inputs from the player (Invert Y)
        }

        const float e = 0.001f; //The limit for how much  of an input the script can recieve before it starts moving the camera

        if((playerControl.slideActive == false) && (menuManage.menuOpen == false))
        {
            if (input.x < -e || input.x > e || input.y < -e || input.y > e) //Checks to see if the inputs are less than or greater than the limit
            {
                orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input; //Makes the orbit angles equal to the speed of the rotation, multiplied by unscaled delta time, multiplied by the player's input

                return true; //Returns true if there is an input
            }
        }

        return false; //Returns false if there isn't an input
    }

    void ConstrainAngles() //Constrains the angles at which the player can rotate the camera
    {
        orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle); //Clamps the given value between the given minimum float and maximum float values

        if (orbitAngles.y < 0.0f) //The horizontal orbit has no range, but this is to ensure it stays within 0 - 360 degrees
        {
            orbitAngles.y += 360.0f; //If the angle is less than 0, it will add 360 degrees
        }
        else
            if (orbitAngles.y >= 360.0f) //The horizontal orbit has no range, but this is to ensure it stays within 0 - 360 degrees
            {
                orbitAngles.y -= 360.0f; //If the angle is more than or equal to 360, it will take 360 degrees
            }
    }

    void ClearVision() //Detects if there is something in the way and clears the vision of the player
    {
        Ray ray = new Ray(focus.position, -transform.forward);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, maxDistance, interactMask))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red);

            if(hitInfo.distance > 0.5f)
            {
                distance = Mathf.Lerp(distance, hitInfo.distance, smoothTime);
            }
            else
            {
                distance = 0.5f;
            }

            //distance = hitInfo.distance;
        }
        else
        {
            if(hitInfo.distance < maxDistance)
            {
                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100.0f, Color.blue);

                distance = maxDistance;
            }
        }
    }
}

/*
 * References:
  *      Unity Documentation:
 *          - https://docs.unity3d.com/ScriptReference/Quaternion.Euler.html
 *          - https://docs.unity3d.com/ScriptReference/Vector3.Lerp.html
 *          - https://docs.unity3d.com/ScriptReference/Mathf.Min.html
 *          - https://docs.unity3d.com/ScriptReference/Mathf.Pow.html
 *          - https://docs.unity3d.com/ScriptReference/Time-unscaledDeltaTime.html
 *          - https://docs.unity3d.com/ScriptReference/Mathf.Acos.html
 *      C# References:
 *          - https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/static
 *      Mathematic Terms:
 *          - https://www.mathsisfun.com/geometry/radians.html
 *      Catlike Coding (Primary Tutorial):
 *          - https://catlikecoding.com/unity/tutorials/movement/orbit-camera/
*/
