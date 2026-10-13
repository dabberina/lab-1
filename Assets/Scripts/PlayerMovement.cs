using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // other variables
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    public GameObject gameOverPanel;
    public GameObject hudScore;
    public GameObject hudRestartButton;
    public TextMeshProUGUI gameOverScoreText;
    // for animation
    public Animator marioAnimator;
    public Transform gameCamera;

    // for audio
    public AudioSource marioAudio;
    public AudioSource marioDeathAudio;
    public float deathImpulse = 15;

    public float stompBounce = 8f;

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

    void GameOverScene()
    {
        // stop time
        Time.timeScale = 0.0f;
        gameOverPanel.SetActive(true);
        hudScore.SetActive(false);
        hudRestartButton.SetActive(false);
        gameOverScoreText.text = scoreText.text; // replace this with whichever way you triggered the game over screen for Checkoff 1
    }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        gameOverPanel.SetActive(false);
        hudScore.SetActive(true);
        hudRestartButton.SetActive(true);
        // resume time
        Time.timeScale = 1.0f;
    }

    public JumpOverGoomba jumpOverGoomba;
    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.localPosition = eachChild.GetComponent<EnemyMovement>().startPosition;
        }
        // reset score
        jumpOverGoomba.score = 0;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(5, (float)5.5, -10);

    }
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    public float speed = 10;
    private Rigidbody2D marioBody;

    public float upSpeed = 10;
    private bool onGroundState = true;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") && !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
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
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    private bool jumpedState = false;

    public void Jump()
    {
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
                alive = false;
            }
        }
    }

    public void GameRestart()
    {
        // reset position
        marioBody.transform.position = new Vector3(0, 0, 0);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(5, 5.5f, -10);
    }


}

