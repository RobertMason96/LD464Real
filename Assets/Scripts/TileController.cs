using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public GameObject food;
    public float gridSize;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void createNewFood()
    {
        int maxGrid = 9;
        int xPos = Random.Range(- maxGrid + 1, maxGrid);
        int zPos = Random.Range(- maxGrid + 1, maxGrid);
        GameObject temp = Instantiate(food,this.transform.position + new Vector3(xPos* gridSize, 10, zPos * gridSize), Quaternion.identity);
        temp.transform.parent = this.transform;
    }
}
