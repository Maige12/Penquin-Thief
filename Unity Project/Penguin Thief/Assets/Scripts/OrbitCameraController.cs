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
    float distance = 5.0f; //The distance the camera can be from the player
    [SerializeField, Min(0.0f)] //Clamps the value so it cannot go below 0
    float focusRadius = 1.0f; //The radius in which the camera will start moving if its focus point differs from the radius
    [SerializeField, Range(0.0f, 1.0f)] //Clamps the value from a range of 0 to 1
    float focusCentering = 0.75f; //The speed at which the camera will center itself abck to the player
    [SerializeField, Range(1.0f, 360.0f)] //Clamps the value from a range of 1 to 360
    float rotationSpeed = 90.0f; //The speed at which the camera rotates around the player (In degrees per-second)
    [SerializeField, Range(-89f, 89f)] //Clamps the value from a range of -89 to 89
    float minVerticalAngle = -30f, maxVerticalAngle = 60f; //The minimum and maximum values that the camera can rotate vertically

    Vector2 orbitAngles = new Vector2(45f, 0f); //Vertical (Pitch) angle, Horizontal (Yaw) angle
    Vector3 focusPoint; //The point that the camera will focus on

    void Awake()
    {
        Vector3 focusPoint = focus.position; //The point to focus on is equal to the position of the focus object

        Cursor.lockState = CursorLockMode.Locked; //Locks the cursor to the centre of the screen
        Cursor.visible = false; //Makes the cursor invisible
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

        Quaternion lookRotation = Quaternion.Euler(orbitAngles); //The current rotation of the camera
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

    void ManualRotation() //Handles the manual rotation of the camera
    {
        Vector2 input = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")); //This Vector2 handles the inputs from
        const float e = 0.001f;

        if (input.x < -e || input.x > e || input.y < -e || input.y > e)
        {
            orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
        }
    }

    void ConstrainAngles() //Constrains the angles at which the player can rotate the camera
    {
        orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

        if (orbitAngles.y < 0f)
        {
            orbitAngles.y += 360f;
        }
        else if (orbitAngles.y >= 360f)
        {
            orbitAngles.y -= 360f;
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
 *      Catlike Coding (Primary Tutorial):
 *          - https://catlikecoding.com/unity/tutorials/movement/orbit-camera/
*/
