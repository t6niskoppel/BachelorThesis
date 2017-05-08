using UnityEngine;

/**
 * 
 * Activates displays connected to the computer
 * 
 */ 

public class DisplayScript : MonoBehaviour
{

    void Start()
    {
        Debug.Log("displays connected: " + Display.displays.Length);

        foreach (Display display in Display.displays)
        {
            display.Activate();
        }

    }
}