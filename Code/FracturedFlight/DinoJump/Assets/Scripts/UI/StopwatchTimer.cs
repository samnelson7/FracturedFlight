using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StopwatchTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Reference to the UI Text component
    public float currentTime = 0f; // The current time
    public bool isRunning = false; // Flag to indicate if the timer is running

    void Start()
    {
        FormatTime();
    }
    void Update()
    {
        if (isRunning)
        {
            currentTime += Time.deltaTime; // Increment the time

            if (currentTime >= 3599f)
            {
                currentTime = 3599f;
                isRunning = false; // Stop the timer
            }

            FormatTime(); // Update the UI with the formatted time
        }
    }
    public void FormatTime()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int milliseconds = Mathf.FloorToInt((currentTime * 100) % 100);

        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    // Function to start the timer
    public void StartTimer()
    {
        isRunning = true;
    }

    // Function to stop the timer
    public void StopTimer()
    {
        isRunning = false;
    }

    // Function to reset the timer
    public void ResetTimer()
    {
        currentTime = 0f;
        FormatTime();
    }
    public void SaveFastestCompletionTime()
    {
        string currentLevelName = SceneManager.GetActiveScene().name;
        char currentLevelNumber = currentLevelName[currentLevelName.Length - 1];
        string anyPercentKey = "Level_" + currentLevelNumber + "_Any%"; // e.g., "Level_1_Any%"
        string allEggsKey = "Level_" + currentLevelNumber + "_All"; // e.g., "Level_1_All"

        float newTime = currentTime;
        float bestAnyPercentTime = PlayerPrefs.GetFloat(anyPercentKey, float.MaxValue);
        float bestAllEggsTime = PlayerPrefs.GetFloat(allEggsKey, float.MaxValue);

        if (newTime < bestAnyPercentTime)
        {
            PlayerPrefs.SetFloat(anyPercentKey, newTime);
            
        }
        if (newTime < bestAllEggsTime && UIManager.instance.getCollectablesFound() == "111")
        {
            PlayerPrefs.SetFloat(allEggsKey, newTime);
        }
        PlayerPrefs.Save();
    }
}
