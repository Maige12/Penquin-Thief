using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Player UI Script (VER 1, 15-05-2020) 
 *  
 *  NOTE: NEED TO ADD SCROLL FUNCTIONALITY
 *  
 *  Description:
 *      - This script controls the visibility/positions of various elements for the Player's UI, these functions including:
 *          - Switching the UI Icons for the Usable Items (Controlled by Scroll Wheel)
 *          - Controlling Collected Item UI Element
 *      
 *  Changelog:
 *      - 15-05-2020 (Darcy Wilson, 32926762):
 *          - Created ChangeUsableItem()
 *              
 *  Known Bugs:
 *      - No Scroll Wheel Functionality
 *      - No Collected Item Functionality
*/

public class PlayerUIScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if((Input.mouseScrollDelta.y > 0f) || (Input.mouseScrollDelta.y < 0f)) //Checks to see if the Scroll Wheel has been used before activating ChangeUsableItem()
        {
            ChangeUsableItem();
        }
    }

    void ChangeUsableItem() //Changes Usable Item UI Position
    {
        if (Input.mouseScrollDelta.y > 0f) //If the Scroll Wheel is scrolled up
        {
            Debug.Log("Scrolling Up");
        }

        if (Input.mouseScrollDelta.y < 0f) //If the Scroll Wheel is scrolled down
        {
            Debug.Log("Scrolling Down");
        }
    }
}

/*
 * References:
 *      - https://docs.unity3d.com/ScriptReference/Input-mouseScrollDelta.html
*/
