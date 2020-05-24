using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Pause Menu UI Script (VER 3, 13-05-2020) 
 *  
 *  ATTACH TO 'pauseMenu' OBJECT IN SCENE HIERARCHY
 *  
 *  NOTE: Make sure pauseMenu Object (Or Object that this script is attached to) is Active in the scene Hierarchy before starting. Currently theres a bug where the 
 *      if the object is not Active, the Script will not start and will not assign anything to the pauseMenuObj Object Parameter, causing a NULL Reference Value
 *      to be returned.
 * 
 *  Description:
 *      - This script controls the Pause Menu UI and it's various functions. It will freeze the gameplay and bring up the Pause Menu for
 *      the player to use. Some buttons included with the Pause UI should be:
 *          - Resume Game (Returns to game, can also be activated by pressing 'ESC' Key)
 *          - Options (Allows the player to change options on the fly (Resolution, Graphics Settings, Sound, etc.))
 *          - Exit Game (Allows the Player to Exit the game, Prompts them if they are sure before confirming)
 *      
 *      Can be activated/deactivated by pressing the 'ESC' key.
 *      
 *  Changelog:
 *      - 11-05-2020 (Darcy Wilson, 32926762):
 *          - Restarted the project (VER 1 was not working, strange Console Issues)
 *          - Added pauseMenuObj & pause parameters
 *          - Added Start(), OpenPauseMenu(), ContinueGame() & QuitGame() Fuunctions
 *          - Added to references list (BOTTOM OF SCRIPT FILE)
 *          - Tested and currently working in Unity 2019.2.17f1
 *          - THINGS TO DO:
 *              - Add Button Functionality (Access Options, Quit Game, Resume Game, etc.)
 *      - 13-05-2020 (Darcy Wilson, 32926762):
 *          - Added Button functionality in the form of QuitButton() & ContButton() Functions
 *              - Added seperatly from functions due to OnClick() Functionality only allowing for functions with One Parameter
 *              - Make Sure to Link the OnClick() Command to QuitButton() & ContButton() respectivly
 *      - 15-05-2020 (Darcy Wilson, 32926762):
 *          - Added UIInitialiserScript.GetPlayerUI.SetActive() to OpenPauseMenu() & ContinueGame() Functions
 *              
 *  Known Bugs:
 *      - 14-05-2020 (Darcy Wilson, 32926762):
 *          - When Entering the Pause Menu for the first time, Player's need to press ESC Twice to exit (ONLY OCCURS IN BUILD VERSION)
 *              - Only happens the first time, does not happen any other time
 *                  - Seems to call the OpenPauseMenu() Function on the initial opening and on the first press, unsure why
 *              - Could possibly be an issue in the Player Controller Script
 *              - Issue does not occur if the player uses the Resume Game button
 *              
 *  NOTE: KEEP THIS SCRIPT WITH ALL OF THE OTHER SCRIPTS, ESPECIALLY THE PLAYER CONTROLLER SCRIPT AS THAT HAS ACCESS TO THIS SCRIPT
 *          TO START OpenPauseMenu() FUNCTION! 
 *          
 *          ADD THESE LINES TO PLAYER CONTROLLER'S UPDATE FUNCTION FOR SCRIPT TO WORK:
 *          
 *          if (Input.GetKeyDown(KeyCode.Escape) && PauseMenuUI.pause == false)
 *          {
 *              PauseMenuUI.OpenPauseMenu();
 *          }
 *          else
 *              if(Input.GetKeyDown(KeyCode.Escape) && PauseMenuUI.pause == true)
 *              {
 *                  PauseMenuUI.ContinueGame();
 *              }
*/

public class PauseMenuUI : MonoBehaviour
{
    public static bool pause; //A Boolean which keeps track if the player has paused the game or not. Value is changed from Player Script (CHANGE TO NAME OF PLAYER CONTROLLER SCRIPT)

    private void Awake()
    {
        pause = false; //Sets Pause State to False by Default
    }

    static public void OpenPauseMenu() //Turns on the Pause Menu, Freezes Time
    {
        Debug.Log("Pausing Game");
        pause = true;
        Time.timeScale = 0; //Freezes time so game freezes
        //Sets the boolean to true when the menu is open, stopping the player from moving
        UIInitialiserScript.GetPauseMenuObj.SetActive(true); //Sets the Pause Menu object to active so it can be interacted with
        //UIInitialiserScript.GetPlayerUI.SetActive(false); //Sets the Player UI to false (UNCOMMENT WHEN NEW UI SYSTEM IS SET UP)
        Cursor.lockState = CursorLockMode.Confined; //Locks cursor to the game view (Cannot move mouse outside of it, only works in BUILD, not EDITOR)
        Cursor.visible = true; //Shows Cursor
        Debug.Log("pauseMenuObj set to Active");
        Debug.Log("playerUI set to Inactive");
    }

    static public void ContinueGame() //Turns off the Pause Menu, Unfreezes Time
    {
        Debug.Log("Resuming Game");
        UIInitialiserScript.GetPauseMenuObj.SetActive(false); //Sets the Pause Menu to false
        //UIInitialiserScript.GetPlayerUI.SetActive(true); //Sets the Player UI to true (UNCOMMENT WHEN NEW UI SYSTEM IS SET UP)
        Cursor.lockState = CursorLockMode.Locked; //Locks Cursor to center of screen
        Cursor.visible = false; //Hides cursor
        Debug.Log("pauseMenuObj set to Inactive");
        Debug.Log("playerUI set to Active");
        Time.timeScale = 1; //Sets game time back to normal time
        pause = false;
    }

    public void ContButton() //Used for the OnClick() funtionality in the resumeGameButton Object (Can only use functions with one parameter)
    {
        ContinueGame(); //Accesses ContinueGame() Function
    }

    static public void QuitGame() //Quits the game
    {
        Debug.Log("Quitting the Game");
        Application.Quit(); //Closes the game window
    }

    public void QuitButton() //Used for the OnClick() funtionality in the quitGameButton Object (Can only use functions with one parameter)
    {
        QuitGame(); //Accesses QuitGame() Function
    }
}

/*
 * References:
 *      - https://docs.unity3d.com/ScriptReference/Time-timeScale.html
 *      - https://docs.unity3d.com/ScriptReference/GameObject-activeInHierarchy.html
 *      - https://docs.unity3d.com/ScriptReference/Application.Quit.html
 *      - https://docs.unity3d.com/ScriptReference/Debug.Log.html
 *      - https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html
 *      - https://docs.unity3d.com/ScriptReference/Cursor-lockState.html
*/
