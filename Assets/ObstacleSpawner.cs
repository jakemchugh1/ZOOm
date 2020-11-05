using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstacle;

    public int numObstacles;

    public List<GameObject> obstacles;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //deleteObstacles();
            spawnObstacles();
        }
    }

    void spawnObstacles()
    {
        for(int i = 0; i < numObstacles; i++)
        {
            obstacles.Add(Instantiate<GameObject>(obstacle));
        }
    }

    void deleteObstacles()
    {
        for (int i = 0; i < obstacles.Capacity; i++)
        {
            Destroy(obstacles[i]);
        }
        obstacles.Clear();
    }
}
