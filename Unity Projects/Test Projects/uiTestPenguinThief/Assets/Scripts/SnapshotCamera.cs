using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //Includes commands for TextMesh Pro
using System.IO; //Allows Files/Folders to be Created
using UnityEditor; //Allows access to AssetDatabase.IsValidFolder

[RequireComponent(typeof(Camera))] //Makes sure that the Script can only be used with Camera Objects
public class SnapshotCamera : MonoBehaviour
{
    static Camera screenCam; //Object which stores 'Screenshot Camera' from hierarchy

    int resWidth = 256, resHeight = 256; //Default Height/Width if there is no Render Texture (Intentionally low to know theres Error)

    void Awake()
    {
        //UNCOMMENT THIS CODE WHEN TESTING IN EDITOR
        //COMMENT THIS CODE WHEN CREATING A BUILD (AssetDatabase CAUSES COMPILE ERRORS)
        //if(!AssetDatabase.IsValidFolder("Screenshots")) //Checks to see if a Screenshots Folder exists
        //{
            //var folder = Directory.CreateDirectory("Assets/Screenshots/"); //Creates a Screenshots folder in Assets
        //}

        //UNCOMMENT THIS CODE WHEN CREATING A BUILD
        //COMMENT THIS CODE WHEN TESTING IN THE EDITOR
        if (!System.IO.Directory.Exists(Application.dataPath + "/Screenshots")) //Checks to see if a Screenshots Folder exists
        {
            Directory.CreateDirectory(Application.dataPath + "/Screenshots"); //Creates a Screenshots folder in the _Data Folder
        }

        screenCam = GetComponent<Camera>(); //Gets Camera Component from Hierarchy
        if(screenCam.targetTexture == null) //If there is no render texture attached to Camera, this will create default one
        {
            screenCam.targetTexture = new RenderTexture(resWidth, resHeight, 24); //(Resolution Width, Resolution Height, Depth Buffer (Bits))
        }
        else
        {
            resWidth = screenCam.targetTexture.width; //Sets resWidth to the Width of the attached Render Texture Asset
            resHeight = screenCam.targetTexture.height; //Sets resHeight to the Height of the attached Render Texture Asset
        }

        screenCam.gameObject.SetActive(false); //Turns Screenshot Camera off at the start so it isn't continually used
    }

    public static void TakeScreenshot() //Handles turning on the camera
    {
        screenCam.gameObject.SetActive(true); //Turns on Camera
    }

    void LateUpdate() //LateUpdate is called after all Update functions have been called.
    {
        if(screenCam.gameObject.activeInHierarchy) //Checks if camera is turned on
        {
            Texture2D screenshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false); //Grid which holds the image information
            screenCam.Render(); //Tells the camera to start rendering so an image can be taken
            RenderTexture.active = screenCam.targetTexture; //Tells Unity to use the Screen Shot camera and not the Main Camera
            screenshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0); //Will look at the pixels from the image. Passed in from active texture (Rect), and then fills from bottom left to top right)
            byte[] bytes = screenshot.EncodeToPNG(); //Create PNG Data from Array of Bytes (EncodeToPNG takes texture information and converts into Bytes)
            string fileName = ScreenshotName(); //File Name for Images
            System.IO.File.WriteAllBytes(fileName, bytes); //Creates a file using the File Name and Bytes array
            Debug.Log("Screenshot taken!");
            screenCam.gameObject.SetActive(false); //Turns off Camera
        }
    }

    string ScreenshotName()
    {
        return string.Format("{0}/Screenshots/screen_{1}x{2}_{3}.png",
            Application.dataPath,
            resWidth,
            resHeight,
            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")); //{0} = Path to Location, {1} = Width, {2} = Height, {3} = Current Time
        /*
        return string.Format("{0}/Screenshots/screen_{1}x{2}_{3}.png",
            Application.persistentDataPath,
            resWidth,
            resHeight,
            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")); //{0} = Path to Location, {1} = Width, {2} = Height, {3} = Current Time
        */
    }
}

/*
 * References:
 *      - https://youtu.be/d-56p770t0U
 *      - https://docs.unity3d.com/ScriptReference/MonoBehaviour.LateUpdate.html
 *      - https://docs.unity3d.com/ScriptReference/GameObject-activeInHierarchy.html
 *      - https://docs.unity3d.com/ScriptReference/ImageConversion.EncodeToPNG.html
 *      - https://docs.unity3d.com/ScriptReference/String.html
 *      - https://docs.microsoft.com/en-us/dotnet/api/system.datetime.now?view=netcore-3.1
 *      - https://docs.microsoft.com/en-us/dotnet/api/system.io?view=dotnet-plat-ext-3.1
 *      - https://docs.unity3d.com/ScriptReference/AssetDatabase.IsValidFolder.html
*/
