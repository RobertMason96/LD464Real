using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float maxSpeed = 5f;
    public float minSpeed = 0.01f;
    public float Gravity = 1f;
    public Vector3 velocity = new Vector3(0, 0, 0);
    public float floorfriction = 5f;
    public float force = 15f;
    public Vector2 inplaneInput=new Vector2(0,0);
    bool jump = false;
    float jumpforce = 5f;
    public float minv = 1f;
    public float friction = 0f;
    public float airFriction = 5f;

    public GameObject holderObject = null;
    public float jumpHeight =1f;
    public Transform holderempty;
    // Start is called before the first frame update
    void Start()
    {
        controller =  this.GetComponent<CharacterController>();
        foreach(Transform t in this.GetComponentsInChildren<Transform>())
        {
            if(t.transform.gameObject.name=="HolderEmpty")
            {
                holderempty = t;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        inplaneInput = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.A))
        {
            inplaneInput.x--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inplaneInput.x++;
        }
        if (Input.GetKey(KeyCode.W))
        {
            inplaneInput.y++;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inplaneInput.y--;
        }
        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 100f, Color.yellow);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("DOWN");
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * jumpHeight, Color.yellow);

            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit,jumpHeight, layerMask))
            {
                jump = true;                
                Debug.Log("Did Hit");
            }
            
            
        }            
        
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(holderObject==null)
            {
                // Bit shift the index of the layer (8) to get a bit mask
                int layerMask = 1 << 8;

                // This would cast rays only against colliders in layer 8.
                // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
                layerMask = ~layerMask;


                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    if (hit.transform.tag == "Pickup")
                    {
                        holderObject = hit.transform.gameObject;
                        Debug.Log("Did Pickup Hit");
                        if(holderObject.GetComponent<Rigidbody>()!=null)
                        {
                            holderObject.GetComponent<Rigidbody>().isKinematic = true;
                        }
                        if (holderObject.GetComponent<Collider>() != null)
                        {
                            holderObject.GetComponent<Collider>().enabled = false;
                        }
                    }
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                    Debug.Log("Did Hit");
                }
                else
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                    Debug.Log("Did not Hit");
                }

            }
            else
            {
                if (holderObject.GetComponent<Rigidbody>() != null)
                {
                    holderObject.GetComponent<Rigidbody>().isKinematic = false;
                }
                if (holderObject.GetComponent<Collider>() != null)
                {
                    holderObject.GetComponent<Collider>().enabled = true;
                }
                holderObject = null;
            }
        }

        if(holderObject!=null)
        {
            holderObject.transform.position = holderempty.position;
            holderObject.transform.rotation = holderempty.rotation;
        }
        
    }
    public void FixedUpdate()
    {

        if(controller.isGrounded)
        {
            friction = floorfriction;
        }
        else
        {
            friction = airFriction;
        }

        float movey=velocity.y;
        if ((controller.isGrounded)&&(!jump))
        {
            movey= 0;
        }
        else
        {
            if(jump)
            {
                movey += jumpforce;
                jump = false;
            }
            else
            {
                movey -= Gravity * Time.fixedDeltaTime;
            }
        }

        
        Vector2 inplaneVelocity = new Vector2(velocity.x, velocity.z);
        inplaneVelocity = minv * inplaneInput * force * Time.fixedDeltaTime + inplaneVelocity * (1 - friction * Time.fixedDeltaTime * minv);        
        if(inplaneVelocity.magnitude>maxSpeed)
        {
            inplaneVelocity = inplaneVelocity.normalized * maxSpeed;
        }
        if (inplaneVelocity.magnitude < minSpeed)
        {
            inplaneVelocity = new Vector2(0,0);
        }

        velocity = new Vector3(inplaneVelocity.x, movey, inplaneVelocity.y);

        if(inplaneVelocity.magnitude!=0)
        {
            transform.forward = new Vector3(inplaneVelocity.x,0,inplaneVelocity.y);
        }
        



        controller.Move(velocity * Time.fixedDeltaTime);
        
        //Add in velocity


        
        
    }
}
