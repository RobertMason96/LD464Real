using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMove : MonoBehaviour
{
    private  List<GameObject> DeBody;
    public GameObject BodyPart;
    public float moveSpeed;
    private float timer;
    public GameObject Waypoint;
    public float gridSize;

    //Testing junk can get rid of
    private Vector3[] fCorners;
    private int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        fCorners = new Vector3[4];
        fCorners[0] = new Vector3(18, 0, 18);
        fCorners[1] = new Vector3(-18, 0, 18);
        fCorners[2] = new Vector3(-18, 0, -18);
        fCorners[3] = new Vector3(18, 0, -18);
        DeBody = new List<GameObject>();
        DeBody.Add(GameObject.FindGameObjectWithTag("SnakeHead"));
        DeBody.Add(GameObject.FindGameObjectWithTag("SnakePart"));
        DeBody.Add(GameObject.FindGameObjectWithTag("SnakeTail"));
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (findDirection(DeBody[0]).magnitude < gridSize)
        {
            FindNextWayPoint();
            
        }
        if(timer >= (2 / moveSpeed))
        {
            Vector3 newPos = DeBody[0].transform.position; 
            Vector3 oldPos = DeBody[0].transform.position;

            Vector3 moveDir = new Vector3(0, 0, 0);
            moveDir = findDirection(DeBody[0]);
            DeBody[0].transform.position += moveDir.normalized * gridSize;
            DeBody[0].transform.rotation = Quaternion.Euler(0,Mathf.Sign(moveDir.x) * Mathf.Sign(moveDir.z) * (Vector3.Angle(moveDir, transform.forward)) + 90, 0);
            for (int i = 1; i < DeBody.Count; i++)
            {
                newPos = DeBody[i].transform.position;
                moveDir = newPos - oldPos;
                DeBody[i].transform.position = oldPos;
                DeBody[i].transform.rotation = Quaternion.Euler(0, Mathf.Sign(moveDir.x) * Mathf.Sign(moveDir.z) * (Vector3.Angle(moveDir, transform.forward)) - 90, 0);
                oldPos = newPos;
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
        counter++;
        if (counter >= 4)
        {
            counter = 0;
        }
        Waypoint.transform.position = fCorners[counter];
    }
}
