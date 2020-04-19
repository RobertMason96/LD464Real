using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public GameObject food;
    public float gridSize;
    public float dropTime;
    private float timer = 0;
    private int foodNum = 0;
    public int foodMax;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= dropTime)
        {
            if (foodNum < foodMax)
            {
                createNewFood();
            }
             timer = 0;
        }
        timer += Time.deltaTime;
    }
    public void replaceFood(GameObject temp)
    {
        Destroy(temp.transform.gameObject);
        foodNum--;
        createNewFood();
    }

    private void createNewFood()
    {
        if (foodNum < foodMax)
        {
            int maxGrid = 9;
            int xPos = Random.Range(-maxGrid + 1, maxGrid);
            int zPos = Random.Range(-maxGrid + 1, maxGrid);
            GameObject temp = Instantiate(food, this.transform.position + new Vector3(xPos * gridSize, 10, zPos * gridSize), Quaternion.identity);
            temp.transform.parent = this.transform;
            foodNum++;
        }
    }
}
