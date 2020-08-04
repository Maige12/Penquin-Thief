using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))] //OrbitCameraController requires that the GameObject attached has a Camera component
public class OrbitCameraController : MonoBehaviour
{
    [SerializeField] //Allows it to show up in the inspector
    Transform focus = default; //The object that the camera will focus on

    [SerializeField, Range(1f, 20f)] //Clamps the camera distance to the player from 1 to 20
    float distance = 5.0f; //The distance the camera can be from the player

    void LateUpdate() //LateUpdate is called after all Update functions have been called.
    {
        Vector3 focusPoint = focus.position; //The point to focus on is equal to the position of the focus object
        Vector3 lookDirection = transform.forward; //The direction for the camera to look at is equal to the focus objects forward vector
        transform.localPosition = focusPoint - lookDirection * distance; //The transform of the camera is equal to the focal point, minus the look direction, multiplied by the distance
    }
}
