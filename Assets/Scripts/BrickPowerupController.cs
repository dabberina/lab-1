using UnityEngine;

public class BrickPowerupController : MonoBehaviour, IPowerupController
{
    public Animator powerupAnimator;   // the Coin's Animator
    public BasePowerup powerup;        // the Coin (CoinPowerup)
    public AudioSource boxAudio;
    public AudioClip spawnClip;
    public float bounceTime = 0.5f;    // how long the spring may bounce before freezing

    private bool used = false;
    private Rigidbody2D body;
    private RigidbodyType2D startBodyType;
    private Vector3 startLocalPosition;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        startBodyType = body.bodyType;
        startLocalPosition = transform.localPosition;
    }

    void OnEnable() { GameManager.OnRestart += ResetBox; }
    void OnDisable() { GameManager.OnRestart -= ResetBox; }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (used || !other.gameObject.CompareTag("Player")) return;

        float marioCentre = other.collider.bounds.center.y;
        float blockBottom = GetComponent<Collider2D>().bounds.min.y;
        if (marioCentre >= blockBottom) return;   // only a hit from below

        used = true;
        powerupAnimator.SetTrigger("spawned");    // coin pops
        boxAudio.PlayOneShot(spawnClip);
        Invoke(nameof(Disable), bounceTime);      // freeze after the first bounce
    }

    public void Disable()
    {
        body.bodyType = RigidbodyType2D.Static;
        transform.localPosition = startLocalPosition;
        body.position = transform.position;
    }

    void ResetBox()
    {
        CancelInvoke(nameof(Disable));   // a pending freeze shouldn't hit the fresh block
        used = false;

        body.bodyType = RigidbodyType2D.Static;   // stop it before moving it
        transform.localPosition = startLocalPosition;
        body.position = transform.position;
        body.bodyType = startBodyType;            // spring active again

        powerup.ResetPowerup();
    }
}