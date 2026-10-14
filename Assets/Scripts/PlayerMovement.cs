using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour, IPowerupApplicable
{
    // other variables
    public Animator marioAnimator;

    // for audio
    public AudioSource marioAudio;
    public AudioSource marioDeathAudio;
    public AudioClip powerupClip;
    public float deathImpulse = 15;

    public float stompBounce = 8f;

    private Vector3 spawnPosition;

    private Collider2D marioCollider;

    void Awake()
    {
        spawnPosition = transform.position;   // where you placed him in this scene
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    void OnDestroy()
    {
        if (GameManager.instance != null)
            GameManager.instance.gameRestart.RemoveListener(GameRestart);
    }

    // state
    [System.NonSerialized]
    public bool alive = true;

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    // other methods

    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    public void RequestPowerupEffect(IPowerup i)
    {
        marioAudio.PlayOneShot(powerupClip);   // goal 4
        i.ApplyPowerup(this);
    }

    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    public float speed = 10;
    private Rigidbody2D marioBody;

    public float upSpeed = 10;
    private bool onGroundState = true;

    void OnCollisionEnter2D(Collision2D col) { CheckLanding(col); }
    void OnCollisionStay2D(Collision2D col) { CheckLanding(col); }
    void CheckLanding(Collision2D col)
    {
        if ((col.gameObject.CompareTag("Ground") ||
             col.gameObject.CompareTag("Obstacles") ||
             col.gameObject.CompareTag("Pipe")) && !onGroundState)
        {
            if (marioBody.linearVelocity.y > 0.01f) return;   // still rising: can't be landing

            float marioFeet = marioCollider.bounds.min.y;
            float surfaceTop = col.collider.bounds.max.y;

            if (marioFeet >= surfaceTop - 0.05f)
            {
                onGroundState = true;
                marioAnimator.SetBool("onGround", onGroundState);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);
        marioCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    public float maxSpeed = 20;

    // FixedUpdate may be called once per frame. See documentation for details.

    private bool moving = false;
    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }

    void Move(int value)
    {

        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.linearVelocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;   // always accept releases, even while paused
            return;
        }

        if (Time.timeScale == 0f) return;   // paused or game over: ignore new presses

        FlipMarioSprite(value);
        moving = true;
        Move(value);
    }

    private bool jumpedState = false;

    public void Jump()
    {
        if (Time.timeScale == 0f) return;   // paused or game over: ignore new presses

        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }

    public void JumpHold()
    {
        if (Time.timeScale == 0f) return;   // paused or game over: ignore new presses

        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            Debug.Log("Collided with goomba!");

            GoombaStomp goomba = other.GetComponent<GoombaStomp>();
            if (goomba != null && goomba.IsStomped) return;

            bool fromAbove = GetComponent<Collider2D>().bounds.min.y > other.bounds.center.y;

            if (goomba != null && fromAbove)
            {
                goomba.Stomp();
                marioBody.linearVelocity = new Vector2(marioBody.linearVelocity.x, 0);
                marioBody.AddForce(Vector2.up * stompBounce, ForceMode2D.Impulse);
            }
            else
            {
                // your existing death code
                Debug.Log("Collided with goomba!");
                marioAnimator.Play("mario-die");
                marioDeathAudio.Play();
                alive = false;
            }
        }
    }

    void GameOverScene()   // still called by the death animation's event
    {
        GameManager.instance.GameOver();
    }

    public void GameRestart()
    {
        // reset position
        marioBody.transform.position = new Vector3(0, 0, 0);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // clean moving stats
        moving = false;
        jumpedState = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;
    }


}

