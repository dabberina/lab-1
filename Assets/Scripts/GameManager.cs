using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;

    public IntVariable gameScore;

    public static event System.Action OnRestart;

    private int score = 0;

    void Start()
    {
        gameScore.Value = 0;
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        SceneManager.activeSceneChanged += SceneSetup;
    }

    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();   // the new scene's HUD sets itself up
        SetScore(gameScore.Value);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        // reset score
        gameScore.Value = 0;
        SetScore(gameScore.Value);
        SetScore(score);
        gameRestart.Invoke();
        OnRestart?.Invoke();
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
    }

    public void IncreaseScore(int increment)
    {
        gameScore.ApplyChange(increment);   // also updates previousHighestValue
        SetScore(gameScore.Value);
    }

    public void SetScore(int score)
    {
        scoreChange.Invoke(score);
    }


    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }
}