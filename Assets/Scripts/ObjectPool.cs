using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject bird;
    [SerializeField] private GameObject cloud;

    private List<GameObject> pooledObstacles = new List<GameObject>();
    private List<GameObject> pooledBirds = new List<GameObject>();
    private List<GameObject> pooledClouds = new List<GameObject>();

    public void Awake()
    {
        //Obstacles
        for (int i =0; i < 5; i++)
        {
            GameObject obj = Instantiate(obstacle, transform);
            obj.SetActive(false);
            pooledObstacles.Add(obj);
        }
        //Birds
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = Instantiate(bird, transform);
            obj.SetActive(false);
            pooledBirds.Add(obj);
        }
        //Clouds
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(cloud, transform);
            obj.SetActive(false);
            pooledClouds.Add(obj);
        }
    }

    //Obstacles
    public GameObject GetPooledObstacle()
    {
        for (int i = 0; i < pooledObstacles.Count; i++)
        {
            if (!pooledObstacles[i].activeInHierarchy)
            {
                return pooledObstacles[i];
            }
        }

        return null;
    }

    //Birds
    public GameObject GetPooledBird()
    {
        for (int i = 0; i < pooledBirds.Count; i++)
        {
            if (!pooledBirds[i].activeInHierarchy)
            {
                return pooledBirds[i];
            }
        }

        return null;
    }

    //Clouds
    public GameObject GetPooledCloud()
    {
        for (int i = 0; i < pooledClouds.Count; i++)
        {
            if (!pooledClouds[i].activeInHierarchy)
            {
                return pooledClouds[i];
            }
        }

        return null;
    }
}
