using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region PUBLIC FIELDS

        [Header(header: "Walk / Run Setting")]
        public float walkSpeed;
        //Walk speed of the player
        public float runSpeed;
        //Secondary Movement variable of the player

        [Header("Slide Settings")]
        public float playerSlideForce;
        public ForceMode appliedForceMode;

        [Header("Sliding State")]
        public bool playerIsSliding;
        //Checks if the player is currently sliding

        [Header("Current Player Speed")]
        public float currentSpeed;
        //tracks the current speed of the player



    #endregion

    #region PRIVATE FIELDS
        public GameObject Player;
        //The player.
        private float _xAxis;
        //Used to track the players current x axis
        private float _zAxis;
        //Used to track the players current z axis
        private Rigidbody _rb;
        //The rigid body attached to the player
        private Vector3 _groundLocation;
        private bool _isShiftPressedDown;
        // to stop multiple jumps or slides being implimented at once

    #endregion

    #region MONODEVELOP ROUTINES

        private void start()
        {
                #region INITIALISING COMPONENTS
                _rb = GetComponent<Rigidbody>();
                #endregion
                // At the start of the game, assigns variable _rb to the rigidbody attached to the player
        }

        private void Update()
        {
            #region MOVEMENT INPUT

            _xAxis = Input.GetAxis("Horizontal");
            _zAxis = Input.GetAxis("Vertical");
            #endregion
            //Assign Axis to the Axis variables
        }
        private void FixedUpdate()
        {   
            #region MOVE Player
            _rb.MovePosition(transform.position + Time.deltaTime * currentSpeed * 
                             transform.TransformDirection(_xAxis, 0f, _zAxis)); 
            #endregion
            //Move the character by changing its transform
        }    

    #endregion


}
