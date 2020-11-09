using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Left, Right, Up, Down, Origin
}

public class TrackSpawner : MonoBehaviour
{
    public GameObject tile;

    public int numbOfTiles;

    public List<GameObject> tiles;

    public Vector3 spawnOrigin;

    private Vector3 spawnPosition;

    private GameObject preTile;

    public float tileWidth = 3f; 

    public float tileHeight = 3f; 

     
    // Start is called before the first frame update
    void Start()
    {
        spawnOrigin = new Vector3(0,0,0);
        spawnTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void spawnTiles()
    {
        for(int i = 0; i < numbOfTiles; i++)

        {   Direction exitDirection;
            if(i==0) 
                exitDirection = Direction.Origin;
            else
                exitDirection = (Direction)Random.Range(0, 3);        
             switch (exitDirection)
             {
                case(Direction.Up):
                    spawnPosition =  new Vector3(0,0,tileHeight);
                break;    
                case(Direction.Right):
                    spawnPosition =  new Vector3(tileWidth,0,0);
                break; 
                case(Direction.Left):
                    spawnPosition =  new Vector3(-tileWidth,0,0);
                break; 
                case(Direction.Down):
                    spawnPosition =  new Vector3(0,0,-tileHeight);
                break; 
                default:
                    spawnPosition =  new Vector3(0,0,0);
                break; 

             }  
             spawnOrigin += spawnPosition;
            GameObject newTile= Instantiate( tile, spawnOrigin, Quaternion.identity);
            newTile.GetComponent<TileObject>().exitDirection = exitDirection;
            if(preTile)
               newTile.GetComponent<TileObject>().entryDirection = newTile.GetComponent<TileObject>().exitDirection ;
            preTile = newTile;
            tiles.Add(newTile);

        }
    }
}
