using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WorldController : MonoBehaviour
{
    public class Team
    {
        public string Name;
        public int Score;
        public Color Colour;
        public Team(string n, Color col)
        {
            Name = n;
            Score = 0;
            Colour = col;
        }
    }
    public class Tiles
    {
        public GameObject Tile;
        public GameObject costText;
        public int tileCost;
        public Tiles(GameObject t, GameObject cT, int tC)
        {
            Tile = t;
            costText = cT;
            tileCost = tC;
        }
    }

    private List<Tiles> worldTile;

    public List<Team> teams;

    private List<GameObject> worldTiles;
    public GameObject[] tileTypes;

    public GameObject snakePre;
    private GameObject snakeInst;
    //Define World
    public float tileSize;
    public float worldSize;
    public Vector3 gridSpacing;

    public Grid grid;

    //Price of new tile per distance away from center
    public int tilePrice;
    //Price of the tiles
    private List<int> tileCost;

    public GameObject costTextPre;
    public List<GameObject> costText;
    public GameObject costCanvas;
    public Camera cam;

    public bool multiplayerTeam;
    public Color[] teamColours;
    private int[] Score;
    // Start is called before the first frame update
    void Awake()
    {
        
        if (!multiplayerTeam)
        {
            Score = new int[1];
            Score[0] = 0;
        }
        else
        {
            Score = new int[teamColours.Length];
            for (int i = 0; i < teamColours.Length; i++)
            {
                Score[i] = 0;
            }
        }

        worldTiles = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "Tiles")
            {
                worldTiles.Add(child.gameObject);
            }
        }
        createWorld();

        tileCost = setCost();

        createCostIcons();

        grid = new Grid(this.transform.position, gridSpacing, updateGrid());

        snakeInst = Instantiate(snakePre, this.transform.position, this.transform.rotation);
        snakeInst.transform.parent = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        moveUI();
    }

    private void moveUI()
    {
        for (int i = 0; i < costText.Count; i++)
        {
            costText[i].transform.position = cam.WorldToScreenPoint(worldTiles[i].transform.position);
        }
    }

    private void TestTiles()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            if (worldTiles.Count >= 1)
            {
                worldTiles[1].SetActive(true);
                grid.boundaries = updateGrid();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            if (worldTiles.Count >= 2)
            {
                worldTiles[2].SetActive(true);
                grid.boundaries = updateGrid();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            if (worldTiles.Count >= 3)
            {
                worldTiles[3].SetActive(true);
                grid.boundaries = updateGrid();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            if (worldTiles.Count >= 4)
            {
                worldTiles[4].SetActive(true);
                grid.boundaries = updateGrid();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            if (worldTiles.Count >= 5)
            {
                worldTiles[5].SetActive(true);
                grid.boundaries = updateGrid();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            if (worldTiles.Count >= 6)
            {
                worldTiles[6].SetActive(true);
                grid.boundaries = updateGrid();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            if (worldTiles.Count >= 7)
            {
                worldTiles[7].SetActive(true);
                grid.boundaries = updateGrid();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            if (worldTiles.Count >= 8)
            {
                worldTiles[8].SetActive(true);
                grid.boundaries = updateGrid();
            }
        }
    }

    private void createCostIcons()
    {
        costText = new List<GameObject>();
        for(int i = 0; i < worldTiles.Count; i++)
        {
            costText.Add(Instantiate(costTextPre, new Vector3(0,0,0), Quaternion.identity));
            
            costText[i].transform.SetParent(costCanvas.transform);
            costText[i].transform.position = cam.WorldToScreenPoint(worldTiles[i].transform.position);
            if (tileCost[i]>0)
            {
                costText[i].GetComponent<Text>().text = " " + tileCost[i];
            }
            else
            {
                costText[i].GetComponent<Text>().text = " ";
            }
        }
    }

    private void createWorld()
    {
        int type = 0;
        Vector3 pos = new Vector3(0,0,0);
        for (int x = 0; x < worldSize; x++)
        {
            for (int z = 0; z < worldSize; z++)
            {
                pos = this.transform.position + (new Vector3((x - ((worldSize - 1) / 2)), 0, (z - ((worldSize - 1) / 2))) * (tileSize + 2));
                pos.y = -2;
                if ((pos.x == this.transform.position.x) && (pos.z == this.transform.position.z))
                {

                }
                else { 
                    type = (int)Random.Range(0, tileTypes.Length);
                    worldTiles.Add(Instantiate(tileTypes[type], pos, Quaternion.identity));
                    worldTiles[worldTiles.Count - 1].transform.localScale = new Vector3(tileSize + 2, 1, tileSize + 2);
                    worldTiles[worldTiles.Count - 1].transform.parent = this.transform;
                    worldTiles[worldTiles.Count - 1].SetActive(false);
                }
            }
        }
    }

    private List<int> setCost()
    {
        List<int> temp = new List<int>();
        foreach (GameObject gObj in worldTiles)
        {
            temp.Add((int)(Mathf.Floor((worldTiles[0].transform.position - gObj.transform.position).magnitude * (tilePrice/ tileSize) ) ) );
        }
        return temp;
    }

    private List<Boundary> updateGrid()
    {
        List<Boundary> b = new List<Boundary>();
        Vector3 tempC; //Center
        Vector3 tempE; //Extent
        foreach (GameObject gObj in worldTiles)
        {
            if (gObj.activeSelf == true)
            {
                tempC = gObj.GetComponent<Renderer>().bounds.center;
                tempE = gObj.GetComponent<Renderer>().bounds.extents;
                b.Add(new Boundary(tempC.x - tempE.x, tempC.x + tempE.x, tempC.z - tempE.z, tempC.z + tempE.z));
            }
        }
        return b;
    }

    private void OnTriggerExit(Collider collision)
    {
        
        if (collision.gameObject.tag == "Pickup")
        {
            int tileIndex = closestTile(collision.gameObject.transform);
            if(tileCost[tileIndex] > 0)
            {
                tileCost[tileIndex]--;
                if(tileCost[tileIndex] <= 0)
                {

                    worldTiles[tileIndex].SetActive(true);
                    costText[tileIndex].GetComponent<Text>().text = " ";
                }
                else
                {
                    costText[tileIndex].GetComponent<Text>().text = " " + tileCost[tileIndex];
                }

            }
            grid.boundaries = updateGrid();
            /*FoodTest fT = collision.gameObject.GetComponent<FoodTest>();
            if (fT != null)
            {
                fT.move();
            }*/
            Destroy(collision.gameObject);

        }
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.position = new Vector3(0, 10, 0);
            Debug.Log("Dead");
            //Destroy(collision.gameObject);

        }
    }

    private int closestTile(Transform dest)
    {
        int result = 0;
        float shortestDistance = float.MaxValue;
        for(int i = 0; i < worldTiles.Count; i++)
        {
            if (shortestDistance >= (dest.position - worldTiles[i].transform.position).magnitude)
            {
                result = i;
                shortestDistance = (dest.position - worldTiles[i].transform.position).magnitude;
            }
        }
        return result;
    }

    public void increaseScore(Color col)
    {
        if (!multiplayerTeam)
        {
            Score[0]++;
            Debug.Log(Score[0]);
        }
        else
        {
            for (int i = 0; i < teamColours.Length; i++)
            {
                if(col == teamColours[i])
                {
                    Score[i]++;
                    Debug.Log(Score[i]);
                }
            }
        }
        
    }
}
