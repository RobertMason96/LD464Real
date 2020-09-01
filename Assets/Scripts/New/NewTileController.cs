using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTileController : MonoBehaviour
{
    private Grid grid;

    public GameObject food;

    private float timerNew = 0;
    private float timerSpawn = 0;

    public float rateNew;
    public float rateSpawn;

    public int maxFood;
    public int minFood;

    private int curMaxFood;
    public int curMaxFoodInitial;

    // Start is called before the first frame update
    void Start()
    {
        //Want the boundary to only be around this tile only
        Vector3 tempC = this.GetComponent<Renderer>().bounds.center;
        Vector3 tempE = this.GetComponent<Renderer>().bounds.extents;
        Boundary b = new Boundary(tempC.x - tempE.x, tempC.x + tempE.x, tempC.z - tempE.z, tempC.z + tempE.z);
        if (GameObject.FindWithTag("World").GetComponent<WorldController>() != null)
        {
            grid = new Grid(GameObject.FindWithTag("World").GetComponent<WorldController>().grid, b);
        }
        else
        {
            grid = new Grid(this.transform.position, new Vector3(2, 2, 2), b);
        }
        
        curMaxFood = curMaxFoodInitial;

        for(int i = 0; i < minFood; i++)
        {
            newFood();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (this.transform.childCount < curMaxFood)
        {
            timerNew += Time.deltaTime;
            timerSpawn += Time.deltaTime;
            if(timerSpawn >= rateSpawn)
            {
                newFood();
                timerSpawn = 0;
            }
            if(timerNew > rateNew)
            {
                if(curMaxFood < maxFood)
                {
                    curMaxFood++;
                    timerNew = 0;
                }
            }
            if ((this.transform.childCount < minFood) && (this.transform.childCount < maxFood))
            {
                newFood();
            }

        }
    }

    private void newFood()
    {
        //Create New Food
        //Randomly find point in grid
        float rx = Random.Range(grid.boundaries[0].minX + 1, grid.boundaries[0].maxX - 1);
        float rz = Random.Range(grid.boundaries[0].minZ + 1, grid.boundaries[0].maxZ - 1);
        GridPoint gP = new GridPoint(grid, new Vector3(rx, 3, rz));
        //Spawn food at that point
        GameObject newFood = Instantiate(food, gP.gridPoint, Quaternion.identity);
        newFood.transform.parent = this.transform;
        //If this is multiplayer randomly choose food colour from team colours
        if (GameObject.FindWithTag("World").GetComponent<WorldController>() != null)
        {
            if (GameObject.FindWithTag("World").GetComponent<WorldController>().multiplayerTeam)
            {
                int numTeams = GameObject.FindWithTag("World").GetComponent<WorldController>().teamColours.Length;
                int r = Random.Range(0, numTeams);
                if(numTeams != 0)
                {
                    newFood.GetComponent<Renderer>().material.color = GameObject.FindWithTag("World").GetComponent<WorldController>().teamColours[r];
                }
            }
        }
    }
}
