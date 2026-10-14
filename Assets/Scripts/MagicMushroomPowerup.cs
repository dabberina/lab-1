using System.Collections;
using UnityEngine;

public class MagicMushroomPowerup : BasePowerup
{
    public bool moveAfterSpawn = true;

    private Collider2D col2d;
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        type = PowerupType.MagicMushroom;

        col2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        // powerup bug fix: can't be touched before it spawns
        col2d.enabled = false;
        rigidBody.bodyType = RigidbodyType2D.Static;
    }

    // Animation Event: last frame of the rise clip
    public override void SpawnPowerup()
    {
        spawned = true;
        animator.enabled = false;   // stop the clip from holding the position, like the Goomba issue
        col2d.enabled = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(PushNextFrame());
    }

    IEnumerator PushNextFrame()
    {
        yield return null;   // wait one frame so the Dynamic switch takes effect
        if (moveAfterSpawn)
            rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!spawned) return;

        if (col.gameObject.CompareTag("Player"))
        {
            IPowerupApplicable target = col.gameObject.GetComponent<IPowerupApplicable>();
            if (target != null)
                target.RequestPowerupEffect(this);
            DestroyPowerup();
        }
        else if (col.gameObject.CompareTag("Pipe"))   // tutorial checks layer 10; use your own pipe tag or layer
        {
            goRight = !goRight;
            rigidBody.AddForce(Vector2.right * 3 * (goRight ? 1 : -1), ForceMode2D.Impulse);
        }
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO
    }

    public override void ResetPowerup()
    {
        StopAllCoroutines();
        if (rigidBody.bodyType == RigidbodyType2D.Dynamic)
            rigidBody.linearVelocity = Vector2.zero;
        rigidBody.bodyType = RigidbodyType2D.Static;
        col2d.enabled = false;

        base.ResetPowerup(); // reactivate first: an Animator can't reset while its object is inactive
        rigidBody.position = transform.position;
        Physics2D.SyncTransforms();

        animator.enabled = true;
        animator.Rebind();     // back to the empty default state
        animator.Update(0f);
    }
}