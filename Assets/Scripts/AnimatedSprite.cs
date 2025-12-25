using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;
    private int frame;
    private GameObject gameManager;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < transform.parent.transform.parent.transform.childCount; i++)
        {
            if (transform.parent.transform.parent.transform.GetChild(i).CompareTag("Respawn"))
            {
                gameManager = transform.parent.transform.parent.transform.GetChild(i).gameObject;
                break;
            }
        }
    }

    private void OnEnable()
    {
        Invoke(nameof(Animate), 0f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Animate()
    {
        frame++;

        if (frame >= sprites.Length)
        {
            frame = 0;
        }

        if (frame >= 0 && frame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[frame];
        }

        Invoke(nameof(Animate), 1f / gameManager.GetComponent<GameManager>().gameSpeed);
    }
}
