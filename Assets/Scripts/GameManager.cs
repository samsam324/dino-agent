using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initialGameSpeed = 7f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }

    private GameObject player;
    private GameObject spawner;

    private void Start()
    {
        for (int i = 0; i < transform.parent.transform.childCount; i++)
        {
            if (transform.parent.transform.GetChild(i).CompareTag("Player"))
            {
                player = transform.parent.transform.GetChild(i).gameObject;
                break;
            }
        }

        for (int i = 0; i < transform.parent.transform.childCount; i++)
        {
            if (transform.parent.transform.GetChild(i).CompareTag("Spawner"))
            {
                spawner = transform.parent.transform.GetChild(i).gameObject;
                break;
            }
        }

        NewGame();
    }

    public void NewGame()
    {
        gameSpeed = initialGameSpeed;
        enabled = true;
    }

    private void Update()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
    }
    
    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);

    }
}
