using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject restartPanel; // UI panel or button that appears when time runs out
    public Button quitButton;
    public Button restartButton;
    public float timeRemaining = 30f;
    public bool isCountdown = true;
    public QTEManager qteManager;

    public bool timerIsRunning = false;

    void Start()
    {
        // Ensure game runs in real-time at start
        Time.timeScale = 1f;
        Player.playerPaused = false;
        restartPanel.SetActive(false);
        quitButton.onClick.AddListener(Quit);
        restartButton.onClick.AddListener(Restart);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Quit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (isCountdown)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime; // So timer works even if timeScale is 0
                    UpdateTimerDisplay(timeRemaining);
                }
                else
                {
                    timeRemaining = 0;
                    timerIsRunning = false;
                    UpdateTimerDisplay(timeRemaining);
                    EndTimer();
                }
            }
        }
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        timeToDisplay = Mathf.Max(0, timeToDisplay);
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void EndTimer()
    {
        restartPanel.SetActive(true); // Show restart UI
        Player.playerPaused = true;
        qteManager.gameObject.SetActive(false);
        //Player.SetGamePaused(true);
        Debug.Log("Time's up! Game paused.");
    }

    public void RestartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Player.SetGamePaused(false);
        restartPanel.SetActive(false);
    }
}