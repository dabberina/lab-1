using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource music;   // drag in the background music AudioSource

    void Awake()
    {
        GameManager.instance.gameRestart.AddListener(RestartMusic);
        GameManager.instance.gameOver.AddListener(StopMusic);
    }

    void OnDestroy()
    {
        if (GameManager.instance == null) return;
        GameManager.instance.gameRestart.RemoveListener(RestartMusic);
        GameManager.instance.gameOver.RemoveListener(StopMusic);
    }

    void RestartMusic()
    {
        music.Stop();
        music.Play();   // starts again from the beginning
    }

    void StopMusic()
    {
        music.Stop();
    }
}
