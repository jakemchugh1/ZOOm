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

    public GameObject getNearestTileNode(Vector3 position)
    {
        if (nodes.Length <= 0) return gameObject;
        GameObject nearest = nodes[0].gameObject;
        float distance = Vector3.Distance(position, nearest.transform.position);
        for(int i = 1; i < nodes.Length; i++)
        {
            
            if(Vector3.Distance(position, nodes[i].transform.position) < distance)
            {
                distance = Vector3.Distance(position, nodes[i].transform.position);
                nearest = nodes[i].gameObject;
            }
        }

        return nearest;
    }

    public GameObject getNearestTrashCan(Vector3 position)
    {
        trashcanScript[] cans = GetComponentsInChildren<trashcanScript>();
        if (cans.Length <= 0) return getNearestMilk(position);
        GameObject nearest = cans[0].gameObject;
        float distance = Vector3.Distance(position, nearest.transform.position);
        for (int i = 1; i < cans.Length; i++)
        {

            if (Vector3.Distance(position, cans[i].transform.position) < distance)
            {
                distance = Vector3.Distance(position, cans[i].transform.position);
                nearest = cans[i].gameObject;
            }
        }

        return nearest.transform.parent.gameObject;
    }

    public GameObject getNearestMilk(Vector3 position)
    {
        milkScript[] cans = GetComponentsInChildren<milkScript>();
        if (cans.Length <= 0) return getNearestTileNode(position);
        GameObject nearest = cans[0].gameObject;
        float distance = Vector3.Distance(position, nearest.transform.position);
        for (int i = 0; i < cans.Length; i++)
        {

            if (Vector3.Distance(position, cans[i].transform.position) < distance)
            {
                distance = Vector3.Distance(position, cans[i].transform.position);
                nearest = cans[i].gameObject;
            }
        }

        return nearest.transform.parent.gameObject;
    }

}
