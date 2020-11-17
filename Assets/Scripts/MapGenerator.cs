using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	private bool isCreating;

	Coord mapCentre;
	private List<TileData> tileList;
   
    private BinaryFormatter formatter;
	
	public Button btnLoad, btnCreate, btnSave, btnReset, btnDone;

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
		Button btnD = btnDone.GetComponent<Button>();
		btnD.onClick.AddListener(backHome);
		this.tileList = new List<TileData>();
        this.formatter = new BinaryFormatter();
		isCreating = false;
		GenerateMap ();
		

	}
	void Update(){
		if(isCreating ==false)
		return;	
		 if (Input.GetKeyDown(KeyCode.W))
        {		
			if(this.tileList.Count!=0)
            mapCentre.y +=1;
			else
			  mapCentre.y = 0;

			Vector3 tilePosition = CoordToPositionUp(mapCentre.x,mapCentre.y);
			Transform newTile = Instantiate (obstaclePrefab, tilePosition, Quaternion.identity) as Transform;
			newTile.localScale = Vector3.one * (scalePercent - outlinePercent);
			newTile.parent = mapHolder;
			TileData t;
			if(preTile!=null) 
			t = new TileData(preTile.exitDirection, Direction.Up);
            else
			{
				t = new TileData(Direction.Origin, Direction.Up);
			}
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
			TileData t;
			if(preTile!=null)	
			t = new TileData(preTile.exitDirection, Direction.Left);
			else
			t = new TileData(Direction.Origin, Direction.Left);
            this.tileList.Add(t);
			preTile = t;
        }
		if (Input.GetKeyDown(KeyCode.S))
        {
			if(this.tileList.Count!=0)
            mapCentre.y -=1;
			else
			  mapCentre.y = 0;
			Vector3 tilePosition = CoordToPositionUp(mapCentre.x,mapCentre.y);
				Transform newTile = Instantiate (obstaclePrefab, tilePosition, Quaternion.identity) as Transform;
				newTile.localScale = Vector3.one * (scalePercent - outlinePercent);
				newTile.parent = mapHolder;
			TileData t;
			if(preTile!=null)	
			t = new TileData(preTile.exitDirection, Direction.Down);
			else
			t = new TileData(Direction.Origin, Direction.Down);            this.tileList.Add(t);
			preTile = t;	
        }
		if (Input.GetKeyDown(KeyCode.D))
        {
				
            mapCentre.x +=1;
			Vector3 tilePosition = CoordToPositionUp(mapCentre.x,mapCentre.y);
				Transform newTile = Instantiate (obstaclePrefab, tilePosition, Quaternion.identity) as Transform;
				newTile.localScale = Vector3.one * (scalePercent - outlinePercent);
				newTile.parent = mapHolder;
			TileData t;
			if(preTile!=null)	
			t = new TileData(preTile.exitDirection, Direction.Right);
			else
			t = new TileData(Direction.Origin, Direction.Right);            
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
  		mapCentre = new Coord ((int)mapSize.x / 2, 0);
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
        isCreating = false;
		resetTrack();
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
		drawTiles();
         
    } 

	public void resetTrack(){
		        isCreating = false;

		 GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
         for(int i=0; i< tiles.Length; i++)
         {
             Destroy(tiles[i]);
         }
		 tileList.Clear();
		 mapCentre = new Coord ((int)mapSize.x / 2, 0);

	} 
	 
	 public void createTrack(){
		preTile = null;
		resetTrack();
		isCreating = true;

		mapCentre = new Coord ((int)mapSize.x/2 , 0);
		// Vector3 tilePosition = CoordToPositionUp(mapCentre.x,mapCentre.y);
		// 		Transform newTile = Instantiate (obstaclePrefab, tilePosition, Quaternion.identity) as Transform;
		// 		newTile.localScale = Vector3.one * (scalePercent - outlinePercent);
		// 		newTile.parent = mapHolder;
		// preTile =   new TileData(Direction.Origin, Direction.Origin);
		// tileList.Add(preTile);
		
	 }

	 public void saveTrack(){
		 isCreating = false;
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


	 public void backHome(){
		 SceneManager.LoadScene(2);
	 }

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