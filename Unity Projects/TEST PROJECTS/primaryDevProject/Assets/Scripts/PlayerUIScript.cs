using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Brought in to control the UI Elements
using TMPro; //Includes commands for TextMesh Pro

/*
 *  Player UI Script (VER 1, 15-05-2020) 
 *  
 *  ATTACH TO 'playerUI' OBJECT IN SCENE HIERARCHY
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
    public TextMeshProUGUI currentItemText;

    string[] usableItemNames; //A list of names for the Usable Items
    int arrayNumber;

    void Awake()
    {
        usableItemNames = new string[] {"Grapple Gun", "Reaching Claw", "Cart", "Screwdriver"}; //Initialising the List of Suable Item names
        arrayNumber = -1;

        currentItemText.text = "No Items";
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.mouseScrollDelta.y > 0f) || (Input.mouseScrollDelta.y < 0f)) //Checks to see if the Scroll Wheel has been used before activating ChangeUsableItem()
        {
            ChangeUsableItem(); //Changes the current usable item
        }
    }

    void ChangeUsableItem() //Changes Usable Item UI Position
    {
        if (Input.mouseScrollDelta.y > 0f) //If the Scroll Wheel is scrolled up
        {
            Debug.Log("Scrolling Up");

            if(arrayNumber < 3)
            {
                arrayNumber++; //Increases array number
                currentItemText.text = usableItemNames[arrayNumber]; //Changes the TextMeshPro Text to current item selected
            }
        }

        if (Input.mouseScrollDelta.y < 0f) //If the Scroll Wheel is scrolled down
        {
            Debug.Log("Scrolling Down");

            if (arrayNumber > 0)
            {
                arrayNumber--; //Decreases array number
                currentItemText.text = usableItemNames[arrayNumber]; //Changes the TextMeshPro Text to current item selected
            }
        }
    }
}

/*
 * References:
 *      - https://docs.unity3d.com/ScriptReference/Input-mouseScrollDelta.html
 *      - https://www.geeksforgeeks.org/c-sharp-arrays-of-strings/
*/
