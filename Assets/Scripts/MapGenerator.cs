using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
	Queue<Coord> shuffledTileCoords;

	Coord mapCentre;
	private List<TileData> tileList;
   
    private BinaryFormatter formatter;
	
	void Start() {
		this.tileList = new List<TileData>();
        this.formatter = new BinaryFormatter();
		LoadData();
		GenerateMap ();
		

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
				if(x==mapCentre.x&&y==mapCentre.y){
				Vector3 tilePosition = CoordToPositionUp(x,y);
				Transform newTile = Instantiate (obstaclePrefab, tilePosition, Quaternion.identity) as Transform;
				newTile.localScale = Vector3.one * (scalePercent - outlinePercent);
				newTile.parent = mapHolder;
				}else{
				Vector3 tilePosition = CoordToPosition(x,y);
				Transform newTile = Instantiate (tilePrefab, tilePosition, Quaternion.identity) as Transform;
				newTile.localScale = Vector3.one * (scalePercent - outlinePercent);
				newTile.parent = mapHolder;
				}
			
			}
		}
		
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

         
    } 
	// end public bool Load()
	Vector3 CoordToPosition(int x, int y) {
		return new Vector3 (scalePercent*(-mapSize.x / 2 + 0.5f + x),  scalePercent*(-mapSize.y / 2 + 0.5f + y), 0);
	}
	Vector3 CoordToPositionUp(int x, int y) {
		return new Vector3 (scalePercent*(-mapSize.x / 2 + 0.5f + x),  scalePercent*(-mapSize.y / 2 + 0.5f + y), -0.001f);
	}
	public Coord GetRandomCoord() {
		Coord randomCoord = shuffledTileCoords.Dequeue ();
		shuffledTileCoords.Enqueue (randomCoord);
		return randomCoord;
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