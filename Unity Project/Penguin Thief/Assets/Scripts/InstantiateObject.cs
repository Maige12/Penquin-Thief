using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObject : MonoBehaviour
{
    public Transform ride;
    public int health = 20; // 0x243456475 is the memory address but it's too hard to read so we use "health" as an alias to it
                            // to make it easier for use to program with
                            // int is 32bits in size and signed means it can -tve or +tve i.e -10, 50 etc
    
    // Start is called before the first frame update
    void Start()
    {
        //GameObject objectReferance = Instantiate(gameObject);
        //int health = 10;
        //Destroy(objectReferance, 1);;
        //GameObject ball
        //Instantiate - Clones the object original and returns the clone.
        Transform objectReference = Instantiate(ride);
        objectReference.gameObject.name = "chair";
        //objectReference.transform.Vector.Position (6,6,6); // wrong way
        objectReference.transform.position = new Vector3(6, 6, 6);
    }
}
