using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodTest : MonoBehaviour
{
    public Material mat;
    public void move()
    {
        Grid grid = getGrid();
        mat = this.GetComponent<Renderer>().material;
        int ri = Random.Range(0, grid.boundaries.Count);
        float rx = Random.Range(grid.boundaries[ri].minX + 1, grid.boundaries[ri].maxX - 1);
        float rz = Random.Range(grid.boundaries[ri].minZ + 1, grid.boundaries[ri].maxZ - 1);
        GridPoint gP = new GridPoint(grid, new Vector3(rx,3,rz));
        this.transform.position = gP.gridPoint;
        float r = Random.Range(0, 1.0f);
        if(r < 0.5f)
        {
            mat.color = Color.green;
        }
        else
        {
            mat.color = Color.blue;
        }
        //Debug.Log(gP.gridPoint);
    }
    private Grid getGrid()
    {
        Grid grid;
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
        return grid;
    }
}
