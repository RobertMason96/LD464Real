using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform holderposition;
    GameObject currentObject = null;
    public float rotationSpeed = 2f;
    bool holding = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //When LeftMouse Click
        if (Input.GetMouseButtonDown(0))
        {
            //if we're not holding an object and we click on a pickup, put it in front of us
            if(currentObject==null)
            {                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Bit shift the index of the layer (8) to get a bit mask
                int layerMask = 1 << 8;
                layerMask = ~layerMask;
                //RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    if (hit.transform.tag == "Pickup")
                    {
                        
                        
                        bool found = false;
                        currentObject = hit.transform.gameObject;  
                        while(!found)
                        {
                            if(currentObject.transform.parent!=null)
                            {
                                currentObject = currentObject.transform.parent.gameObject;
                            }
                            else
                            {
                                found = true;
                            }
                        }
                        currentObject.transform.position = holderposition.position;
                        currentObject.transform.rotation = holderposition.rotation;
                    }
                    
                }
                else
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                    
                }
            } 
            //If we are holding something and we click away from the object we put it down
            else
            {
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // Bit shift the index of the layer (8) to get a bit mask
                int layerMask = 1 << 8;
                layerMask = ~layerMask;
                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    if (hit.transform.tag != "Pickup")
                    {                        
                        if(currentObject.GetComponent<GameWorldDevice>()!=null)
                        {
                            currentObject.transform.position = currentObject.GetComponent<GameWorldDevice>().InitialPosition;
                            currentObject.transform.rotation = currentObject.GetComponent<GameWorldDevice>().InitialRotation;
                            currentObject = null;
                            
                        }
                        else
                        {
                            Debug.Log("ERROR NO GAMEWORLDDEVICE");
                        }
                    }
                    else
                    {
                        if((hit.transform.gameObject.GetComponent<ItemType>()!=null)&&(currentObject.GetComponent<DeviceController>()!=null))
                        {
                            switch (hit.transform.gameObject.GetComponent<ItemType>().itype)
                            {
                                case ItemType.itemType.Device:
                                    break;
                                case ItemType.itemType.DownBtn:
                                    currentObject.GetComponent<DeviceController>().DownButtonPressed();
                                    break;
                                case ItemType.itemType.LeftBtn:
                                    currentObject.GetComponent<DeviceController>().LeftButtonPressed();
                                    break;
                                case ItemType.itemType.RightBtn:
                                    currentObject.GetComponent<DeviceController>().RightButtonPressed();
                                    break;
                                case ItemType.itemType.UpBtn:
                                    currentObject.GetComponent<DeviceController>().UpButtonPressed();
                                    break;
                            }
                            
                        }
                    }
                    
                }
                else
                {
                    if (currentObject.GetComponent<GameWorldDevice>() != null)
                    {
                        currentObject.transform.position=currentObject.GetComponent<GameWorldDevice>().InitialPosition;
                        currentObject.transform.rotation=currentObject.GetComponent<GameWorldDevice>().InitialRotation;
                        currentObject = null;
                        
                    }
                    else
                    {
                        Debug.Log("ERROR NO GAMEWORLDDEVICE");
                    }
                }
            }
        }
        if(Input.GetMouseButton(1)&&currentObject!=null)
        {
            RotateObject(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"),  currentObject);
        }
            
    }



    //FROM LD43
    //Used to rotate the object
    private void RotateObject(float locx, float locy,GameObject go)
    {        
        //Rotation stuff
        float mH = rotationSpeed * locx;
        float mV = rotationSpeed * locy;
        go.transform.Rotate(mV, -mH,0 );
    }
}
