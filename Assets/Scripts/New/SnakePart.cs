using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    private List<GameObject> SnakeBody;
    private List<Vector3> WayPoints;
    public float speed;
    public GameObject newSnakePart;

    public int maxSnakeSize;

    // Start is called before the first frame update
    void Awake()
    {
        SnakeBody = new List<GameObject>();
        WayPoints = new List<Vector3>();

        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "SnakeHead")
            {
                SnakeBody.Add(child.gameObject);
            }
        }
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "SnakePart")
            {
                SnakeBody.Add(child.gameObject);
            }
        }
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "SnakeTail")
            {
                SnakeBody.Add(child.gameObject);
            }
        }
        for (int i = 1; i < SnakeBody.Count; i++)
        {
            WayPoints.Add(SnakeBody[i-1].transform.position);
        }
        WayPoints.Add(SnakeBody[SnakeBody.Count - 2].transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveTheSnake();
    }
    private void moveTheSnake()
    {
        //Move and rotate all snake parts (except the head thats handled in the path finding)
        //SnakeHead moved in snakeAI script
        for (int i = 1; i < SnakeBody.Count; i++)
        {
            SnakeBody[i].transform.rotation = rotatePart(i, SnakeBody[i - 1].transform.position);
            SnakeBody[i].transform.position = movePart(i);
        }
    }
    private Quaternion rotatePart(int part, Vector3 dest)
    {
        //Rotate the piece to face the piece in front
        return Quaternion.LookRotation(-SnakeBody[part].transform.position + dest + new Vector3(0.0001f,0,0.0001f));
    }
    private Vector3 movePart(int part)
    {
        //Move the piece towards the piece in front 
        //Maybe (SnakeBody.transform.position + ((SnakeBody.transform.forward * moveSpeed * timeDeltaTime) * Mathf.Sign((SnakeBody[part].transform.position - SnakeBody[part - 1].transform.position).magnitude - PartLength)));
        return (SnakeBody[part].transform.position += ((WayPoints[part] - SnakeBody[part].transform.position).normalized) * speed);
    }

    public void UpdateWayPoints(Vector3 newPoint)
    {
        WayPoints.Insert(0, newPoint);
        WayPoints.RemoveRange(SnakeBody.Count + 1, WayPoints.Count - (SnakeBody.Count + 1));
    }
    public void createNewPart(Material mat)
    {
        if((SnakeBody.Count) < maxSnakeSize)
        {
            GameObject snek = Instantiate(newSnakePart, SnakeBody[SnakeBody.Count - 1].transform.position, SnakeBody[SnakeBody.Count - 1].transform.rotation);
            if (GameObject.FindWithTag("World").GetComponent<WorldController>() != null)
            {
                if (GameObject.FindWithTag("World").GetComponent<WorldController>().multiplayerTeam)
                {
                    snek.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = mat.color;
                }
            }
            snek.transform.parent = this.transform;
            SnakeBody.Insert(SnakeBody.Count - 1, snek);
            WayPoints.Insert(SnakeBody.Count - 1, WayPoints[SnakeBody.Count - 1]);
            WayPoints[SnakeBody.Count] = WayPoints[WayPoints.Count - 1];
            WayPoints.RemoveRange(SnakeBody.Count + 1, WayPoints.Count - (SnakeBody.Count + 1));
        }
    
    }
}
