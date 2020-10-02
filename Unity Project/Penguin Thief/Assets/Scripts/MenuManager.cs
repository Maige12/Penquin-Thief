using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //Adds the Text Mesh Pro Library

public class MenuManager : MonoBehaviour
{
    [SerializeField] //Allows me to set the object through the Inspector
    GameObject infoScreenObj; //The Info Screen canvcas object
    [SerializeField] //Allows me to set the object through the Inspector
    GameObject pauseScreenObj; //The Pause Screen canvcas object
    [SerializeField] //Allows me to set the object through the Inspector
    GameObject resultScreenObj; //The Result Screen canvcas object

    [SerializeField] //Allows me to set the object through the Inspector
    TextMeshProUGUI finalPoints; //The final point total that the player has

    GameObject currentMenu; //The current menu that the player is on

    [HideInInspector] //Hides the value from the Inspector
    public bool menuOpen; //Checks to see if the Menu is Open
    [HideInInspector] //Hides the value from the Inspector
    public bool gameWin; //Checks to see if the Game has been won (Used to stop inputs for Pausing)
    public bool infoScreen;

    // Start is called before the first frame update
    void Start()
    {
        if(pauseScreenObj == null)
        {
            pauseScreenObj = GameObject.Find("Pause Screen");

            infoScreenObj.SetActive(false);
        }
        
        if(infoScreenObj == null)
        {
            infoScreenObj = GameObject.Find("Level Info");

            infoScreenObj.SetActive(false); //Sets the Info Screen to be inactive
        }

        if(resultScreenObj == null)
        {
            resultScreenObj = GameObject.Find("Results Screen");

            resultScreenObj.SetActive(false);
        }

        menuOpen = false;
        gameWin = false;

        if(pauseScreenObj == true)
        {
            pauseScreenObj.SetActive(false);
        }

        if (infoScreen == true)
        {
            menuOpen = true; //Tells the game a menu is open

            infoScreenObj.SetActive(true); //Sets the Info Screen to be active
            currentMenu = infoScreenObj;

            Time.timeScale = 0; //Freezes time so game freezes

            Cursor.lockState = CursorLockMode.Confined; //Locks cursor to the game view
            Cursor.visible = true; //Shows Cursor
        }
        else
        {
            infoScreenObj.SetActive(false); //Sets the Info Screen to be inactive

            menuOpen = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.Escape)) && (gameWin == false))
        {
            switch(menuOpen)
            {
                case true:
                    Debug.Log("ESC Key has been pressed. Closing Menu");

                    ActivateScene();

                    break;
                case false:
                    Debug.Log("ESC Key has been pressed. Opening Menu");

                    PauseGame();

                    break;
            }
        }
    }

    public void PauseGame()
    {
        if(pauseScreenObj == null)
        {
            currentMenu = GameObject.Find("Pause Screen");
        }
        else
            currentMenu = pauseScreenObj;

        currentMenu.SetActive(true);

        DeactivateScene(); //Deactivates Player functions
    }

    public void ResultsScreen(int total)
    {
        DeactivateScene(); //Deactivates Player functions

        gameWin = true;

        resultScreenObj.SetActive(true);

        finalPoints.SetText(total.ToString());
    }

    public void DeactivateScene() //Deactivates Player Movement, Physics and Animation
    {
        Debug.Log("Deactivating Scene");

        Time.timeScale = 0; //Freezes time so game stops

        Cursor.lockState = CursorLockMode.Confined; //Locks cursor to the game view
        Cursor.visible = true; //Shows Cursor

        menuOpen = true; //Tells the system the player is in a Menu
    }

    public void ActivateScene() //Reactivates Player Movement, Physics and Animation
    {
        Debug.Log("Reactivating Scene");

        Time.timeScale = 1; //Unfreezes time so game starts again

        Cursor.lockState = CursorLockMode.Locked; //Locks the cursor to the center of the screen
        Cursor.visible = false; //Hides Cursor from the player

        menuOpen = false; //Tells the system the player is no longer in a Menu

        if(currentMenu != null) //Checks to see if the current menu is set to Null
        {
            currentMenu.SetActive(false); //Seys the current menu to Inactive
        }

        currentMenu = null; //Clears out the current meny value
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");

        Application.Quit(); //Quits the game
    }
}