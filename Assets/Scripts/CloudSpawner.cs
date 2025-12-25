using UnityEditor;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    private GameObject gameManager;

    public float minSpawnRate = 0.1f;
    public float maxSpawnRate = 3f;

    public GameObject cloud;

    [SerializeField] private GameObject gameField;
    [SerializeField] private GameObject objectPool;

    Vector3 scaleTemp;

    private void OnEnable()
    {
        for (int i = 0; i < transform.parent.transform.childCount; i++)
        {
            if (transform.parent.transform.GetChild(i).CompareTag("Respawn"))
            {
                gameManager = transform.parent.transform.GetChild(i).gameObject;
                break;
            }
        }

        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Spawn()
    {
        GameObject obstacle = objectPool.GetComponent<ObjectPool>().GetPooledCloud();

        if (obstacle != null)
        {
            scaleTemp = (Vector3.right * Random.Range(1f, 2f)) + (Vector3.up) + (Vector3.forward);
            scaleTemp.y = scaleTemp.x;
            obstacle.transform.localPosition = Vector3.zero;
            obstacle.transform.localPosition += transform.localPosition + (Vector3.up * Random.Range(2f, 4f));
            if (Random.value > 0.5f)
            {
                scaleTemp.x *= -1f;
            }

            obstacle.transform.localScale = scaleTemp;
            obstacle.SetActive(true);
        }
        float time = Random.Range(minSpawnRate, maxSpawnRate);
        float diff = gameManager.GetComponent<GameManager>().gameSpeed - 7f;

        Invoke(nameof(Spawn), (time - ((diff * time) / (gameManager.GetComponent<GameManager>().gameSpeed + diff))));
    }
}
