using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

    public Vector3 spawnPosition;

    private GameObject preTile;

    public float tileWidth = 3f; 

    public float tileHeight = 3f;

    public NavMeshSurface navMesh;

    public NavMeshAgent[] racers;

    private List<TileData> tileList;
   
    private BinaryFormatter formatter;

     
    // Start is called before the first frame update
    void Start()
    {
        spawnOrigin = new Vector3(0,0,0);
        this.tileList = new List<TileData>();
        this.formatter = new BinaryFormatter();
        spawnTiles();
        // LoadData();

        // racers = FindObjectsOfType<NavMeshAgent>();

        // navMesh = GetComponent<NavMeshSurface>();
        // //build navmesh after tiles generated
        // navMesh.BuildNavMesh();
        // //lets the racers go
        // setDestination();

        


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setDestination()
    {
        Debug.Log(spawnPosition);
        for (int i = 0; i < racers.Length; i++)
        {
            racers[i].destination = spawnOrigin;
        }
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
            GameObject newTile= Instantiate( tile, spawnOrigin, Quaternion.identity,transform);
            newTile.GetComponent<TileObject>().exitDirection = exitDirection;
            if(preTile)
               newTile.GetComponent<TileObject>().entryDirection = newTile.GetComponent<TileObject>().exitDirection ;
            preTile = newTile;
            tiles.Add(newTile);
        }
        foreach (var obj in tiles)
            {
                TileData t = new TileData(obj.GetComponent<TileObject>().entryDirection, obj.GetComponent<TileObject>().exitDirection);

                this.tileList.Add(t);
            }
            Save();
    }
    public void Save()
    {
        // Gain code access to the file that we are going
        // to write to

        try
        {
            // Create a FileStream that will write data to file.
            FileStream writerFileStream = 
                new FileStream("Track", FileMode.Create, FileAccess.Write);
            // Save our dictionary of friends to file
            this.formatter.Serialize(writerFileStream, this.tileList);
 
            // Close the writerFileStream when we are done.
            writerFileStream.Close();
        }
        catch (System.Exception e) {
            Debug.Log(e);
        } // end try-catch
    }
    public void LoadData() 
    {
      
        // Check if we had previously Save information of our friends
        // previously
     
        if (File.Exists("Track"))
        {
 
            try
            {
                // Create a FileStream will gain read access to the 
                // data file.
                FileStream readerFileStream = new FileStream("Track", 
                    FileMode.Open, FileAccess.Read);
                // Reconstruct information of our friends from file.
                this.tileList = (List<TileData>)this.formatter.Deserialize(readerFileStream);
                // Close the readerFileStream when we are done
                readerFileStream.Close();
 
            } 
            catch (System.Exception e) 
            {
                Debug.Log(e);
            } // end try-catch
 
        } // end if

        loadTiles();
         
    } // end public bool Load()
 void loadTiles()
    {
        for(int i = 0; i < tileList.Count; i++)

        {   Direction exitDirection;
        TileData t = tileList[i];
        
              Debug.Log(t.exitDirection);
             switch (t.exitDirection)
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
            GameObject newTile= Instantiate( tile, spawnOrigin, Quaternion.identity,transform);
            newTile.GetComponent<TileObject>().exitDirection = t.exitDirection;
            if(preTile)
               newTile.GetComponent<TileObject>().entryDirection = newTile.GetComponent<TileObject>().exitDirection ;
            preTile = newTile;
            tiles.Add(newTile);
        }
        
    }

}
