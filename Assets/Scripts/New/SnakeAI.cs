using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAI : MonoBehaviour
{
    //Grid
    public Grid grid;
    //Movement
    private float initialSpeed = 0;
    public float maxSpeed;
    private float hunger;
    public float hungerRate;
    private Vector3 direction;
    private Vector3 currentGridPoint;
    //For test this is just a object selected in the editor
    //Snake Controller
    private SnakePart snakeController;
    //GridtEST
    public GameObject testGood;
    public GameObject testBad;
    private float timer = 0;


    // Start is called before the first frame update
    void Start()
    {
        snakeController = this.transform.parent.gameObject.GetComponent<SnakePart>();
        if (snakeController == null)
        {
            Debug.Log("No snake");
            Destroy(this);
        }
        else
        {
            snakeController.speed = initialSpeed;
        }
        getGrid();
        //Initialise grid
        //Grid(Vector3 c, Vector3 g, List<Boundary> b)


        
        //Set the initial Direction
        direction = new Vector3(0, 0, 1);
        currentGridPoint = this.transform.position + (direction * grid.gridSpacing.z);
        snakeController.UpdateWayPoints(currentGridPoint);
        //Should probably do this based on which direction is forward
        //gridTest();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        getGrid();

        //Update speed
        float moveSpeed = changeSpeed();
        snakeController.speed = moveSpeed;
        //At grid point?
        if((this.transform.position - currentGridPoint).magnitude < moveSpeed)
        {
            ////Yes - gridUpdate
            currentGridPoint = gridUpdate(moveSpeed);

        }
        else
        {
            ////No - Move towards grid point
            
        }

        moveSnake(moveSpeed);

    }

    private void getGrid()
    {
        if (GameObject.FindWithTag("World").GetComponent<WorldController>() != null)
        {
            grid = GameObject.FindWithTag("World").GetComponent<WorldController>().grid;
        }
        else
        {
            List<Boundary> b = new List<Boundary>();
            b.Add(new Boundary(-11, 11, -11, 11));
            grid = new Grid(new Vector3(0, 0, 0), new Vector3(2, 2, 2), b);
        }
    }

    private Vector3 gridUpdate(float mS)
    {
        //try to find food
        GameObject dest = findFood();
        Vector3 nextGridPoint;
        //Does food exist?
        if (dest == null)
        {
            nextGridPoint = noFood();
        }
        else
        {
            //// Yes
            nextGridPoint = findNextGridPoint(dest.transform.position, mS);
            //////Find next grid point on route
            //////Move towards next grid point
        }
        snakeBody(nextGridPoint);
        return nextGridPoint;
    }

    private void snakeBody(Vector3 nextGridPoint)
    {
        //sends information on  the new waypoint to the rest of the snake
        snakeController.UpdateWayPoints(nextGridPoint);
    }

    private Vector3 noFood()
    {
        Vector3 nextGridPoint = currentGridPoint + new Vector3(direction.x * grid.gridSpacing.x, direction.y * grid.gridSpacing.y, direction.z * grid.gridSpacing.z);
        //// No - move grid point 1 forward (unless out of bounds then turn)
        GridPoint gP = new GridPoint(grid, nextGridPoint);
        //Is forward 1 inbounds?
        if (gP.inbounds)
        {

        }
        else
        {
            //if not turn right
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                direction = new Vector3(0, 0, 1);
            }
            else
            {
                direction = new Vector3(1, 0, 0);
            }
            nextGridPoint = currentGridPoint + new Vector3(direction.x * grid.gridSpacing.x, direction.y * grid.gridSpacing.y, direction.z * grid.gridSpacing.z);
            gP = new GridPoint(grid, nextGridPoint);
            //is this point inbounds
            if (gP.inbounds)
            {

            }
            else
            {
                //if its turn left
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                {
                    direction = new Vector3(-1, 0, 0);
                }
                else
                {
                    direction = new Vector3(0, 0, -1);
                }
                nextGridPoint = currentGridPoint + new Vector3(direction.x * grid.gridSpacing.x, direction.y * grid.gridSpacing.y, direction.z * grid.gridSpacing.z);
                gP = new GridPoint(grid, nextGridPoint);
                //is this inbound?
                if (gP.inbounds)
                {

                }
                else
                {
                    //snake can't move anywhere kill snake
                    Debug.Log("Error, snake out of bounds");
                }
            }

        }
        return gP.gridPoint;
    }

    private Vector3 findNextGridPoint(Vector3 dest, float mS)
    {
        //Moving in x or z?
        //Is food behind in direction?
        ////Yes - Change direction
        ////No 
        ////Inline perpendicular to direction
        //////Yes - Change direction
        //////No - Move forward
        Vector3 point;
        Vector3 moveDir = new Vector3(dest.x - this.transform.position.x, 0, dest.z - this.transform.position.z);
        if ((Mathf.Abs(moveDir.x) < (mS * Mathf.Abs(direction.x))) || (Mathf.Abs(moveDir.z) < (mS * Mathf.Abs(direction.z))))
        {
            point = changeDirection(dest);
        }
        else if (Mathf.Sign(moveDir.x) == (- Mathf.Sign(direction.x) * Mathf.Abs(direction.x)) || Mathf.Sign(moveDir.z) == (-Mathf.Sign(direction.z) * Mathf.Abs(direction.z)))
        {
            point = changeDirection(dest);
        }
        else
        {
            point = moveForward(dest);
        }
        return point;

    }

    private Vector3 moveForward(Vector3 dest)
    {
        GridPoint gP = new GridPoint(grid, (currentGridPoint + new Vector3(direction.x * grid.gridSpacing.x, 0, direction.z * grid.gridSpacing.z)));
        Vector3 temp = new Vector3(Mathf.Abs(direction.z) * Mathf.Sign(dest.x - this.transform.position.x), 0, Mathf.Abs(direction.x) * Mathf.Sign(dest.z - this.transform.position.z));
        if (gP.inbounds)
        {

        }
        else
        {
            gP = new GridPoint(grid, (currentGridPoint + new Vector3(temp.x * grid.gridSpacing.x, 0, temp.z * grid.gridSpacing.z)));
            if (gP.inbounds)
            {
                direction = temp;
            }
            else
            {
                gP = new GridPoint(grid, (currentGridPoint + new Vector3(-temp.x * grid.gridSpacing.x, 0, -temp.z * grid.gridSpacing.z)));
                if (gP.inbounds)
                {
                    direction = -temp;
                }
                else
                {
                    Debug.Log("Its lost, cannot move inbounds, moveForward");
                }
            }
        }
        return gP.gridPoint;
    }

    private Vector3 changeDirection(Vector3 dest)
    {
        Vector3 temp = new Vector3(Mathf.Abs(direction.z) * Mathf.Sign(dest.x - this.transform.position.x), 0, Mathf.Abs(direction.x) * Mathf.Sign(dest.z - this.transform.position.z));
        GridPoint gP = new GridPoint(grid, (currentGridPoint + new Vector3(temp.x * grid.gridSpacing.x, 0, temp.z * grid.gridSpacing.z)));
        if(gP.inbounds){
            //Good
            direction = temp;
        }
        else
        {
            //Look at space forward
            gP = new GridPoint(grid, (currentGridPoint + new Vector3(direction.x * grid.gridSpacing.x, 0, direction.z * grid.gridSpacing.z)));
            if (gP.inbounds)
            {

            }
            else
            {
                gP = new GridPoint(grid, (currentGridPoint + new Vector3(-temp.x * grid.gridSpacing.x, 0, -temp.z * grid.gridSpacing.z)));
                if (gP.inbounds)
                {
                    direction = -temp;
                }
                else
                {
                    Debug.Log("Its lost, cannot move inbounds, changeDirection");
                }
            }
        }
        return gP.gridPoint;
    }

    private GameObject findFood()
    {
        GameObject temp = null;
        float shortestDistance;
        GridPoint gP;
        GameObject[] F = GameObject.FindGameObjectsWithTag("Pickup");
        if (F.Length != 0)
        {
            shortestDistance = (this.transform.position - F[0].transform.position).magnitude;
            foreach (GameObject gObj in F)
            {
                if (shortestDistance >= (this.transform.position - gObj.transform.position).magnitude)
                {
                    if (Mathf.Abs((this.transform.position - gObj.transform.position).y) < 2)
                    {
                        gP = new GridPoint(grid, gObj.transform.position);
                        if (gP.inbounds)
                        {
                            temp = gObj;
                        }
                        shortestDistance = (this.transform.position - gObj.transform.position).magnitude;
                    }
                }
            }
        }
        return temp;
        //Find food
    }

    private void moveSnake(float mS)
    {
        //Move and rotate all snake parts (except the head thats handled in the path finding)
        this.transform.rotation = rotatePart();
        this.transform.position = movePart(mS);
    }

    private Quaternion rotatePart()
    {
        //Rotate the piece to face the piece in front
        return Quaternion.LookRotation(direction);
    }

    private Vector3 movePart(float mS)
    {
        //Move the piece towards the piece in front 
        //Maybe (SnakeBody.transform.position + ((SnakeBody.transform.forward * moveSpeed * timeDeltaTime) * Mathf.Sign((SnakeBody[part].transform.position - SnakeBody[part - 1].transform.position).magnitude - PartLength)));
        return (this.transform.position += (( currentGridPoint - this.transform.position).normalized) * mS);
    }

    private float changeSpeed()
    {
        float moveSpeed = (initialSpeed + (hunger)) ;
        if (moveSpeed < maxSpeed)
        {
            hunger += (hungerRate);
        }
        else
        {
            moveSpeed = maxSpeed;
        }
        return moveSpeed * Time.fixedDeltaTime;

    }

    private void gridTest()
    {
        GridPoint gP;
        gP = new GridPoint(grid, new Vector3(-7, 0, 0));
        for(int x = -7; x <= 7; x++)
        {
            for (int z = -7; z <= 7; z++)
            {
                gP = new GridPoint(grid, new Vector3(x,0,z));
                if (gP.inbounds)
                {
                    Instantiate(testGood, gP.gridPoint + new Vector3(0,2,0), Quaternion.identity);
                }
                else
                {
                    Instantiate(testBad, gP.gridPoint, Quaternion.identity);
                }
                
            }
        }
    }


    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Pickup")
        {
            /*FoodTest fT = collision.gameObject.GetComponent<FoodTest>();
            hunger = 0;
            //
            if (fT != null)
            {
                
                fT.move();
            }*/
            hunger = 0;
            
            if (GameObject.FindWithTag("World").GetComponent<WorldController>() != null)
            {
                GameObject.FindWithTag("World").GetComponent<WorldController>().increaseScore(collision.gameObject.GetComponent<Renderer>().material.color);
            }
            snakeController.createNewPart(collision.gameObject.GetComponent<Renderer>().material);
            Destroy(collision.gameObject);
        }

    }
}
