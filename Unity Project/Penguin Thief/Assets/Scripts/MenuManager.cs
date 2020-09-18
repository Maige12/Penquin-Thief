using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    GameObject infoScreenObj;
    GameObject currentMenu;

    [HideInInspector]
    public bool menuOpen;
    public bool infoScreen;

    // Start is called before the first frame update
    void Start()
    {
        if(infoScreenObj == null)
        {
            infoScreenObj = GameObject.Find("Level Info");

            infoScreenObj.SetActive(false); //Sets the Info Screen to be inactive

            menuOpen = false;
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch(menuOpen)
            {
                case true:
                    Debug.Log("ESC Key has been pressed. Closing Menu");

                    ActivateScene();

                    break;
                case false:
                    Debug.Log("ESC Key has been pressed. Opening Menu");

                    DeactivateScene();

                    break;
            }
        }
    }

    public void DeactivateScene()
    {
        Debug.Log("Deactivating Scene");

        Time.timeScale = 0; //Freezes time so game stops

        Cursor.lockState = CursorLockMode.Confined; //Locks cursor to the game view
        Cursor.visible = true; //Shows Cursor

        menuOpen = true;
    }

    public void ActivateScene()
    {
        Debug.Log("Reactivating Scene");

        Time.timeScale = 1; //Unfreezes time so game starts again

        Cursor.lockState = CursorLockMode.Locked; //Locks the cursor to the center of the screen
        Cursor.visible = false; //Hides Cursor from the player

        menuOpen = false;

        if(currentMenu != null)
        {
            currentMenu.SetActive(false);
        }
    }
}