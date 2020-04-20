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

    public float pickupLength = 2f;

    public float collisionLength = 2f;
    public LayerMask phignorelayer;

    public GameObject holderObject = null;
    public float jumpHeight =1.5f;
    public Transform holderempty;
    float deltaRay = 0.1f;

    public float crashTime = 0.2f;

    public Animator animator;

    


    bool canMove = true;

    bool running = false;

    public AudioClip pain;

    public float pushBackForce = 15f;

    public AudioSource audioSource;
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
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
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
            if(inplaneInput != new Vector2(0, 0))
            {
                if(!running)
                {
                    animator.SetBool("Start", true);
                }
                running = true;
            }            
            else
            {
                if(running)
                {
                    animator.SetBool("Start", false);
                }
                running = false;
            }
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * jumpHeight, Color.yellow);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * pickupLength, Color.red);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * collisionLength, Color.blue);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("DOWN");
                // Bit shift the index of the layer (8) to get a bit mask
                int layerMask = 1 << 8;

                // This would cast rays only against colliders in layer 8.
                // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
                layerMask = ~layerMask;


                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hit, jumpHeight, layerMask))
                {
                    jump = true;

                }


            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (holderObject == null)
                {
                    // Bit shift the index of the layer (8) to get a bit mask
                    int layerMask = 1 << 8;

                    // This would cast rays only against colliders in layer 8.
                    // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
                    layerMask = ~layerMask;


                    // Does the ray intersect any objects excluding the player layer
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, pickupLength, layerMask))
                    {
                        if (hit.transform.tag == "Pickup")
                        {
                            holderObject = hit.transform.gameObject;
                            holderObject.layer = LayerMask.NameToLayer("Player");
                            controller.center = new Vector3(controller.center.x, controller.center.y, 0.52f);
                            controller.radius = 0.96f;
                            
                        }
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                    }
                    else
                    {
                        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);

                    }

                }
                else
                {

                    controller.radius = 0.5f;
                    controller.center = new Vector3(controller.center.x, controller.center.y, 0f);
                    holderObject.layer = LayerMask.NameToLayer("Default");

                    holderObject = null;
                    
                }
            }
            if (holderObject != null)
            {
                holderObject.transform.position = holderempty.position;
                holderObject.transform.rotation = holderempty.rotation;

                

               

                


            }
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
        float blend = inplaneVelocity.magnitude / maxSpeed;
        animator.SetFloat("Blend",blend);
                
    }
}
