using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobertSnakeCollider : MonoBehaviour
{


    RobertSnakeMove RSM=null;

    public void Start()
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
        
        if (currentObject.GetComponent<RobertSnakeMove>() != null)
        {
            RSM = currentObject.GetComponent<RobertSnakeMove>();
            
        }


    }
    /*
    void OnCollisionEnter(Collider collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.tag == "Wall")
        {
            if(RSM!=null)
            {
                RSM.Death();
            }
            else
            {
                Debug.Log("ERROR NO RSM");
            }
            

        }
    }*/
}
