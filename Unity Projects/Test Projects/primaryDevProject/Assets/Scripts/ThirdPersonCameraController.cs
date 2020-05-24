using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public float mouseSensitivity = 10.0f; //A modifier which adjusts the mouse sensitivity
    public Transform targetObject; //Object which the camera is tied to (Apply to an Empty Object which is a Child of the Target)
    public float targetDistance = 1.0f; //How far the camera should be from the target
    public Vector2 pitchMinMax = new Vector2(-5, 85); //A Value used to calmp the Maximum and Minimum Pitch Values. X Value is the Min, Y Value is the Max.
    public float rotationSmoothTime = 0.1f; //Approximate number of seconds for SmoothDampAngle to go from current value to target value (ROTATION)

    float yaw; //Movement in the X Direction
    float pitch; //Movement in the Y Direction
    Vector3 rotationSmoothVelocity; //DONT MODIFY OURSELVES, used for SmoothDampAngle calculations
    Vector3 currentRotation; //Current rotation of the camera around the target object

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
}

/*
 * References:
 *      - https://youtu.be/sNmeK3qK7oA
 *          - Video used to create this Third Person Camera Controller. Use this to create a basic controller and add our own custom functions
 *          where needed.
*/
