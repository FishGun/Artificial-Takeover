using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public bool isStationaryOnStart;
    public float movementSpeed;
    public float detectionRadius;
    public GameObject eyePos;
    public float wantedDistance;
    public SpriteRenderer sprite;
    public bool otherTurning;

    bool isTriggerd = false;
    bool awaitingMovement;
    GameObject player;
    Vector3 playerPos;
    Rigidbody2D rb;
    Vector2 playerAngle;
    Vector2 velocityToSet;
    int minus1or1;

    
private void Start()
{
    rb = GetComponent<Rigidbody2D>();
    player = GameObject.FindWithTag("Player");
}


private void Update()
{

    if (gameObject.CompareTag("Smol Tank"))
    {
        GetComponentInChildren<Animator>().SetFloat("X Velocity", rb.velocity.x);
    }

    playerPos = player.transform.position;
    playerAngle = playerPos - transform.position;

    if( playerAngle.normalized.x < 0.1f)
    {
        minus1or1 = -1;
    }
    else if (playerAngle.normalized.x >= -0.1f)
    {
        minus1or1 = 1;
    }

    float distance = Vector2.Distance(eyePos.transform.position, playerPos);
    if (distance <= detectionRadius)
    {
        int layerMask = ~LayerMask.GetMask("Enemy","Ignore Raycast");

        RaycastHit2D hit = Physics2D.Raycast(eyePos.transform.position, playerAngle, Mathf.Infinity, layerMask);
        if(hit.collider.gameObject == player)
        {
            if(distance > wantedDistance + 0.1f && !awaitingMovement)
            {
                velocityToSet = new Vector2(minus1or1 * movementSpeed, rb.velocity.y);
                awaitingMovement = true;
            }
            else if (distance < -wantedDistance - 0.1f && !awaitingMovement)
            {
                velocityToSet = new Vector2(minus1or1 * movementSpeed, rb.velocity.y);
                awaitingMovement = true;
            }
            else if (distance < wantedDistance && wantedDistance > 0 && !awaitingMovement)
            {
                velocityToSet = new Vector2(-minus1or1 * movementSpeed, rb.velocity.y);
                awaitingMovement = true;
            }
            else if (distance > -wantedDistance && wantedDistance < 0 && !awaitingMovement)
            {
                velocityToSet = new Vector2(-minus1or1 * movementSpeed, rb.velocity.y);
                awaitingMovement = true;
            }

            if (!otherTurning)
            {
                if (playerAngle.x <= 0f)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }
    if (!awaitingMovement)
    {
        velocityToSet = new Vector2(0, 0);
    }
}

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(velocityToSet.x, rb.velocity.y);
        awaitingMovement = false;
    }
}