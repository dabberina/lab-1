using UnityEngine;

public class SecretBlock : MonoBehaviour
{
    [Header("Animators")]
    public Animator blockAnimator;   // on Block_Brown_coin itself — plays the bump
    public Animator coinAnimator;    // on Coin_hidden — plays the pop-out

    [Header("Audio")]
    public AudioSource blockAudio;
    public AudioClip coinSound;

    [Header("Optional cleanup")]
    public Collider2D bottomTrigger;     // drag BotCollider here
    public GameObject coinObject;        // drag Coin_hidden here, to fully disable later

    private bool isUsed = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isUsed) return;

        if (other.CompareTag("Player"))
        {
            isUsed = true;

            blockAnimator.SetTrigger("Bump");
            coinAnimator.SetTrigger("PopCoin");
            blockAudio.PlayOneShot(coinSound);

        }
    }

    // Animation Event — place at the LAST frame of the coin-pop clip
    public void FinishSecretBlock()
    {
        if (bottomTrigger != null)
            bottomTrigger.enabled = false;

        if (coinObject != null)
            coinObject.SetActive(false); // fully remove the coin once its animation finishes
    }
}