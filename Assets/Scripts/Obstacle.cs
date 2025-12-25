using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float leftEdge;
    private GameObject gameManager;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 2f;

        for (int i = 0; i < transform.parent.transform.parent.transform.childCount; i++)
        {
            if (transform.parent.transform.parent.transform.GetChild(i).CompareTag("Respawn"))
            {
                gameManager = transform.parent.transform.parent.transform.GetChild(i).gameObject;
                break;
            }
        }
    }
    private void Update()
    {
        transform.localPosition += Vector3.left * gameManager.GetComponent<GameManager>().gameSpeed * Time.deltaTime;

        if (transform.localPosition.x < leftEdge)
        {
            gameObject.SetActive(false);
        }
    }
}
