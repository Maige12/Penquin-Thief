using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenScript : MonoBehaviour
{
    static public bool gameEnd; //A boolean to detect if the game has finished

    void Awake()
    {
        gameEnd = false; //Set to False by default
    }

    void Update()
    {
        /* UNCOMMENT ONCE A SYSTEM FOR DETETCING A FINISH POINT HAS BEEN MADE
        if (gameEnd == true && PauseMenuUI.pause == false) //Checks to see if the game has finished (gameEnd = true, Game has finished)
        {
            UIInitialiserScript.GetEndScreenObj.SetActive(true); //Sets the End Screen to True

            if(UIInitialiserScript.GetPauseMenuObj.activeInHierarchy || UIInitialiserScript.GetPlayerUI.activeInHierarchy) //Checks if Pasue Menu/Player UI is Active
            {
                UIInitialiserScript.GetPauseMenuObj.SetActive(false); //Turns off Pause Menu if it's on
                UIInitialiserScript.GetPlayerUI.SetActive(false); //Sets the Pause Menu object to active so it can be interacted with
            }

            EndScreenScript.gameEnd = true;
            Time.timeScale = 0; //Freezes time so game freezes
            Cursor.lockState = CursorLockMode.Confined; //Locks cursor to the game view (Cannot move mouse outside of it, only works in BUILD, not EDITOR)
            Cursor.visible = true; //Shows Cursor
        }
        */
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
