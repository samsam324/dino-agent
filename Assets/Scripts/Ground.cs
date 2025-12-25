using UnityEngine;

public class Ground : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private GameObject gameManager;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        for (int i = 0; i < transform.parent.transform.childCount; i++)
        {
            if (transform.parent.transform.GetChild(i).CompareTag("Respawn"))
            {
                gameManager = transform.parent.transform.GetChild(i).gameObject;
                break;
            }
        }
    }

    private void Update()
    {
        float speed = gameManager.GetComponent<GameManager>().gameSpeed / transform.localScale.x;
        meshRenderer.material.mainTextureOffset += Vector2.right * speed * Time.deltaTime;
    }
}
