using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMove : MonoBehaviour
{
    private List<GameObject> DeBody;
    public GameObject BodyPart;
    public float moveSpeed;
    private float timer;

    //Spacing of the grid in the world
    public float gridSize;

    private Vector3 movementDirection;

    //Food
    private GameObject food;

    public string foodTag;

    public GameObject blood;

    // Start is called before the first frame update
    void Start()
    {
        DeBody = new List<GameObject>();
        DeBody.Add(GameObject.FindGameObjectWithTag("SnakeHead"));
        DeBody.Add(GameObject.FindGameObjectWithTag("SnakePart"));
        DeBody.Add(GameObject.FindGameObjectWithTag("SnakeTail"));
        movementDirection = -DeBody[0].transform.right * gridSize;
        timer = 0;
        food = findFood();
        getDirections();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= (2 / moveSpeed))
        {
            food = findFood();
            getDirections();
            moveSnake();
            timer = 0;
        }
        timer += Time.deltaTime;
    }

    private void getDirections()
    {
        if (food != null)
        {
            Vector3 moveDir = new Vector3(0, 0, 0);
            moveDir = new Vector3(food.transform.position.x - DeBody[0].transform.position.x, DeBody[0].transform.position.y, food.transform.position.z - DeBody[0].transform.position.z);

            if (Mathf.Abs(moveDir.x) < gridSize && Mathf.Abs(moveDir.z) < gridSize)
            {
                foodEatten(food);
                
            }
            else
            {
                if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.z))
                {
                    if (Mathf.Abs(moveDir.x) <= gridSize)
                    {
                        movementDirection = Mathf.Sign(moveDir.z) * new Vector3(0, 0, 1) * gridSize;
                    }
                    else
                    {
                        if (Mathf.Sign(moveDir.x) != Mathf.Sign(movementDirection.x))
                        {
                            movementDirection = Mathf.Sign(moveDir.z) * new Vector3(0, 0, 1) * gridSize;
                        }
                    }
                }
                else
                {
                    if (Mathf.Abs(moveDir.z) <= gridSize)
                    {
                        movementDirection = Mathf.Sign(moveDir.x) * new Vector3(1, 0, 0) * gridSize;
                    }
                    else
                    {
                        if (Mathf.Sign(moveDir.z) != Mathf.Sign(movementDirection.z))
                        {
                            movementDirection = Mathf.Sign(moveDir.x) * new Vector3(1, 0, 0) * gridSize;
                        }
                    }
                }
            }


        }
    }

    private Vector3 getAngle(Vector3 direction)
    {
        Vector3 temp = new Vector3(0,0,0);
        if(direction.x == 0)
        {
            if (direction.z >0)
            {
                temp = new Vector3(0, 90, 0);
            }
            else
            {
                temp = new Vector3(0, -90, 0);
            }
        }
        else
        {
            if (direction.x > 0)
            {
                temp = new Vector3(0, 180, 0);
            }
            else
            {
                temp = new Vector3(0, 0, 0);
            }
        }
        return temp;
    }

    private void foodEatten(GameObject temp)
    {
        TileController tC = temp.transform.parent.gameObject.GetComponent<TileController>();
        if (tC != null)
        {
            tC.replaceFood(temp);
        }

        DeBody.Add(DeBody[DeBody.Count - 1]);
        DeBody[DeBody.Count - 2] = Instantiate(BodyPart, DeBody[DeBody.Count - 1].transform.position, DeBody[DeBody.Count - 1].transform.rotation);
        DeBody[DeBody.Count - 2].transform.parent = this.transform;      

    }

    private void moveSnake()
    {
        Vector3 newPos = DeBody[0].transform.position;
        Vector3 oldPos = DeBody[0].transform.position;

        Vector3 moveDir = movementDirection;


        DeBody[0].transform.position += moveDir;
        DeBody[0].transform.rotation = Quaternion.Euler(getAngle(moveDir));
             
        for (int i = 1; i < DeBody.Count; i++)
        {
            newPos = DeBody[i].transform.position;
            moveDir = newPos - oldPos;
            DeBody[i].transform.position = oldPos;
            DeBody[i].transform.rotation = Quaternion.Euler(getAngle(moveDir));
            oldPos = newPos;
            if(newPos == DeBody[0].transform.position)
            {
                Death();
            }

            GameObject[] F = GameObject.FindGameObjectsWithTag(foodTag);
            if (F.Length != 0)
            {
                foreach (GameObject gObj in F)
                {
                    moveDir = new Vector3(gObj.transform.position.x - newPos.x, gObj.transform.position.y - newPos.y, gObj.transform.position.z - newPos.z);

                    if (Mathf.Abs(moveDir.x) < gridSize && Mathf.Abs(moveDir.z) < gridSize && Mathf.Abs(moveDir.y) < gridSize)
                    {

                        foodEatten(gObj);

                    }
                }
            }

        }
    }

    private GameObject findFood()
    {
        
        int i = 0;
        int x = 0;
        GameObject temp = null;
        GameObject[] F = GameObject.FindGameObjectsWithTag(foodTag);
        if (F.Length != 0)
        {
            float shortestDistance = (DeBody[0].transform.position - F[0].transform.position).magnitude;
            foreach (GameObject gObj in F) 
            {
                if (shortestDistance > (DeBody[0].transform.position - gObj.transform.position).magnitude)
                {
                    if (Mathf.Abs((DeBody[0].transform.position - gObj.transform.position).y) < gridSize)
                    {
                        i = x;
                        shortestDistance = (DeBody[0].transform.position - gObj.transform.position).magnitude;
                    }
                }
                x++;
            }
            temp = F[i];
        }
        return temp;
    }

    private void Death()
    {
        Instantiate(blood, DeBody[0].transform.position, Quaternion.identity);
        DeBody.Remove(DeBody[0]);
        Debug.Log((DeBody.Count));
    }
}
