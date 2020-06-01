using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Adds SceneManager functionality

public class GameOverScript : MonoBehaviour
{
    static public void OpenGameOver() //Turns on the Pause Menu, Freezes Time
    {
        Debug.Log("GAME OVER");
        Time.timeScale = 0; //Freezes time so game freezes

        UIInitialiserScript.GetPauseMenuObj.SetActive(false); //Sets the Pause Menu object to inactive so it can't be interacted with
        UIInitialiserScript.GetPlayerUI.SetActive(false); //Sets the Player UIu object to inactive so it can't be interacted with
        UIInitialiserScript.GetGameOverScreen.SetActive(true); //Sets the Game Over object to active so it can be interacted with

        PauseMenuUI.canPause = false;
        EndScreenScript.gameEnd = true;

        Cursor.lockState = CursorLockMode.Confined; //Locks cursor to the game view (Cannot move mouse outside of it, only works in BUILD, not EDITOR)
        Cursor.visible = true; //Shows Cursor
    }

    static public void RestartGame() //Restarts the game (DOES NOT WORK CURRENTLY, FREEZES GAME. FIX LATER (HIDE BUTTON FOR NOW))
    {
        Debug.Log("Resetting the Game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Loads the current Scene, resetting the game (IMPLEMENT BETTER SYSTEM LATER)
    }

    public void RestartButton() //Used for the OnClick() funtionality in the restartGameButton Object (Can only use functions with one parameter)
    {
        RestartGame(); //Accesses ResetGane() Function
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