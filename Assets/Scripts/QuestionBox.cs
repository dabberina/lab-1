using UnityEngine;

public class QuestionBox : MonoBehaviour
{
    [Header("Animators")]
    public Animator boxAnimator;   // used only for the later "inactive" state swap
    public Animator coinAnimator;

    [Header("Audio")]
    public AudioSource boxAudio;
    public AudioClip coinSound;

    [Header("Optional cleanup")]
    public Collider2D bottomTrigger;

    private bool isUsed = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isUsed) return;

        if (other.CompareTag("Player"))
        {
            isUsed = true;

            coinAnimator.SetTrigger("PopCoin");
            boxAudio.PlayOneShot(coinSound);
        }
    }

    // Animation Event, placed at the LAST frame of the coin-pop clip
    public void DeactivateBox()
    {
        boxAnimator.SetTrigger("Inactive");

        if (bottomTrigger != null)
            bottomTrigger.enabled = false;
    }
}