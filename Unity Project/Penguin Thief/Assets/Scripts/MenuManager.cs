using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //Adds the Text Mesh Pro Library
using UnityEngine.SceneManagement; //Adds functionality so we can Load Scenes
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] //Allows me to set the object through the Inspector
    GameObject infoScreenObj; //The Info Screen canvas object
    [SerializeField] //Allows me to set the object through the Inspector
    GameObject pauseScreenObj; //The Pause Screen canvas object
    [SerializeField] //Allows me to set the object through the Inspector
    GameObject resultScreenObj; //The Result Screen canvas object
    [SerializeField] //Allows me to set the object through the Inspector
    GameObject inGameScreenObj; //The In-Game Screen canvas object 

    List<GameObject> smallCol; //A list of UI GameObjects for Small Collectables
    [SerializeField]
    GameObject smallCollect; //The object which is connected to the list of Small Collectable UI Objects

    List<GameObject> largeCol; //A list of UI GameObjects for Large Collectables
    [SerializeField]
    GameObject largeCollect; //The object which is connected to the list of Large Collectable UI Objects

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
        smallCol = new List<GameObject>();
        smallCol = smallCollect.GetChildren();

        largeCol = new List<GameObject>();
        largeCol = largeCollect.GetChildren();

        if (pauseScreenObj == null)
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

        if(inGameScreenObj == null)
        {
            inGameScreenObj = GameObject.Find("In Game");
        }

        menuOpen = false;
        gameWin = false;

        if (inGameScreenObj == true)
        {
            inGameScreenObj.SetActive(false);
        }

        if (pauseScreenObj == true)
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

    public void UICollectToggle(int i, bool largeSmallToggle, bool turnOff) //This will toggle all of the small objects on or off (i = item number, largeSmallToggle (True is Large, False is Small), turnOff will reset values if true)
    {
        switch(largeSmallToggle) //Checks to see if a Large or Small object is being toggled
        {
            case true: //If true, toggle Small objects 
                if (turnOff == true)
                {
                    for (int y = 0; y < smallCol.Count; y++)
                    {
                        smallCol[y].GetComponent<Image>().color = new Color32(136, 124, 255, 80); //Untints the image Colour and turns the the opacity back on
                    }
                }
                else
                {
                    smallCol[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255); //Untints the image Colour and turns the the opacity back on
                }

                break;
            case false: //If false, toggle Large objects
                if (turnOff == true)
                {
                    for (int y = 0; y < largeCol.Count; y++)
                    {
                        largeCol[y].GetComponent<Image>().color = new Color32(136, 124, 255, 80); //Untints the image Colour and turns the the opacity back on
                    }
                }
                else
                {
                    largeCol[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255); //Untints the image Colour and turns the the opacity back on
                }

                break;
        }
    }

    public void DeactivateScene() //Deactivates Player Movement, Physics and Animation
    {
        Debug.Log("Deactivating Scene");

        Time.timeScale = 0; //Freezes time so game stops

        Cursor.lockState = CursorLockMode.Confined; //Locks cursor to the game view
        Cursor.visible = true; //Shows Cursor

        inGameScreenObj.SetActive(false);

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

        inGameScreenObj.SetActive(true);

        currentMenu = null; //Clears out the current meny value
    }

    public void RestartGame() 
    {
        Debug.Log("Restarting Game");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Restarts the game by grabbing the currently active scene
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");

        Application.Quit(); //Quits the game
    }
}