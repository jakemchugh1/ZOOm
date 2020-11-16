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

    public GameObject tileIntersect;

    public GameObject tileTurnLR;

       
    public int numbOfTiles;

    public List<GameObject> tiles;

    public Vector3 spawnOrigin;

    public Vector3 spawnPosition;

    private GameObject preTile;

    public float tileWidth = 3f; 

    public float tileHeight = 3f;


    private List<TileData> tileList;
   
    private BinaryFormatter formatter;

     
    // Start is called before the first frame update
    void Start()
    {
        spawnOrigin = new Vector3(0,0,0);
        this.tileList = new List<TileData>();
        this.formatter = new BinaryFormatter();
        // spawnTiles();
        LoadData();

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

    void spawnTiles()
    {
        for(int i = 0; i < numbOfTiles; i++)
        {  
            Direction exitDirection;
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
            GameObject newTile = null;
            if(preTile){
                if(preTile.GetComponent<TileObject>().exitDirection  == Direction.Left|| preTile.GetComponent<TileObject>().exitDirection == Direction.Right)
                    newTile= Instantiate( tile, spawnOrigin,  Quaternion.Euler(0, 90, 0),transform);
                else
                    newTile= Instantiate( tile, spawnOrigin, Quaternion.identity,transform);
             newTile.GetComponent<TileObject>().entryDirection = preTile.GetComponent<TileObject>().exitDirection ;

            }else
                newTile= Instantiate( tile, spawnOrigin, Quaternion.identity,transform);
            
            newTile.GetComponent<TileObject>().exitDirection = exitDirection;
         
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
                new FileStream(GlobalVariables.selectedFile, FileMode.Create, FileAccess.Write);
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
     
        if (File.Exists(GlobalVariables.selectedFile))
        {
 
            try
            {
                // Create a FileStream will gain read access to the 
                // data file.
                FileStream readerFileStream = new FileStream(GlobalVariables.selectedFile, 
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
        GameObject previousTile = null;
        for(int i = 0; i < tileList.Count; i++){  
            TileData t = tileList[i];
        
             switch (t.entryDirection)
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
             GameObject newTile = null;
             if(i==0){
                if (t.exitDirection == Direction.Left || t.exitDirection == Direction.Right)
                {
                    newTile = Instantiate(tile, spawnOrigin, Quaternion.Euler(0, 90, 0), transform);
                }
                else
                {
                    newTile = Instantiate(tile, spawnOrigin, Quaternion.identity, transform);
                }
             }
             else if(t.entryDirection!=t.exitDirection&&t.entryDirection!=Direction.Origin){
              
               if(t.entryDirection == Direction.Up)
               {
                if (t.exitDirection == Direction.Left) {
                 newTile= Instantiate( tileTurnLR, spawnOrigin,  Quaternion.Euler(0, 270, 0),transform);
               }else if (t.exitDirection == Direction.Right) {
                   newTile= Instantiate( tileTurnLR, spawnOrigin,  Quaternion.Euler(0, 180, 0),transform);
               }else
                   newTile= Instantiate( tile, spawnOrigin, Quaternion.identity,transform);

               } 
               else if(t.entryDirection == Direction.Down)
               {
                if (t.exitDirection == Direction.Left) {
                 newTile= Instantiate( tileTurnLR, spawnOrigin,  Quaternion.Euler(0, 0, 0),transform);
               }else if (t.exitDirection == Direction.Right) {
                 newTile= Instantiate( tileTurnLR, spawnOrigin,  Quaternion.Euler(0, 90, 0),transform);
               }else
                   newTile= Instantiate( tile, spawnOrigin, Quaternion.identity,transform);
               } 
               else if(t.entryDirection == Direction.Left)
               {
                if (t.exitDirection == Direction.Up) {
                 newTile= Instantiate( tileTurnLR, spawnOrigin,  Quaternion.Euler(0, 90, 0),transform);
               }else if (t.exitDirection == Direction.Down) {
                newTile= Instantiate( tileTurnLR, spawnOrigin,  Quaternion.Euler(0, 180, 0),transform);
               }else
                   newTile= Instantiate( tile, spawnOrigin, Quaternion.Euler(0, 90, 0),transform);
               } 
               else if(t.entryDirection == Direction.Right)
               {
                if (t.exitDirection == Direction.Up) {
                newTile= Instantiate( tileTurnLR, spawnOrigin,  Quaternion.Euler(0, 0, 0),transform);
               }else if (t.exitDirection == Direction.Down) {
                 newTile= Instantiate( tileTurnLR, spawnOrigin,  Quaternion.Euler(0, 270, 0),transform);
               }else
                   newTile= Instantiate( tileIntersect, spawnOrigin, Quaternion.Euler(0, 90, 0),transform);
               } 
               else
                newTile= Instantiate( tileIntersect, spawnOrigin, Quaternion.identity,transform);

             }
             else{
                if (t.entryDirection == Direction.Left || t.entryDirection == Direction.Right)
                {
                    newTile = Instantiate(tile, spawnOrigin, Quaternion.Euler(0, 90, 0), transform);

                }
                else
                {
                    newTile = Instantiate(tile, spawnOrigin, Quaternion.identity, transform);
                }
         
             }
             newTile.GetComponent<TileObject>().exitDirection = t.exitDirection;

            if (preTile)
            {

                //preTile.GetComponent<TileObject>().nextTile = newTile.GetComponent<TileObject>();
                //newTile.GetComponent<TileObject>().previousTile = preTile.GetComponent<TileObject>();
                preTile.GetComponent<TileObject>().nextTile = newTile.GetComponent<TileObject>();
                newTile.GetComponent<TileObject>().entryDirection = preTile.GetComponent<TileObject>().exitDirection;
            }

            newTile.GetComponent<TileObject>().tileIndex = i;
            
            preTile = newTile;
            tiles.Add(newTile);
        }
        
    }

}
