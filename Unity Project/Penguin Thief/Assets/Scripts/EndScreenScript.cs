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
