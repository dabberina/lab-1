using UnityEngine;
using UnityEngine.Events;

public class GoombaStomp : MonoBehaviour
{
    public Animator goombaAnimator;
    public UnityEvent onStomped;      // wire this to GameManager.IncreaseScore

    public bool IsStomped { get; private set; }

    public void Stomp()
    {
        if (IsStomped) return;
        IsStomped = true;

        goombaAnimator.SetTrigger("stomped");
        GetComponent<Collider2D>().enabled = false;      // can no longer hurt Mario
        GetComponent<EnemyMovement>().enabled = false;   // stop walking
        onStomped.Invoke();
    }

    // Animation Event: last frame of the squash clip
    public void FinishStomp()
    {
        gameObject.SetActive(false); // hide the Goomba
    }

    public void ResetStomp()
    {
        IsStomped = false;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<EnemyMovement>().enabled = true;
        goombaAnimator.ResetTrigger("stomped");
        goombaAnimator.Play("goomba-moving");
    }
}