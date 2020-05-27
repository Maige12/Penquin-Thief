using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PLEASE COMMENT THIS CODE

//CLEAN UP CODE LATER

public class ObjectClicker : MonoBehaviour
{
    public int Score;
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

    private void Update()
    {
        if(Input.GetMouseButton(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 10.0f))
            {
                if(hit.transform != null)
                {
                    if(hit.collider.tag == "Key1")
                    {
                        print("You touched the Key,  nice!");
                        Destroy(hit.collider.gameObject);
                        Key1 = 1;
                    }

                    if(hit.collider.tag == "Key2")
                    {
                        print("You touched the Key2,  nice!");
                        Destroy(hit.collider.gameObject);
                        Key2 = 1;
                    }

                    if(hit.collider.tag == "Key3")
                    {
                        print("You touched the Key3,  nice!");
                        Destroy (GameObject.FindWithTag("Key3"));
                        Key3 = 1;
                    }

                    if(hit.collider.tag == "Key4")
                    {
                        print("You touched the Key4,  nice!");
                        Destroy (GameObject.FindWithTag("Key4"));
                        Key4 = 1;
                    }

                    if(hit.collider.tag == "Key5")
                    {
                        print("You touched the Key5,  nice!");
                        Destroy (GameObject.FindWithTag("Key5"));
                        Key5 = 1;
                    }

                    if (hit.collider.tag == "Key6")
                    {
                        print("You touched the Key5,  nice!");
                        Destroy(GameObject.FindWithTag("Key6"));
                        Key5 = 1;
                    }

                    if (hit.collider.tag == "Key7")
                    {
                        print("You touched the Key5,  nice!");
                        Destroy(GameObject.FindWithTag("Key7"));
                        Key5 = 1;
                    }

                    if (hit.collider.tag == "Key8")
                    {
                        print("You touched the Key5,  nice!");
                        Destroy(GameObject.FindWithTag("Key8"));
                        Key5 = 1;
                    }


                    if (hit.collider.tag == "Door1" && Key1 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door1"));
                    }

                    if(hit.collider.tag == "Door2" && Key2 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door2"));

                    }

                    if(hit.collider.tag == "Door3" && Key3 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door3"));
                    }

                    if(hit.collider.tag == "Door4" && Key4 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door4"));
                    }

                    if(hit.collider.tag == "Door5" && Key5 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door5"));
                    }

                    if (hit.collider.tag == "Door6" && Key5 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door6"));
                    }

                    if (hit.collider.tag == "Door7" && Key5 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door7"));
                    }

                    if (hit.collider.tag == "Door8" && Key5 == 1)
                    {
                        print("You touched the door,  nice!");
                        Destroy(GameObject.FindWithTag("Door8"));
                    }

                    if (hit.collider.tag == "Item1")
                    {
                        print("You found an item,  nice!");
                        Destroy(GameObject.FindWithTag("Item1"));
                        Item1 = 1;
                    }

                    if(hit.collider.tag == "Item2")
                    {
                        print("You found an item,  nice!");
                        Destroy(GameObject.FindWithTag("Item2"));
                        Item2 = 1;
                    }

                    if(hit.collider.tag == "Item3")
                    {
                        print("You found an item,  nice!");
                        Destroy(GameObject.FindWithTag("Item3"));
                        Item3 = 1;
                    }

                    if(hit.collider.tag == "Item4")
                    {
                        print("You found an item,  nice!");
                        Destroy(GameObject.FindWithTag("Item4"));
                        Item4 = 1;
                    }

                    if(hit.collider.tag == "Scorepoint" )
                    {
                        if(Item1 == 1)
                        {
                            print("You turned in an item,  nice!");

                            Score = Score + 500;
                            Item1sent = 1;
                            Item1 = 0;
                        }

                        if(Item2 == 1)
                        {
                            Score = Score + 500;
                            Item2sent = 1;
                            Item2 = 0;
                        }

                        if(Item3 == 1)
                        {
                            Score = Score + 500;
                            Item3sent = 1;
                            Item3 = 0;
                        }

                        if(Item4 == 1)
                        {
                            Score = Score + 500;
                            Item4sent = 1;
                            Item4 = 0;
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
