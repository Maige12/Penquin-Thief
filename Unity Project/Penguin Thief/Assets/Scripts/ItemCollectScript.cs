using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Item Collect Script
 *  
 *  ATTACH TO PLAYER OBJECT IN SCENE HIERARCHY
 *  
 *  Description:
 *      - This script handles the collection and depositing of items, as well as the unlocking of doors
 *              
 *  Known Bugs:
 *      - N/A
*/

public class ItemCollectScript : MonoBehaviour
{
    [HideInInspector] //Hides the value from the Inspector
    public int keys; //Player's total keys collected
    [HideInInspector] //Hides the value from the Inspector
    public int collectableSmall; //Player's total collectables collected
    [HideInInspector] //Hides the value from the Inspector
    public int collectableLarge; //Player's total collectables collected
    [HideInInspector] //Hides the value from the Inspector
    public int score; //Player's total score points

    public GameObject[] collectLarge;
    public GameObject currentLargeObj;
    public GameObject canvas;
    public int totalLarge;

    MenuManager menuManagerScript;

    void Awake()
    {
        keys = 0; //Starts keys at 0
        collectableSmall = 0; //Starts small collectables at 0
        collectableLarge = 0; //Starts large collectables at 0
        score = 0; //Starts score at 0

        menuManagerScript = canvas.GetComponent<MenuManager>();

        collectLarge = GameObject.FindGameObjectsWithTag("Collectable Large");
        totalLarge = collectLarge.Length;

        Debug.Log("Total Large Collectables is " + totalLarge);
    }

    void OnCollisionEnter(Collision collision) //Will only activate when entering collision
    {
        switch (collision.gameObject.tag) //Checks the collision tag
        {
            case "Key": //Checks if the tag was 'Key' (Usable Keys)
                if (keys < 0) //Checks to see if the current value is less than 0
                {
                    Debug.Log("Error: Value of 'keys' below 0, value equal to " + keys + ", resetting to 0"); //Warns the developer that the value is less than 0

                    keys = 0; //Resets the value if it is less than 0
                }

                keys++; //Increments the value by 1

                Debug.Log("'keys' value incremented by 1, value is now " + keys); //Outputs the current amount of keys objects to the developer console

                FindObjectOfType<AudioManager>().Play("ItemCollect"); //Uses the Audio Manager object to play the Item Collect SFX

                Destroy(collision.gameObject); //Destroys the Game Object which the collision is based from

                break;
            case "Collectable Small": //Checks if the tag was 'Collectable Small' (Collectable Items (Small objects, player can carry as many as they like))
                if (collectableSmall < 0) //Checks to see if the current value is less than 0
                {
                    Debug.Log("Error: Value of 'collectableSmall' below 0, value equal to " + collectableSmall + ", resetting to 0"); //Warns the developer that the value is less than 0

                    collectableSmall = 0; //Resets the value if it is less than 0
                }

                if (collectableSmall < 5) //Checks to see if the number of collectables the player has is less then 5 (The Maximum)
                {
                    collectableSmall++; //Increments the value by 1

                    menuManagerScript.UICollectToggle(collectableSmall - 1, true, false);

                    Debug.Log("'collectableSmall' value incremented by 1, value is now " + collectableSmall); //Outputs the current amount of collectableSmall objects to the developer console

                    FindObjectOfType<AudioManager>().Play("ItemCollect"); //Uses the Audio Manager object to play the Item Collect SFX

                    Destroy(collision.gameObject); //Destroys the Game Object which the collision is based from
                }
                else
                {
                    Debug.Log("The player is collecting the Maximum amount of Small Objects and must deposit them"); //Tells the developer that the player is st the max limit for Small Objects
                }

                break;
            case "Collectable Large": //Checks if the tag was 'Collectable Large' (Collectable Items (Large objects, player can only carry one at a time))
                if ((collectableLarge < 0) || (collectableLarge > 1)) //Checks to see if the current value is less than 0
                {
                    Debug.Log("Error: Value of 'collectableLarge' outisde of range, value equal to " + collectableLarge + ", resetting to 0"); //Warns the developer that the value is less than 0

                    collectableLarge = 0; //Resets the value if it is less than 0
                }

                if (collectableLarge == 0) //Checks to see if the current value is equal to 0
                {
                    collectableLarge++; //Increments the value by 1

                    menuManagerScript.UICollectToggle(collectableLarge - 1, false, false);

                    Debug.Log("'collectableLarge' value incremented by 1, value is now " + collectableLarge); //Outputs the current amount of collectableLarge objects to the developer console

                    currentLargeObj = collision.gameObject;

                    FindObjectOfType<AudioManager>().Play("ItemCollect"); //Uses the Audio Manager object to play the Item Collect SFX

                    currentLargeObj.SetActive(false); //Sets the currently picked up Large Object to be Inactive

                    //Destroy(collision.gameObject); //Destroys the Game Object which the collision is based from
                }
                else
                {
                    Debug.Log("The player is already carrying a Large Object and must deposit it"); //Tells the developer that the player is already carrying a large object
                }

                break;
            case "Locked Door": //Checks if the tag was 'Locked Door' (Locked Doors)
                if (keys > 0)
                {
                    Debug.Log("The player has a key, unlocking door"); //Tells the developer that the player is opening a door

                    Destroy(collision.gameObject); //Destroys the Game Object which the collision is based from

                    keys--; //Decrements the value by 1

                    Debug.Log("The player has used a key, they now have " + keys + " keys left"); //Tells the developer that the player has used a key and how many keys they have left
                }
                else
                {
                    Debug.Log("The Player has no keys to open the door"); //Tells the developer that the player has no keys
                }

                break;
        }
    }

    void OnTriggerEnter(Collider collider) //Runs when the player enters a trigger
    {
        switch (collider.gameObject.tag) //Checks the collider tag
        {
            case "Deposit": //Checks if the tag was 'Deposit' (Deposit Point)
                if ((collectableSmall > 0) || (collectableLarge > 0)) //If the player has any items to deposit, run this If Statement
                {
                    if (collectableSmall > 0)
                    {
                        Debug.Log("You have deposited " + collectableSmall + " Small Object(s)"); //Prints the amount of small objects deposited

                        menuManagerScript.UICollectToggle(0, true, true);

                        score += 100 * collectableSmall; //Adds to the total score

                        collectableSmall = 0; //Removes all small objects from player inventory

                        Debug.Log("Current Score: " + score); //Prints out the new score
                    }

                    if (collectableLarge > 0)
                    {
                        Debug.Log("You have deposited " + collectableLarge + " Large Object"); //Prints the amount of large objects deposited

                        menuManagerScript.UICollectToggle(0, false, true);

                        score += 500; //Adds to the total score

                        collectableLarge = 0; //Removes all large objects from player inventory

                        Destroy(currentLargeObj);

                        totalLarge -= 1;

                        Debug.Log("Current Score: " + score); //Prints out the new score

                        if (totalLarge == 0)
                        {
                            menuManagerScript.ResultsScreen(score);
                        }
                    }

                    FindObjectOfType<AudioManager>().Play("ItemDeposit"); //Uses the Audio Manager object to play the Item Deposit SFX
                }
                else
                {
                    Debug.Log("The Player has no items, please collect some"); //Tells the developer that the player has no items to deposit
                }

                break;
        }
    }

    public void CaughtReset() //This function is called when being caught by the Night Guard. It resets values so you lose your collected objects
    {
        if(currentLargeObj != null)
        {
            currentLargeObj.SetActive(true); //Sets the currently picked up Large Object to be Active so it can be picked up again 

            menuManagerScript.UICollectToggle(0, true, true); //Resets the Small Collectable UI Elements
            menuManagerScript.UICollectToggle(0, false, true); //Resets the Large Collectable UI Elements

            currentLargeObj = null;
        }

        collectableLarge = 0; //Removes all large objects from player inventory
        collectableSmall = 0; //Removes all small objects from player inventory
    }
}