using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PLEASE COMMENT THIS CODE

public class ObjectClicker : MonoBehaviour
{
    public int Score;
    public int totalKeys = 0;
    public int Key1 = 0;
    public int Key2 = 0;
    public int Key3 = 0;
    public int Key4 = 0;
    public int Key5 = 0;
    public int Item1 = 0;
    public int Item1sent = 0;
    public int Item2 = 0;
    public int Item2sent = 0;
    public int Item3 = 0;
    public int Item3sent = 0;
    public int Item4 = 0;
    public int Item4sent = 0;
    public int totalItems = 0;

    public LayerMask mask; //LayerMask(s) that the Raycast will interact with (Select from Inspector)

    private void Update()
    {
        if(Input.GetMouseButton(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 50.0f, mask))
            {
                if (hit.transform != null)
                {
                    //KEY OBJECTS
                    if (hit.collider.tag == "Key1")
                    {
                        print("You touched the Key,  nice!");
                        Destroy(hit.collider.gameObject);
                        Key1 = 1;
                        totalKeys++;

                        FindObjectOfType<AudioManager>().Play("Get Key"); //Plays the 'Get Key' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Adds +1 to Key Count
                    }

                    if(hit.collider.tag == "Key2")
                    {
                        print("You touched the Key2,  nice!");
                        Destroy(hit.collider.gameObject);
                        Key2 = 1;
                        totalKeys++;

                        FindObjectOfType<AudioManager>().Play("Get Key"); //Plays the 'Get Key' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Adds +1 to Key Count
                    }

                    if(hit.collider.tag == "Key3")
                    {
                        print("You touched the Key3,  nice!");
                        Destroy (GameObject.FindWithTag("Key3"));
                        Key3 = 1;
                        totalKeys++;

                        FindObjectOfType<AudioManager>().Play("Get Key"); //Plays the 'Get Key' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Adds +1 to Key Count
                    }

                    if(hit.collider.tag == "Key4")
                    {
                        print("You touched the Key4,  nice!");
                        Destroy (GameObject.FindWithTag("Key4"));
                        Key4 = 1;
                        totalKeys++;

                        FindObjectOfType<AudioManager>().Play("Get Key"); //Plays the 'Get Key' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Adds +1 to Key Count
                    }

                    if(hit.collider.tag == "Key5")
                    {
                        print("You touched the Key5,  nice!");
                        Destroy (GameObject.FindWithTag("Key5"));
                        Key5 = 1;
                        totalKeys++;

                        FindObjectOfType<AudioManager>().Play("Get Key"); //Plays the 'Get Key' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Adds +1 to Key Count
                    }

                    if (hit.collider.tag == "Key6")
                    {
                        print("You touched the Key5,  nice!");
                        Destroy(GameObject.FindWithTag("Key6"));
                        Key5 = 1;
                        totalKeys++;

                        FindObjectOfType<AudioManager>().Play("Get Key"); //Plays the 'Get Key' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Adds +1 to Key Count
                    }

                    if (hit.collider.tag == "Key7")
                    {
                        print("You touched the Key5,  nice!");
                        Destroy(GameObject.FindWithTag("Key7"));
                        Key5 = 1;
                        totalKeys++;

                        FindObjectOfType<AudioManager>().Play("Get Key"); //Plays the 'Get Key' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Adds +1 to Key Count
                    }

                    if (hit.collider.tag == "Key8")
                    {
                        print("You touched the Key5,  nice!");
                        Destroy(GameObject.FindWithTag("Key8"));
                        Key5 = 1;
                        totalKeys++;

                        FindObjectOfType<AudioManager>().Play("Get Key"); //Plays the 'Get Key' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Adds +1 to Key Count
                    }

                    //DOOR OBJECTS
                    if (hit.collider.tag == "Door1" && Key1 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door1"));

                        totalKeys--;

                        FindObjectOfType<AudioManager>().Play("Destroy Door"); //Plays the 'Destroy Door' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Takes -1 to Key Count
                    }

                    if(hit.collider.tag == "Door2" && Key2 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door2"));

                        totalKeys--;

                        FindObjectOfType<AudioManager>().Play("Destroy Door"); //Plays the 'Destroy Door' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Takes -1 to Key Count
                    }

                    if(hit.collider.tag == "Door3" && Key3 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door3"));

                        totalKeys--;

                        FindObjectOfType<AudioManager>().Play("Destroy Door"); //Plays the 'Destroy Door' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Takes -1 to Key Count
                    }

                    if(hit.collider.tag == "Door4" && Key4 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door4"));

                        totalKeys--;

                        FindObjectOfType<AudioManager>().Play("Destroy Door"); //Plays the 'Destroy Door' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Takes -1 to Key Count
                    }

                    if(hit.collider.tag == "Door5" && Key5 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door5"));

                        totalKeys--;

                        FindObjectOfType<AudioManager>().Play("Destroy Door"); //Plays the 'Destroy Door' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Takes -1 to Key Count
                    }

                    if (hit.collider.tag == "Door6" && Key5 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door6"));

                        totalKeys--;

                        FindObjectOfType<AudioManager>().Play("Destroy Door"); //Plays the 'Destroy Door' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Takes -1 to Key Count
                    }

                    if (hit.collider.tag == "Door7" && Key5 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door7"));

                        totalKeys--;

                        FindObjectOfType<AudioManager>().Play("Destroy Door"); //Plays the 'Destroy Door' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Takes -1 to Key Count
                    }

                    if (hit.collider.tag == "Door8" && Key5 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door8"));

                        totalKeys--;

                        FindObjectOfType<AudioManager>().Play("Destroy Door"); //Plays the 'Destroy Door' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateKeys(totalKeys); //Takes -1 to Key Count
                    }

                    //COLLECTABLE OBJECTS
                    if (hit.collider.tag == "Item1")
                    {
                        print("You found an item,  nice!");
                        Destroy(GameObject.FindWithTag("Item1"));
                        Item1 = 1;

                        totalItems++;

                        FindObjectOfType<AudioManager>().Play("Get Collectable"); //Plays the 'Get Collectable' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateCollectables(totalItems); //Adds +1 to Collectable Count
                    }

                    if(hit.collider.tag == "Item2")
                    {
                        print("You found an item,  nice!");
                        Destroy(GameObject.FindWithTag("Item2"));
                        Item2 = 1;

                        totalItems++;

                        FindObjectOfType<AudioManager>().Play("Get Collectable"); //Plays the 'Get Collectable' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateCollectables(totalItems); //Adds +1 to Collectable Count
                    }

                    if(hit.collider.tag == "Item3")
                    {
                        print("You found an item,  nice!");
                        Destroy(GameObject.FindWithTag("Item3"));
                        Item3 = 1;

                        totalItems++;

                        FindObjectOfType<AudioManager>().Play("Get Collectable"); //Plays the 'Get Collectable' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateCollectables(totalItems); //Adds +1 to Collectable Count
                    }

                    if(hit.collider.tag == "Item4")
                    {
                        print("You found an item,  nice!");
                        Destroy(GameObject.FindWithTag("Item4"));
                        Item4 = 1;

                        totalItems++;

                        FindObjectOfType<AudioManager>().Play("Get Collectable"); //Plays the 'Get Collectable' SFX

                        FindObjectOfType<PlayerUIScript>().UpdateCollectables(totalItems); //Adds +1 to Collectable Count
                    }

                    //PLAYER SCORE
                    if(hit.collider.tag == "Scorepoint" )
                    {
                        if(Item1 == 1)
                        {
                            print("You turned in an item,  nice!");

                            Score = Score + 500;
                            Item1sent = 1;
                            Item1 = 0;

                            totalItems--;

                            FindObjectOfType<AudioManager>().Play("Deposit"); //Plays the 'Deposit' SFX

                            FindObjectOfType<PlayerUIScript>().UpdateCollectables(totalItems); //Takes -1 to Collectable Count
                        }

                        if(Item2 == 1)
                        {
                            Score = Score + 500;
                            Item2sent = 1;
                            Item2 = 0;

                            totalItems--;

                            FindObjectOfType<AudioManager>().Play("Deposit"); //Plays the 'Deposit' SFX

                            FindObjectOfType<PlayerUIScript>().UpdateCollectables(totalItems); //Takes -1 to Collectable Count
                        }

                        if(Item3 == 1)
                        {
                            Score = Score + 500;
                            Item3sent = 1;
                            Item3 = 0;

                            totalItems--;

                            FindObjectOfType<AudioManager>().Play("Deposit"); //Plays the 'Deposit' SFX

                            FindObjectOfType<PlayerUIScript>().UpdateCollectables(totalItems); //Takes -1 to Collectable Count
                        }

                        if(Item4 == 1)
                        {
                            Score = Score + 500;
                            Item4sent = 1;
                            Item4 = 0;

                            totalItems--;

                            FindObjectOfType<AudioManager>().Play("Deposit"); //Plays the 'Deposit' SFX

                            FindObjectOfType<PlayerUIScript>().UpdateCollectables(totalItems); //Takes -1 to Collectable Count
                        }
                    }
                }
            }
        }
    }

    private void PrintName(GameObject go)
    {
        print(go.name);
    }
}