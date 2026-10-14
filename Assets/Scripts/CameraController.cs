using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform endLimit;   // this scene's EndLimit: same scene, so the Inspector reference is fine

    public Transform player;
    private Vector3 startPosition;
    private float offset;
    private float startX;
    private float endX;

    void Start()
    {
        startPosition = transform.position;

        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
        float viewportHalfWidth = Mathf.Abs(bottomLeft.x - transform.position.x);

        offset = transform.position.x - player.position.x;
        startX = transform.position.x;
        endX = endLimit.position.x - viewportHalfWidth;

        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    void OnDestroy()
    {
        if (GameManager.instance != null)
            GameManager.instance.gameRestart.RemoveListener(GameRestart);
    }

    public void GameRestart()
    {
        transform.position = startPosition;
    }

    void Update()
    {
        float desiredX = Mathf.Clamp(player.position.x + offset, startX, endX);
        transform.position = new Vector3(desiredX, transform.position.y, transform.position.z);
    }
}