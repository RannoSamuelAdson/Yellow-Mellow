using System.Collections;
using TMPro;
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

    public void StartAnimatingText(float timeAdd)
    {
        timeRemaining += timeAdd; // Add item's value to the timer
        text.text = "+" + timeAdd.ToString("F0") + "s"; // Display added time
        StartCoroutine(AnimateText(timeAdd));
    }

        public TextMeshProUGUI text; // Assign in inspector
        public float growDuration = 1f;
        public float maxScale = 1.5f;
        public float fadeDuration = 2f;
        public float delayBeforeFade = 1f;

        IEnumerator AnimateText(float timeAdded)
        {
            // Reset scale and alpha
            text.transform.localScale = Vector3.one;
            Color startColor = text.color;
            startColor.a = 1;
            text.color = startColor;

            // Grow the text
            float elapsedGrow = 0f;
            while (elapsedGrow < growDuration)
            {
                float t = elapsedGrow / growDuration;
                float scale = Mathf.Lerp(1f, maxScale, t);
                text.transform.localScale = Vector3.one * scale;

                elapsedGrow += Time.deltaTime;
                yield return null;
            }

            text.transform.localScale = Vector3.one * maxScale;

            // Wait before fading
            yield return new WaitForSeconds(delayBeforeFade);

            // Fade out
            float elapsedFade = 0f;
            Color originalColor = text.color;
            while (elapsedFade < fadeDuration)
            {
                float t = elapsedFade / fadeDuration;
                Color fadedColor = originalColor;
                fadedColor.a = Mathf.Lerp(1f, 0f, t);
                text.color = fadedColor;

                elapsedFade += Time.deltaTime;
                yield return null;
            }

            // Ensure fully transparent
            Color finalColor = text.color;
            finalColor.a = 0f;
            text.color = finalColor;
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
            if (isCountdown && !Player.playerPaused)
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