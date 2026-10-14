using UnityEngine;


public abstract class BasePowerup : MonoBehaviour, IPowerup
{
    public PowerupType type;
    public bool spawned = false;
    protected bool consumed = false;
    protected bool goRight = true;
    protected Rigidbody2D rigidBody;
    protected Vector3 startLocalPosition;

    // base methods
    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startLocalPosition = transform.localPosition;
    }

    // interface methods
    // 1. concrete methods
    public PowerupType powerupType
    {
        get // getter
        {
            return type;
        }
    }

    public bool hasSpawned
    {
        get // getter
        {
            return spawned;
        }
    }

    public void DestroyPowerup()
    {
        gameObject.SetActive(false);   // hide instead of destroy, so a restart can bring it back
    }

    // 2. abstract methods, must be implemented by derived classes
    public abstract void SpawnPowerup();
    public abstract void ApplyPowerup(MonoBehaviour i);

    public virtual void ResetPowerup()
    {
        transform.localPosition = startLocalPosition; 
        spawned = false;
        consumed = false;
        goRight = true;
        gameObject.SetActive(true);
    }
}