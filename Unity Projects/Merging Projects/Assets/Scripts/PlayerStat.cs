using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class PlayerStats 
{
    private static int health = 10;
    public static TMPro.TextMeshProUGUI healthText;

    public static void UpdateHealth(int valueChange)
    {
        //health = health + valueChange;
        health += valueChange;

        // go find the health text in the scene heirarchy
        // we can use GameObject.Find and GameObject class because
        // up at the top we are using UnityEngine so we can reference
        // all the classes in that namespace.
        GameObject tempTextObject = GameObject.Find("Health");

        // update the text with our new value of health
        tempTextObject.GetComponent<TextMeshProUGUI>().text = "Health:" + health.ToString();
    }
}
