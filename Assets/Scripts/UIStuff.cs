using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIStuff : MonoBehaviour
{
    public GameObject mainmenu;
    public GameObject information;
    public GameObject credits;

    public Image tutorial;
    public Sprite[] tutorials;
    int currentScreen = 0;
    public GameObject NextBtn;
    public GameObject PreviousBtn;
    // Start is called before the first frame update
    void Start()
    {
        currentScreen = 0;
        if(tutorials.Length>0)
        {
            tutorial.sprite = tutorials[currentScreen];
        }
        else
        {
            Debug.Log("Help, No Tutorials!");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openmainmenu()
    {
        mainmenu.SetActive(true);
        information.SetActive(false);
        credits.SetActive(false);
    }

    public void openinformation()
    {
        information.SetActive(true);
        mainmenu.SetActive(false);        
        credits.SetActive(false);
    }

    public void opencredits()
    {
        credits.SetActive(true);
        mainmenu.SetActive(false);
        information.SetActive(false);
        

    }


    public void nextbutton()
    {
        if (currentScreen < tutorials.Length-1)
        {
            currentScreen++;
            if(currentScreen==tutorials.Length-1)
            {
                NextBtn.SetActive(false);
            }
            if(currentScreen==1)
            {
                PreviousBtn.SetActive(true);
            }
        }
        tutorial.sprite = tutorials[currentScreen];
    }

    public void previousbutton()
    {
        if (currentScreen >0)
        {
            currentScreen--;
            if (currentScreen == 0)
            {
                PreviousBtn.SetActive(false);
            }
            if (currentScreen == tutorials.Length - 2)
            {
                NextBtn.SetActive(true);
            }
        }
        tutorial.sprite = tutorials[currentScreen];
    }

    public void playButton()
    {
        SceneManager.LoadScene("DavidsScene2", LoadSceneMode.Single);
    }

    public void quitButton()
    {
        Application.Quit();
    }
}
