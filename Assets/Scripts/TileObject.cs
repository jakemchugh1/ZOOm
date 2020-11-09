using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Direction entryDirection;
    public Direction exitDirection;
    void Start()
    {
    exitDirection = (Direction)Random.Range(0, 3);        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
