using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UITutorial : MonoBehaviour
{
    private bool WASD;
    private bool E;
    // Start is called before the first frame update
    void Start()
    {
        WASD = false;
        E = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!WASD)
        {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                WASD = true;
                this.gameObject.GetComponent<Text>().text = "E to pick up food";
            }
        }
        else
        {
            if (!E)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    E = true;
                    Destroy(this.gameObject);
                }
            }

        }
    }
}
