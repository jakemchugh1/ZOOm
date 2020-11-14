using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
public class MapGenerator : MonoBehaviour {
	
	public Transform tilePrefab;
	public Transform obstaclePrefab;
	public Vector2 mapSize;
	private Transform mapHolder;
	[Range(0,1)]
	public float outlinePercent;
	[Range(0,1)]
	public float scalePercent;
	List<Coord> allTileCoords;

	Coord mapCentre;
	private List<TileData> tileList;
   
    private BinaryFormatter formatter;
	
	public Button btnLoad, btnCreate, btnSave, btnReset;

	private TileData preTile = null;

	void Start() {
		Button btn = btnLoad.GetComponent<Button>();
		btn.onClick.AddListener(LoadData);
		Button btnR = btnReset.GetComponent<Button>();
		btnR.onClick.AddListener(resetTrack);
		Button btnC = btnCreate.GetComponent<Button>();
		btnC.onClick.AddListener(createTrack);
		Button btnS = btnSave.GetComponent<Button>();
		btnS.onClick.AddListener(saveTrack);
		this.tileList = new List<TileData>();
        this.formatter = new BinaryFormatter();
		GenerateMap ();
		

	}
	void Update(){
		 if (Input.GetKeyDown(KeyCode.W))
        {
            mapCentre.y +=1;
			Vector3 tilePosition = CoordToPositionUp(mapCentre.x,mapCentre.y);
			Transform newTile = Instantiate (obstaclePrefab, tilePosition, Quaternion.identity) as Transform;
			newTile.localScale = Vector3.one * (scalePercent - outlinePercent);
			newTile.parent = mapHolder;

			TileData t = new TileData(preTile.exitDirection, Direction.Up);
            this.tileList.Add(t);
			preTile = t;
        }
		if (Input.GetKeyDown(KeyCode.A))
        {
            mapCentre.x -=1;
			Vector3 tilePosition = CoordToPositionUp(mapCentre.x,mapCentre.y);
				Transform newTile = Instantiate (obstaclePrefab, tilePosition, Quaternion.identity) as Transform;
				newTile.localScale = Vector3.one * (scalePercent - outlinePercent);
				newTile.parent = mapHolder;
			TileData t = new TileData(preTile.exitDirection, Direction.Left);
            this.tileList.Add(t);
			preTile = t;
        }
		if (Input.GetKeyDown(KeyCode.S))
        {
            mapCentre.y -=1;
			Vector3 tilePosition = CoordToPositionUp(mapCentre.x,mapCentre.y);
				Transform newTile = Instantiate (obstaclePrefab, tilePosition, Quaternion.identity) as Transform;
				newTile.localScale = Vector3.one * (scalePercent - outlinePercent);
				newTile.parent = mapHolder;
			TileData t = new TileData(preTile.exitDirection, Direction.Down);
            this.tileList.Add(t);
			preTile = t;	
        }
		if (Input.GetKeyDown(KeyCode.D))
        {
            mapCentre.x +=1;
			Vector3 tilePosition = CoordToPositionUp(mapCentre.x,mapCentre.y);
				Transform newTile = Instantiate (obstaclePrefab, tilePosition, Quaternion.identity) as Transform;
				newTile.localScale = Vector3.one * (scalePercent - outlinePercent);
				newTile.parent = mapHolder;
			TileData t = new TileData(preTile.exitDirection, Direction.Right);
            this.tileList.Add(t);
			preTile = t;	
        }

	}
	
	public void GenerateMap() {

		allTileCoords = new List<Coord> ();
		for (int x = 0; x < mapSize.x; x ++) {
			for (int y = 0; y < mapSize.y; y ++) {
				allTileCoords.Add(new Coord(x,y));
			}
		}
		mapCentre = new Coord ((int)mapSize.x/2 , 0);

		string holderName = "Generated Map";
		if (transform.Find (holderName)) {
			DestroyImmediate (transform.Find (holderName).gameObject);
		}
		
		mapHolder = new GameObject (holderName).transform;
		mapHolder.parent = transform;
		
		
		for (int x = 0; x < mapSize.x; x ++) {
			for (int y = 0; y < mapSize.y; y ++) {
			
				Vector3 tilePosition = CoordToPosition(x,y);
				Transform newTile = Instantiate (tilePrefab, tilePosition, Quaternion.identity) as Transform;
				newTile.localScale = Vector3.one * (scalePercent - outlinePercent);
				newTile.parent = mapHolder;
			
			}
		}
		
		

	}

	public void drawTiles(){
		for (int i = 0; i<this.tileList.Count; i++){
        TileData t = tileList[i];

             switch (t.exitDirection)
             {
                case(Direction.Up):
                    mapCentre.y +=1;
                break;    
                case(Direction.Right):
                    mapCentre.x += 1 ;
                break; 
                case(Direction.Left):
                    mapCentre.x -= 1 ;
                break; 
                case(Direction.Down):
                    mapCentre.y -= 1 ;

                break; 
                default:
    			mapCentre = new Coord ((int)mapSize.x / 2, 0);
                break; 

             }  
			 Vector3 tilePosition = CoordToPositionUp(mapCentre.x,mapCentre.y);
				Transform newTile = Instantiate (obstaclePrefab, tilePosition, Quaternion.identity) as Transform;
				newTile.localScale = Vector3.one * (scalePercent - outlinePercent);
				newTile.parent = mapHolder;
		}

	}
	public void LoadData() 
    {
      
     
        if (File.Exists("Track2"))
        {
 
            try
            {
                // Create a FileStream will gain read access to the 
                // data file.
                FileStream readerFileStream = new FileStream("Track2", 
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
		drawTiles();
         
    } 

	public void resetTrack(){
		 GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
         for(int i=0; i< tiles.Length; i++)
         {
             Destroy(tiles[i]);
         }
		 tileList.Clear();
	} 
	 
	 public void createTrack(){
		resetTrack();
		mapCentre = new Coord ((int)mapSize.x/2 , 0);
		Vector3 tilePosition = CoordToPositionUp(mapCentre.x,mapCentre.y);
				Transform newTile = Instantiate (obstaclePrefab, tilePosition, Quaternion.identity) as Transform;
				newTile.localScale = Vector3.one * (scalePercent - outlinePercent);
				newTile.parent = mapHolder;
		preTile =   new TileData(Direction.Origin, Direction.Origin);
		tileList.Add(preTile);
		
	 }

	 public void saveTrack(){
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
	// end public bool Load()
	Vector3 CoordToPosition(int x, int y) {
		return new Vector3 (scalePercent*(-mapSize.x / 2 + 0.5f + x),  scalePercent*(-mapSize.y / 2 + 0.5f + y), 0);
	}
	Vector3 CoordToPositionUp(int x, int y) {
		return new Vector3 (scalePercent*(-mapSize.x / 2 + 0.5f + x),  scalePercent*(-mapSize.y / 2 + 0.5f + y), -0.001f);
	}
	

	public struct Coord {
		public int x;
		public int y;

		public Coord(int _x, int _y) {
			x = _x;
			y = _y;
		}

		public static bool operator ==(Coord c1, Coord c2) {
			return c1.x == c2.x && c1.y == c2.y;
		}

		public static bool operator !=(Coord c1, Coord c2) {
			return !(c1 == c2);
		}

	}
}