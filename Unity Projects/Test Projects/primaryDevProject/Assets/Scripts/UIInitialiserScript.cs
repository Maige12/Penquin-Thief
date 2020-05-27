using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Brought in to control the UI Elements
using TMPro; //Includes commands for TextMesh Pro

/*
 *  UI Initialiser Script (VER 1, 13-05-2020) 
 *  
 *  ATTACH TO 'Canvas' OBJECT IN SCENE HIERARCHY
 *  
 *  NOTE: Make sure all UI Elements are set to Active in the scene so that the GameObject Parameters can be initialised. This Script will handle turning these
 *      objects OFF/leaving them ON once they have all been assigned to their respective GameObjects.
 *  
 *  Description:
 *      - This script initialises various UI GameObject Parameters, creating a centralised file for Initialisation, Getting and Setting of parameters 
 *      for the Games various UI Elements.
 *      
 *      This Script should be attached to the Scenes 'Canvas' Object.
 *      
 *  Changelog:
 *      - 13-05-2020 (Darcy Wilson, 32926762):
 *          - Created UIInitialiserScript Script
 *          - Created GetPauseMenuObj & GetPlayerUI GET/SET Functions
 *          - Created CheckParameters Function
 *          - Code fully commented
 *          - Tested and Currently Works
 *              - AS LONG AS ALL UI OBJECTS IN HIERARCHY ARE SET TO ACTIVE
 *      - 15-05-2020 (Darcy Wilson, 32926762):
 *          - Created usableItems & collectedItems GameObject
 *          - Created GetUsableItems & GetCollectedItems GET/SET Functions
 *          - Added For Loop to Start() to initialise GetUsableItems & GetCollectedItems
 *          - Added For Loop Check to CheckParameters() to make sure Arrays have no NULL Values
 *              
 *  Known Bugs:
 *      - N/A
 *      
 *  Recommended Canvas/EventSystem Hierarchy:
 *      Canvas
 *          playerUI (Tagged 'Player UI')
 *              usableItems
 *                  usableItem0
 *                  usableItem1
 *                  usableItem2
 *                  usableItem3
 *              CollectedItemsUI
 *                  collectedItems0
 *                  collectedItems1
 *                  collectedItems2
 *                  collectedItems3
 *          pauseMenu (Tagged 'Pause Menu')
 *              pauseMenuBackground
 *                  resumeGameButton
 *                      Text (TMP)
 *                  quitGameButton
 *                      Text (TMP)
 *      EventSystem
 *          
 *  NOTE: Names/Tags used in example Hierarchy are used in Script to find objects, please use this example when naming/tagging objects in a scene
*/

public class UIInitialiserScript : MonoBehaviour
{
    private static GameObject pauseMenuObj, endScreenObj, playerUI; //Private Variables to store UI GameObjects (Cannot be changed outside of Class)

    public static GameObject GetPauseMenuObj //GetPauseMenuObj is a GET/SET Method. Used to Initialise Pause Menu UI Object (pauseMenu in Hierarchy)
    {
        get { return pauseMenuObj; } //Used to get value when GetPauseMenuObj is called
        set {   if(pauseMenuObj == null) //Only SETS if the object is empty
                {
                    pauseMenuObj = GameObject.FindWithTag("Pause Menu");
                }
            } //Sets pauseMenuObj to GameObject with specific Tag
    }

    public static GameObject GetEndScreenObj //GetEndScreenObj is a GET/SET Method. Used to Initialise End Screen UI Object (endScreen in Hierarchy)
    {
        get { return endScreenObj; } //Used to get value when GetEndScreenObj is called
        set {   if (endScreenObj == null) //Only SETS if the object is empty
                {
                    endScreenObj = GameObject.FindWithTag("End Screen");
                }
            } //Sets endScreenObj to GameObject with specific Tag
    }

    public static GameObject GetPlayerUI //GetPlayerUI is a GET/SET Method. Used to Initialise Player UI Object (playerUI in Hierarchy)
    {
        get { return playerUI; } //Used to get value when of GetPlayerUI when GetPlayerUI is called
        set {   if(playerUI == null) //Only SETS if the object is empty
                {
                    playerUI = GameObject.FindWithTag("Player UI");
                }
            } //Sets playerUI to GameObject with specific Tag
    }

    // Start is called before the first frame update
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; //Locks cursor to the centre of the screen
        Cursor.visible = false; //Hides cursor from view

        GetPauseMenuObj = gameObject; //Calls GET/SET Function for GetPauseMenuObj to initialise it
        GetEndScreenObj = gameObject; //Calls GET/SET Function for GetEndScreenObj to initialise it
        GetPlayerUI = gameObject; //Calls GET/SET Function for GetPlayerUI to initialise it (UNCOMMENT WHEN NEW UI SYSTEM IS SET UP)

        CheckParameters(); //Calls CheckParameters() function to make sure values have been set
    }

    void CheckParameters() //A Function used to make sure values have been set. If GameObject variables have NULL Value, prints out Error to Log and Exits Game
    {
        if (GetPauseMenuObj != null) //Checks to see if GameObject is set to NULL
        {
            Debug.Log("Object paired to pauseMenuObj successfully!");
        }
        else
        {
            Debug.LogError("Unable to find object with Tag 'Pause Menu'", pauseMenuObj);
            Application.Quit(); //Closes the Game
        }

        GetPauseMenuObj.SetActive(false); //Sets GetPauseMenuObj to Inactive (Should be hidden when starting game)

        if (GetEndScreenObj != null) //Checks to see if GameObject is set to NULL
        {
            Debug.Log("Object paired to endScreenObj successfully!");
        }
        else
        {
            Debug.LogError("Unable to find object with Tag 'End Screen'", endScreenObj);
            Application.Quit(); //Closes the Game
        }

        GetEndScreenObj.SetActive(false); //Sets GetEndScreenObj to Inactive (Should be hidden when starting game)

        if (GetPlayerUI != null) //Checks to see if GameObject is set to NULL
        {
            Debug.Log("Object paired to playerUI successfully!");
        }
        else
        {
            Debug.LogError("Unable to find object with Tag 'Player UI'", playerUI);
            Application.Quit(); //Closes the Game
        }
    }
}

/*
 * References:
 *      - https://docs.unity3d.com/ScriptReference/Debug.LogError.html
 *      - https://www.w3schools.com/cs/cs_properties.asp
 *      - https://docs.unity3d.com/ScriptReference/Application.Quit.html
 *      - https://docs.unity3d.com/ScriptReference/GameObject.FindWithTag.html
 *      - https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html
 *      - https://www.w3schools.com/cs/cs_arrays.asp
 *      - https://docs.unity3d.com/ScriptReference/GameObject.Find.html
 *      - https://stackoverflow.com/questions/40595148/how-to-make-a-property-with-a-if-statement
 *      - https://docs.unity3d.com/ScriptReference/Cursor-lockState.html
 *      - https://docs.unity3d.com/ScriptReference/Cursor-visible.html
*/
