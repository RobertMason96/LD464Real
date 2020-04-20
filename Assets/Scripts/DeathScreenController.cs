using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreenController : MonoBehaviour
{
    public GameObject mainmenu;
    public GameObject playagain;
    public GameObject quit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void openmainmenu()
    {
        Debug.Log("Go to Mainmenu");
    }
    public void openplayagain()
    {
        Debug.Log("Go to Playagain");
    }
    public void openquit()
    {
        Debug.Log("Go to Quit");
    }
}
