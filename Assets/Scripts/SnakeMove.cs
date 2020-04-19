using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMove : MonoBehaviour
{
    private List<GameObject> DeBody;
    public GameObject BodyPart;
    public float moveSpeed;
    private float timer;

    //Waypoint information
    public GameObject Waypoint;
    private bool IsFood; //is the next waypoint food?

    //Spacing of the grid in the world
    public float gridSize;

    //Food
    private GameObject food;

    // Start is called before the first frame update
    void Start()
    {
        IsFood = true;
        DeBody = new List<GameObject>();
        DeBody.Add(GameObject.FindGameObjectWithTag("SnakeHead"));
        DeBody.Add(GameObject.FindGameObjectWithTag("SnakePart"));
        DeBody.Add(GameObject.FindGameObjectWithTag("SnakeTail"));
        timer = 0;
        food = findFood();
        FindNextWayPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= (2 / moveSpeed))
        {
            findFood();
            FindNextWayPoint();
            if (findDirection(DeBody[0]).magnitude <= gridSize)
            {
                FindNextWayPoint();
                if (IsFood)
                {
                    foodEatten();
                }
                else
                {
                    moveSnake();
                }
            }
            else
            {
                moveSnake();
            }
            timer = 0;
        }
        timer += Time.deltaTime;
    }
    private Vector3 findDirection(GameObject gObj)
    {
        Vector3 dir = new Vector3(Waypoint.transform.position.x - gObj.transform.position.x, 0 , Waypoint.transform.position.z- gObj.transform.position.z);
        return dir;
        
    }
    private void FindNextWayPoint()
    {
        if((Mathf.Abs(food.transform.position.x - DeBody[0].transform.position.x) >= gridSize) && (Mathf.Abs(food.transform.position.z - DeBody[0].transform.position.z) >= gridSize))
        {
            Debug.Log("poop here");
            IsFood = false;
            Waypoint.transform.position = new Vector3(DeBody[0].transform.position.x, DeBody[0].transform.position.y, Mathf.Floor(food.transform.position.z / gridSize) * gridSize);
        }
        else
        {
            IsFood = true;
            Waypoint.transform.position = new Vector3(Mathf.Floor(food.transform.position.x / gridSize), DeBody[0].transform.position.y, Mathf.Floor(food.transform.position.z / gridSize)) * gridSize;
        }
    }
    private void foodEatten()
    {
        DeBody.Add(DeBody[DeBody.Count - 1]);
        DeBody[DeBody.Count - 2] = Instantiate(BodyPart, DeBody[DeBody.Count - 1].transform.position, DeBody[DeBody.Count - 1].transform.rotation);
        DeBody[DeBody.Count - 2].transform.parent = this.transform;

        Vector3 newPos = DeBody[0].transform.position;
        Vector3 oldPos = DeBody[0].transform.position;

        Vector3 moveDir = new Vector3(0, 0, 0);
        moveDir = findDirection(DeBody[0]);
        DeBody[0].transform.position += moveDir.normalized * gridSize;
        DeBody[0].transform.rotation = Quaternion.Euler(0, Mathf.Sign(moveDir.x) * Mathf.Sign(moveDir.z) * (Vector3.Angle(moveDir, transform.forward)) + 90, 0);
        for (int i = 1; i < DeBody.Count - 1; i++)
        {
            newPos = DeBody[i].transform.position;
            moveDir = newPos - oldPos;
            DeBody[i].transform.position = oldPos;
            DeBody[i].transform.rotation = Quaternion.Euler(0, Mathf.Sign(moveDir.x) * Mathf.Sign(moveDir.z) * (Vector3.Angle(moveDir, transform.forward)) - 90, 0);
            oldPos = newPos;
        }

    }
    private void moveSnake()
    {
        Vector3 newPos = DeBody[0].transform.position;
        Vector3 oldPos = DeBody[0].transform.position;

        Vector3 moveDir = new Vector3(0, 0, 0);
        moveDir = findDirection(DeBody[0]);
        DeBody[0].transform.position += moveDir.normalized * gridSize;
        DeBody[0].transform.rotation = Quaternion.Euler(0, Mathf.Sign(moveDir.x) * Mathf.Sign(moveDir.z) * (Vector3.Angle(moveDir, transform.forward)) + 90, 0);
        for (int i = 1; i < DeBody.Count; i++)
        {
            newPos = DeBody[i].transform.position;
            moveDir = newPos - oldPos;
            DeBody[i].transform.position = oldPos;
            DeBody[i].transform.rotation = Quaternion.Euler(0, Mathf.Sign(moveDir.x) * Mathf.Sign(moveDir.z) * (Vector3.Angle(moveDir, transform.forward)) - 90, 0);
            oldPos = newPos;
        }
    }
    private GameObject findFood()
    {
        float shortestDistance = 100.0f;
        int i = 0;
        int x = 0;
        GameObject[] F = GameObject.FindGameObjectsWithTag("Food");
        if (F != null)
        {
            foreach (GameObject gObj in F) 
            {
                if (shortestDistance > (DeBody[0].transform.position - gObj.transform.position).magnitude)
                {
                    i = x;
                    shortestDistance = (DeBody[0].transform.position - gObj.transform.position).magnitude;
                }
                x++;
            }
        }
        return F[i];
    }
}
