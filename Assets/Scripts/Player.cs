using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody character;
    private BoxCollider hitbox;

    public float gravity = 3.6f;
    public float jumpForce = 16f;
    public float downForce = 10f;
    public bool downHeld { get; private set; }
    public LayerMask mask;

private void Awake()
    {
        character = GetComponent<Rigidbody>();
        hitbox = GetComponent<BoxCollider>();
        downHeld = false;
    }
       
    private void OnEnable()
    {
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, .5f, mask, QueryTriggerInteraction.Ignore))
        {
            character.velocity = new Vector3(0f, 0f, 0f);
            if (Input.GetButton("Jump") && character.velocity.y == 0)
            {
                character.velocity = new Vector3(0f, jumpForce, 0f);
            }
        }

        else
        {
            character.AddForce(new Vector3(0f, -1f, 0f) * gravity);
        }

        if (Input.GetKey("down"))
        {
            downHeld = true;
            hitbox.center = new Vector3(0.0001893044f, -0.1777089f, 0f);
            hitbox.size = new Vector3(1.181729f, 0.6052011f, 0.2f);

            if ((Physics.Raycast(transform.position, Vector3.down, .5f, mask, QueryTriggerInteraction.Ignore)) == false)
            {
                character.velocity = new Vector3(0f, -1 * downForce, 0f);
            }
        }

        else
        {
            downHeld = false;
            hitbox.center = new Vector3(0f, -0.01041643f, 0f);
            hitbox.size = new Vector3(0.9166667f, 0.9791671f, 0.2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }
}
