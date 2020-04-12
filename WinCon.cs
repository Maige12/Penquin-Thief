using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCon : MonoBehaviour
{
    #region Attributes
    public bool winConAchieved;
    // Activates when the player has acheieved what is needed to beat the level.
    #endregion 

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            print("The winCon has been achieved");
        }
        else 
        {
            print("invalid");
        } 
        
    }
    void Start()
    {
    
    winConAchieved = true;
    //For now, the level starts with the win condition acheived


    }
}
