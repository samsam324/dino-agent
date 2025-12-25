using Unity.MLAgents.Sensors;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Collections.Generic;

public class DinoAgent : Agent
{
    private Rigidbody character;
    private BoxCollider hitbox;

    private float gravity = 9.81f * 3f;
    private float jumpForce = 10f;
    private float downForce = 17f;
    public bool downHeld { get; private set; }
    public bool downHeld2 { get; private set; }
    [SerializeField]
    private LayerMask mask;

    private bool jump;

    GameObject obstacle;
    List<GameObject> tempObstacles = new List<GameObject>();

    [SerializeField] private GameObject objectPool;

    private GameObject gameManager;

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        //Jump
        if (Input.GetButton("Jump"))
        {
            discreteActions[0] = 0;
        }
        //Crouch or down
        else if (Input.GetKey("down"))
        {
            discreteActions[0] = 1;
        }
        //Neither
        else
        {
            discreteActions[0] = 2;
        }
    }

    private void Start()
    {
        for (int i = 0; i < transform.parent.transform.childCount; i++)
        {
            if (transform.parent.transform.GetChild(i).CompareTag("Respawn"))
            {
                gameManager = transform.parent.transform.GetChild(i).gameObject;
                break;
            }
        }

        character = GetComponent<Rigidbody>();
        hitbox = GetComponent<BoxCollider>();
        downHeld = false;
        jump = false;
    }

    public override void OnEpisodeBegin()
    {
        obstacle = null;

        for (int i = 0; i < objectPool.transform.childCount; i++)
        {
            if (objectPool.transform.GetChild(i).CompareTag("Obstacle"))
            {
                objectPool.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < transform.parent.transform.childCount; i++)
        {
            if (transform.parent.transform.GetChild(i).CompareTag("Respawn"))
            {
                transform.parent.transform.GetChild(i).GetComponent<GameManager>().NewGame();
                break;
            }
        }

        character.angularVelocity = Vector3.zero;
        character.velocity = Vector3.zero;
        transform.localPosition = new Vector3(-6f, -0.1000001f, 0f);
        jump = false;
        downHeld = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        tempObstacles.Clear();
        for (int i = 0; i < objectPool.transform.childCount; i++)
        {
            if (objectPool.transform.GetChild(i).gameObject.CompareTag("Obstacle") && objectPool.transform.GetChild(i).gameObject.activeInHierarchy && objectPool.transform.GetChild(i).transform.position.x > transform.position.x - 1.3f)
            {
                tempObstacles.Add(objectPool.transform.GetChild(i).gameObject);
            }
        }

        if (tempObstacles.Count == 1)
        {
            obstacle = tempObstacles[0];
        }
        else if (tempObstacles.Count != 0)
        {
            obstacle = tempObstacles[0];
            for (int i = 1; i < tempObstacles.Count; i++)
            {
                if (obstacle.transform.position.x > tempObstacles[i].transform.position.x)
                {
                    obstacle = tempObstacles[i];
                }
            }
        }

        //Dino Position
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.y);

        //Dino Velocity
        sensor.AddObservation(character.velocity.y);

        //Speed of the game
        sensor.AddObservation(gameManager.GetComponent<GameManager>().gameSpeed);

        //Pass in positions of targets on screen
        if (obstacle != null)
        {
            sensor.AddObservation(obstacle.transform.localPosition.x);
            sensor.AddObservation(obstacle.transform.localPosition.y);

            if (obstacle.name == "Bird(Clone)")
            {
                sensor.AddObservation(0);
            }

            else if (obstacle.name == "Cactus_Large_Doube(Clone)")
            {
                sensor.AddObservation(1);
            }

            else if (obstacle.name == "Cactus_Large_Single(Clone)")
            {
                sensor.AddObservation(2);
            }

            else if (obstacle.name == "Cactus_Large_Triple(Clone)")
            {
                sensor.AddObservation(3);
            }

            else if (obstacle.name == "Cactus_Small_Doube(Clone)")
            {
                sensor.AddObservation(4);
            }

            else if (obstacle.name == "Cactus_Small_Single(Clone)")
            {
                sensor.AddObservation(5);
            }

            else
            {
                sensor.AddObservation(6);
            }
        }
        else 
        {
            sensor.AddObservation(+10000);
            sensor.AddObservation(+10000);
            sensor.AddObservation(+10000);
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var discreteActions = actionBuffers.DiscreteActions;
        Vector3 controlSignal = Vector3.zero;
        
        //Jumping
        if (discreteActions[0] == 0)
        {
            jump = true;
            downHeld = false;
        }

        //Going down or crouching
        else if (discreteActions[0] == 1)
        {
            downHeld = true;
            jump = false;
            downHeld2 = true;
        }

        else
        {
            jump = false;
            downHeld = false;
        }
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, Vector3.down, .5f, mask, QueryTriggerInteraction.Ignore))
        {
            character.velocity = new Vector3(0f, 0f, 0f);
        }

        else
        {
            character.AddForce(new Vector3(0f, -1f, 0f) * gravity);
        }

        if (jump)
        {
            downHeld2 = false;
            hitbox.center = new Vector3(0f, -0.01041643f, 0f);
            hitbox.size = new Vector3(0.9166667f, 0.9791671f, 0.2f);

            if (Physics.Raycast(transform.position, Vector3.down, .5f, mask, QueryTriggerInteraction.Ignore))
            {
                character.velocity = new Vector3(0f, jumpForce, 0f);
            }

            jump = false; 
        }

        else if (downHeld)
        {
            downHeld2 = true;
            hitbox.center = new Vector3(0.0001893044f, -0.1777089f, 0f);
            hitbox.size = new Vector3(1.181729f, 0.6052011f, 0.2f);

            if (!(Physics.Raycast(transform.position, Vector3.down, .5f, mask, QueryTriggerInteraction.Ignore)))
            {
                character.velocity = new Vector3(0f, -1 * downForce, 0f);
            }

            downHeld = false;
        }

        else
        {
            downHeld2 = false;
            hitbox.center = new Vector3(0f, -0.01041643f, 0f);
            hitbox.size = new Vector3(0.9166667f, 0.9791671f, 0.2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            EndEpisode();
        }

        else if (other.CompareTag("ScoreBarrier"))
        {
            AddReward(1.0f);
        }
    }
}
