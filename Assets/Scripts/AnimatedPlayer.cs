using UnityEngine;

public class AnimatedPlayer : MonoBehaviour
{
    public Sprite[] runSprites;
    public Sprite[] crouchSprites;

    private SpriteRenderer spriteRenderer;
    private int frame;
    private GameObject gameManager;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < transform.parent.transform.childCount; i++)
        {
            if (transform.parent.transform.GetChild(i).CompareTag("Respawn"))
            {
                gameManager = transform.parent.transform.GetChild(i).gameObject;
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
        if (GetComponent<DinoAgent>().downHeld2 == false)
        {
            if (frame >= runSprites.Length)
            {
                frame = 0;
            }

            if (frame >= 0 && frame < runSprites.Length)
            {
                spriteRenderer.sprite = runSprites[frame];
            }
        }

        else
        {
            if (frame >= crouchSprites.Length)
            {
                frame = 0;
            }

            if (frame >= 0 && frame < crouchSprites.Length)
            {
                spriteRenderer.sprite = crouchSprites[frame];
            }
        }

        

        Invoke(nameof(Animate), 1f / gameManager.GetComponent<GameManager>().gameSpeed);
    }
}
