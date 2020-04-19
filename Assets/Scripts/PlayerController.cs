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
    // Start is called before the first frame update
    void Start()
    {
        controller =  this.GetComponent<CharacterController>();
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
        if (!controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jump = true;
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
            friction = 0f;
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

        controller.Move(velocity * Time.fixedDeltaTime);
        
        //Add in velocity


        
        jump = false;
    }
}
