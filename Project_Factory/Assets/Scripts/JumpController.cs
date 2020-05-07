using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    [SerializeField] float jumpForce;
    [SerializeField] float fallFaster;
    [SerializeField] float lowJump;
    public GameObject jumpChecker;

    public Animator animator;
    Rigidbody2D player;
    [System.NonSerialized] public bool canJump;
    [System.NonSerialized] public bool onGround;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        animator = GameObject.Find("Player").GetComponent<Animator>();
    }

    void Update()
    {

        if (canJump)
        {
            animator.SetBool("OnGround", true);
            onGround = true;
        }
        else
        {
            animator.SetBool("OnGround", false);
            onGround = false;
        }

        animator.SetFloat("Y velocity", player.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            player.velocity = new Vector2(player.velocity.x,(jumpForce / 2));
            canJump = false;
        }

        if (player.velocity.y < 0)
        {
            player.velocity += Vector2.up * Physics2D.gravity.y * (fallFaster - 1) * Time.deltaTime;
        }
        else if (player.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            player.velocity += Vector2.up * Physics2D.gravity.y * (lowJump - 1) * Time.deltaTime;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {

        for (int i = 0; i < collision.contacts.Length; i++)
        {
            if (collision.GetContact(i).point.y < jumpChecker.transform.position.y && !collision.collider.isTrigger)
            {
                canJump = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        canJump = false;
    }
}
