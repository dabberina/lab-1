using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoxPowerupController : MonoBehaviour, IPowerupController
{
    public Animator powerupAnimator;
    public BasePowerup powerup; // reference to this question box's powerup
    private RigidbodyType2D startBodyType;

    public AudioSource boxAudio;   // root's AudioSource
    public AudioClip spawnClip;   // was riseClip: coin sound or mushroom rise sound
    private bool used = false;


    void Start()
    {
        startBodyType = GetComponent<Rigidbody2D>().bodyType;

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (used || !other.gameObject.CompareTag("Player")) return;

        float marioCentre = other.collider.bounds.center.y;
        float boxBottom = GetComponent<Collider2D>().bounds.min.y;
        if (marioCentre >= boxBottom) return;

        used = true;

        GetComponent<Animator>().SetTrigger("spawned");
        powerupAnimator.SetTrigger("spawned");
        boxAudio.PlayOneShot(spawnClip);
    }

    // used by animator
    public void Disable()
    {
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        transform.localPosition = new Vector3(0, 0, 0);
    }

    void OnEnable() { GameManager.OnRestart += ResetBox; }
    void OnDisable() { GameManager.OnRestart -= ResetBox; }

    void ResetBox()
    {
        used = false;

        GetComponent<Rigidbody2D>().bodyType = startBodyType;   // turn the spring back on
        transform.localPosition = Vector3.zero;

        Animator anim = GetComponent<Animator>();
        anim.Rebind();          // back to the normal "?" state
        anim.Update(0f);

        powerup.ResetPowerup();

    }
}