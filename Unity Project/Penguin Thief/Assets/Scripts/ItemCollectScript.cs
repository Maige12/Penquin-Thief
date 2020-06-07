using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectScript : MonoBehaviour
{
    public int keys; //Player's total keys collected
    public int collectables; //Player's total collectables collected
    public int score; //Player's total score points

    PlayerUIScript playerUI = new PlayerUIScript(); //Makes an object reference to PlayerUIScript.cs

    void Awake()
    {
        keys = 0; //Starts keys at 0
        collectables = 0; //Starts collectables at 0
    }

    void OnCollisionEnter(Collision collision) //Will only activate when entering collision
    {
        switch (collision.gameObject.tag) //Checks the collision tag
        {
            case "Key": //Checks if the tag was 'Key' (Usable Keys)
                Debug.Log("You collected " + collision.gameObject.name + ", adding key!"); //Prints to the Debug Log which item is collected (Key)
                keys++; //Adds 1 key to the count
                playerUI.UpdateKeys(keys);
                Destroy(collision.gameObject); //Destroys the game object
                break;
            case "Collectable": //Checks if the tag was 'Collectable' (Collectable Items)
                Debug.Log("You collected " + collision.gameObject.name + ", adding collectable!"); //Prints to the Debug Log which item is collected (Collectable)
                collectables++; //Adds 1 collectable to the count
                playerUI.UpdateCollectables(collectables);
                Destroy(collision.gameObject); //Destroys the game object
                break;
            case "Deposit":
                if(collectables != 0) //Checks if the player has items to deposit
                {
                    Debug.Log("You deposited " + collectables + " items, good job!"); //Prints to the Debug Log how many items were deposited
                    score = 500 * collectables; //Adds 500 points to score
                    collectables = 0; //Sets the number of collectables back to 0
                    playerUI.UpdateCollectables(collectables);
                }
                else
                    Debug.Log("You have " + collectables + " items to deposit, go find some!"); //Prints to the Debug Log if theres no items to collect
                break;
            case "Locked Door":
                if (keys != 0) //Checks if the player has any keys left
                {
                    Debug.Log("You unlocked a door"); //Prints to the Debug Log when a door is unlocked
                    keys--; //Takes one key from the player
                    playerUI.UpdateKeys(keys);
                    Destroy(collision.gameObject); //Destroys the game object
                }
                else
                    Debug.Log("You have no keys, go find some!"); //Prints to the Debug Log if the player has no keys
                break;
        }
    }
}
