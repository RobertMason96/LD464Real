using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
    public void openplayagain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
    public void openquit()
    {
        Application.Quit();
    }
}
