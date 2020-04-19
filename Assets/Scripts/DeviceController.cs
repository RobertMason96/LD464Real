using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceController : MonoBehaviour
{

    public float timeStep=0.5f;
    public float maxX = 10f;
    public float minX = -10f;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpButtonPressed()
    {
        player.transform.position += new Vector3(0f, 1f);
    }
    public void DownButtonPressed()
    {
        player.transform.position += new Vector3(0f, -1f);
    }
    public void LeftButtonPressed()
    {
        player.transform.position += new Vector3(-1f, 0f);
    }
    public void RightButtonPressed()
    {
        player.transform.position += new Vector3(1f, 0f);
    }

}
