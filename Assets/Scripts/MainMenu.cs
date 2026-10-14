using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public IntVariable gameScore;
    public TextMeshProUGUI highscoreText;

    void Start()
    {
        ShowHighscore();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Loading Page");
    }

    public void ResetHighscore()
    {
        gameScore.ResetHighestValue();
        ShowHighscore();
        EventSystem.current.SetSelectedGameObject(null);
    }

    void ShowHighscore()
    {
        highscoreText.text = "TOP-" + gameScore.previousHighestValue.ToString("D6");
    }
}