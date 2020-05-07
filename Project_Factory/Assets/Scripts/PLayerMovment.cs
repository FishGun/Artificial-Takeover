using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PLayerMovment : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;

    private float movement;
    bool shouldMove;
    private float xForceToAdd;
    public Animator animator;

    public float walkDelay;
    float timeVar;
    public AudioClip Outside;
    public AudioClip Interior1;
    public AudioClip Interior2;
    public AudioSource walking;
    //bool shouldPlayWalk = true;

    Rigidbody2D player;
    
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        movement = Input.GetAxis("Horizontal");
        if (Input.GetKey("d"))
        {
            if (player.velocity.x < 0)
                SetVelocityX(player.velocity.x / 1.2f);
            xForceToAdd += speed;
            animator.SetBool("IsRunning", true);
        }

        if (Input.GetKey("a"))
        {
            if (player.velocity.x > 0)
                SetVelocityX(player.velocity.x / 1.2f);
            xForceToAdd += -speed;
            animator.SetBool("IsRunning", true);
        }

        if ((Input.GetKey("a") && Input.GetKey("d")) || (!Input.GetKey("a") && !Input.GetKey("d")))
        {
            SetVelocityX(player.velocity.x / 1.2f);
            animator.SetBool("IsRunning", false);
        }

        if (SceneManager.GetActiveScene().name == "Outside 1" || SceneManager.GetActiveScene().name == "Outside 1 (Return)")
        {
            walking.clip = Outside;
        }
        else if(SceneManager.GetActiveScene().name == "Interior 1" || SceneManager.GetActiveScene().name == "Interior 1 (Return)")
        {
            walking.clip = Interior1;
        }
        else if (SceneManager.GetActiveScene().name == "Interior 2")
        {
            walking.clip = Interior2;
        }

        if (Input.GetKey("a") || Input.GetKey("d"))
        {
            if (timeVar < Time.time && FindObjectOfType<JumpController>().onGround)
            {
                walking.Play();
                timeVar = Time.time + walkDelay;
            }
        }

        if (player.velocity.x > maxSpeed)
        {
            SetVelocityX(maxSpeed);
        }
        else if (player.velocity.x < -maxSpeed)
        {
            SetVelocityX(-maxSpeed);
        }

        if (Input.mousePosition.x >= Camera.main.pixelWidth/2)
        {
            animator.SetBool("FacingRight", true);
        }
        else
        {
            animator.SetBool("FacingRight", false);
        }
    }

    void FixedUpdate()
    {
        player.AddForce(new Vector2(xForceToAdd, 0));
        xForceToAdd = 0;
    }

    private void SetVelocityX(float i)
    {
        player.velocity = new Vector2(i, player.velocity.y);
    }

}

