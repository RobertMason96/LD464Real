using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public GameObject food;
    public float gridSize;
    public float dropTime;
    private float timer;
    public int foodMax;
    public string foodTag;
    public float groundSize;
    // Start is called before the first frame update
    void Start()
    {
        timer = dropTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= dropTime)
        {
            if (GameObject.FindGameObjectsWithTag(foodTag).Length < foodMax)
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
        createNewFood();
    }

    private void createNewFood()
    {
        if (GameObject.FindGameObjectsWithTag(foodTag).Length < foodMax)
        {
            int maxGrid = (int) Mathf.Floor(groundSize / gridSize);
            int xPos = Random.Range(-maxGrid + 1, maxGrid);
            int zPos = Random.Range(-maxGrid + 1, maxGrid);
            GameObject temp = Instantiate(food, this.transform.position + new Vector3(xPos * gridSize, 10, zPos * gridSize), Quaternion.Euler(-90,0,0));
            temp.transform.parent = this.transform;
        }
    }
}
