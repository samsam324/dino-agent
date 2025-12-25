using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
    }

    private GameObject gameManager;

    public SpawnableObject[] objects;

    public float minSpawnRate = 0.7f;
    public float maxSpawnRate = 1.5f;

    [SerializeField] private GameObject gameField;
    [SerializeField] private GameObject objectPool;

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
        float spawnChance = Random.value;

        foreach (var obj in objects)
        {
            if (spawnChance < obj.spawnChance)
            {
                GameObject obstacle = null;

                if (obj.prefab.name != "Bird")
                {
                    obstacle = objectPool.GetComponent<ObjectPool>().GetPooledObstacle();

                    if (obstacle != null)
                    {
                        obstacle.transform.localPosition = Vector3.zero;
                        obstacle.transform.localPosition += transform.localPosition;
                        obstacle.GetComponent<SpriteRenderer>().sprite = obj.prefab.GetComponent<SpriteRenderer>().sprite;
                        obstacle.GetComponent<BoxCollider>().center = obj.prefab.GetComponent<BoxCollider>().center;
                        obstacle.GetComponent<BoxCollider>().size = obj.prefab.GetComponent<BoxCollider>().size;
                        obstacle.SetActive(true);
                    }
                }

                else
                {
                    obstacle = objectPool.GetComponent<ObjectPool>().GetPooledBird();

                    if (obstacle != null)
                    {
                        obstacle.transform.localPosition = Vector3.zero;
                        obstacle.transform.localPosition += transform.localPosition;
                        obstacle.transform.position += Vector3.up * Random.value;
                        obstacle.SetActive(true);
                    }
                }

                break;
            }

            spawnChance -= obj.spawnChance;
        }

        float time = Random.Range(minSpawnRate, maxSpawnRate);
        float diff = gameManager.GetComponent<GameManager>().gameSpeed - 7f;

        Invoke(nameof(Spawn), time - ((diff * time) / (gameManager.GetComponent<GameManager>().gameSpeed + diff)));
    }
}
