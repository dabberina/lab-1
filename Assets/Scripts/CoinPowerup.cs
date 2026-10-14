using UnityEngine;

public class CoinPowerup : BasePowerup
{
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        type = PowerupType.Coin;
        animator = GetComponent<Animator>();
    }

    // Animation Event: first frame of the coin pop clip
    public override void SpawnPowerup()
    {
        spawned = true;
        GameManager.instance.IncreaseScore(1);   // a box coin is collected the moment it pops out
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        // nothing to apply: Mario never touches a box coin
    }

    public override void ResetPowerup()
    {
        base.ResetPowerup();   // reactivates it, clears spawned, restores its position
        animator.Rebind();
        animator.Update(0f);
    }
}