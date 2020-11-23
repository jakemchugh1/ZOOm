using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeperScript : MonoBehaviour
{
    public RacerBehaviorScript[] drivers;

    public TextMeshProUGUI lapText;
    public TextMeshProUGUI first;
    public TextMeshProUGUI second;
    public TextMeshProUGUI third;
    public TextMeshProUGUI fourth;
    // Start is called before the first frame update
    void Start()
    {
        drivers = FindObjectsOfType<RacerBehaviorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GlobalVariables.paused)
        {
            lapText.text = ("Lap: " + drivers[0].lap + "/3");
            SortFirst();
        }
        
    }

    void SortFirst()
    {
        System.Array.Sort(drivers, new driverCompare());
        first.text = "1st: Player "+drivers[0].playerNumber;
        drivers[0].place = 1;
        second.text = "2nd: Player " + drivers[1].playerNumber;
        drivers[1].place = 2;
        third.text = "3rd: Player " + drivers[2].playerNumber;
        drivers[2].place = 3;
        fourth.text = "4th: Player " + drivers[3].playerNumber;
        drivers[3].place = 4;
    }
    class driverCompare : IComparer<RacerBehaviorScript>
    {
        public int Compare(RacerBehaviorScript left, RacerBehaviorScript right)
        {
            //compare lap
            if (left.lap > right.lap) return -1;
            if (left.lap < right.lap) return 1;
            //compare tile
            if (left.checkpoint.GetComponent<TileObject>().tileIndex > right.checkpoint.GetComponent<TileObject>().tileIndex) return -1;
            if (left.checkpoint.GetComponent<TileObject>().tileIndex < right.checkpoint.GetComponent<TileObject>().tileIndex) return 1;
            //compare within tile
            if (Vector3.Distance(left.transform.position, left.checkpoint.GetComponent<TileObject>().nextTile.transform.position) > Vector3.Distance(right.transform.position, right.checkpoint.GetComponent<TileObject>().nextTile.transform.position)) return 1;
            if (Vector3.Distance(left.transform.position, left.checkpoint.GetComponent<TileObject>().nextTile.transform.position) < Vector3.Distance(right.transform.position, right.checkpoint.GetComponent<TileObject>().nextTile.transform.position)) return -1;
            return 1;
        }
    }

}

