using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Boundary
{
    //defines the boundaries for a square grid, for the tiles
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
    public Boundary(float lX, float uX, float lZ, float uZ)
    {
        minX = lX;
        maxX = uX;
        minZ = lZ;
        maxZ = uZ;
    }
}

public class Grid 
{
    public Vector3 centre;
    public Vector3 gridSpacing;
    public List<Boundary> boundaries;
    public Grid(Vector3 c, Vector3 g, List<Boundary> b)
    {
        centre = c;
        gridSpacing = g;
        boundaries = b;
    }
    public Grid(Vector3 c, Vector3 g, Boundary b)
    {
        centre = c;
        gridSpacing = g;
        boundaries = new List<Boundary>();
        boundaries.Add(b);
    }
    public Grid(Grid g, Boundary b) 
    {
        centre = g.centre;
        gridSpacing = g.gridSpacing;
        boundaries = new List<Boundary>();
        boundaries.Add(b);
    }
    
}

public class GridPoint
{
    public Grid grid;
    public Vector3 gridPoint;
    public bool inbounds;
    public GridPoint(Grid g, Vector3 point)
    {
        grid = g;
        inbounds = false;
        bool inX = false; 
        bool inZ = false;
        Vector3 shortestDist = new Vector3(float.MaxValue, point.y, float.MaxValue);
        for(int i = 0; i < grid.boundaries.Count; i++)
        {
            if ((point.x >= grid.boundaries[i].minX) &&  (point.x <= grid.boundaries[i].maxX) && (point.z >= grid.boundaries[i].minZ) && (point.z <= grid.boundaries[i].maxZ))
            {
                inbounds = true;
            }else
            {
                if((point.x >= grid.boundaries[i].minX) && (point.x <= grid.boundaries[i].maxX) && !inZ)
                {
                    shortestDist.x = 0;
                    inX = true;
                }
                if((point.z >= grid.boundaries[i].minZ) && (point.z <= grid.boundaries[i].maxZ) && !inX)
                {
                    shortestDist.z = 0;
                    inZ = true;
                }
                if (Mathf.Abs(grid.boundaries[i].minX - point.x) < Mathf.Abs(shortestDist.x))
                {
                    shortestDist.x = grid.boundaries[i].minX - point.x;
                }
                if (Mathf.Abs(grid.boundaries[i].maxX - point.x) < Mathf.Abs(shortestDist.x))
                {
                    shortestDist.x = grid.boundaries[i].maxX - point.x;
                }
                if (Mathf.Abs(grid.boundaries[i].minZ - point.z) < Mathf.Abs(shortestDist.z))
                {
                    shortestDist.z = grid.boundaries[i].minZ - point.z;
                }
                if (Mathf.Abs(grid.boundaries[i].maxZ - point.z) < Mathf.Abs(shortestDist.z))
                {
                    shortestDist.z = grid.boundaries[i].maxZ - point.z;
                }
            }
        }
        if (!inbounds)
        {
            point = shortestDist + point;
            //Debug.Log(point);
        }
        gridPoint = findClosestGridPoint(g, point);
    }
    private Vector3 findClosestGridPoint(Grid g, Vector3 point)
    {
        Vector3 temp = (point - g.centre);
        Vector3 gS = g.gridSpacing;
        temp = new Vector3(Mathf.Sign(temp.x) * gS.x * Mathf.Floor(Mathf.Abs(temp.x / gS.x)), Mathf.Sign(temp.y) * gS.y * Mathf.Floor(Mathf.Abs(temp.y / gS.y)), Mathf.Sign(temp.z) * gS.z * Mathf.Floor(Mathf.Abs(temp.z / gS.z))) + g.centre;
        //Check boundaries Maybe?
        return temp;
    }
}
