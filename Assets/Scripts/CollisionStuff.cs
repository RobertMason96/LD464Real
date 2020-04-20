using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionStuff : MonoBehaviour
{
    public GameObject blood;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "SnakeHead")
        {
            GameObject temp = Instantiate(blood, this.transform.position, Quaternion.identity);
            SnakeMove sm = collision.gameObject.transform.parent.gameObject.GetComponent<SnakeMove>();
            if(sm != null)
            {
                sm.PlayerEatten();

            }
            else
            {
                Debug.Log("0");
            }
            Destroy(temp, 2.0f);
            Debug.Log("Change to Death Sceen");
            Destroy(this.gameObject.gameObject);
        }
    }

}
