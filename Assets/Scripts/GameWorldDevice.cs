using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorldDevice : MonoBehaviour
{
    //public GameObject InitailPositionTransform;

    public Vector3 InitialPosition;
    public Quaternion InitialRotation;
    // Start is called before the first frame update
    void Start()
    {
        InitialPosition = this.transform.position;
        InitialRotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
