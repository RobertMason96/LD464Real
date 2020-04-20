using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobertSnakeCollider : MonoBehaviour
{



    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            bool found = false;
            GameObject currentObject = this.gameObject;
            while (!found)
            {
                if (currentObject.transform.parent != null)
                {
                    currentObject = currentObject.transform.parent.gameObject;
                }
                else
                {
                    found = true;
                }
            }
            if(currentObject.GetComponent<RobertSnakeMove>()!=null)
            {
                currentObject.GetComponent<RobertSnakeMove>().Death();
            }
        }
    }
}
