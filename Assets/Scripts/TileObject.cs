using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Direction entryDirection;
    public Direction exitDirection;
    
    public TileObject nextTile;

    public int tileIndex;

    public TileNode[] nodes;
    void Start()
    {
        exitDirection = (Direction)Random.Range(0, 3);
        nodes = GetComponentsInChildren<TileNode>();
    }
    
    public GameObject getRandomTileNode()
    {
        if (nodes.Length <= 0) return gameObject;
        return nodes[Random.Range(0,nodes.Length)].gameObject;
    }

}
