using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    void Awake()
    {
        GameManager.instance.gameStart.AddListener(GameStart);
        GameManager.instance.gameRestart.AddListener(GameStart);
        GameManager.instance.scoreChange.AddListener(SetScore);
        GameManager.instance.gameOver.AddListener(GameOver);
    }

    void OnDestroy()
    {
        if (GameManager.instance == null) return;
        GameManager.instance.gameStart.RemoveListener(GameStart);
        GameManager.instance.gameRestart.RemoveListener(GameStart);
        GameManager.instance.scoreChange.RemoveListener(SetScore);
        GameManager.instance.gameOver.RemoveListener(GameOver);
    }

    private Vector3[] scoreTextPosition = {
        new Vector3(-747, 473, 0),
        new Vector3(0, 0, 0)
        };
    private Vector3[] restartButtonPosition = {
        new Vector3(844, 455, 0),
        new Vector3(0, -150, 0)
    };

    public GameObject scoreText;
    public Transform restartButton;

    public GameObject gameOverPanel;
    public GameObject hudScore;
    public GameObject hudRestartButton;
    public GameObject gameOverScoreText;
    public GameObject pauseButton;
    public GameObject highscoreText;
    public IntVariable gameScore;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart()
    {
        // hide gameover panel
        gameOverPanel.SetActive(false);
        hudScore.SetActive(true);
        hudRestartButton.SetActive(true);
        pauseButton.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[0];
        restartButton.localPosition = restartButtonPosition[0];
    }

    public void SetScore(int score)
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }


    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        hudScore.SetActive(false);
        hudRestartButton.SetActive(false);
        pauseButton.SetActive(false);
        gameOverScoreText.GetComponent<TextMeshProUGUI>().text = scoreText.GetComponent<TextMeshProUGUI>().text;
        scoreText.transform.localPosition = scoreTextPosition[1];
        restartButton.localPosition = restartButtonPosition[1];
        // set highscore
        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP-" + gameScore.previousHighestValue.ToString("D6");
    }
}
