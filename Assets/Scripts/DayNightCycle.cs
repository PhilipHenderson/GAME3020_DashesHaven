using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;
    public TMP_Text timeText;
    public float secondsInFullDay = 120f; // Adjust the length of a full day/night cycle

    private float currentTime = 0f;
    private bool isPaused = false;

    public PlayerFishController fishController;

    void Update()
    {
        if (fishController == null)
        {
            fishController = FindAnyObjectByType<PlayerFishController>();
        }
        if (!isPaused)
        {
            UpdateTime();
            UpdateSunRotation();
        }
    }

    void UpdateTime()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= secondsInFullDay)
        {
            currentTime = 0f;
        }

        // Calculate the time of day based on the current time
        float normalizedTime = currentTime / secondsInFullDay;
        int hours = Mathf.FloorToInt(24f * normalizedTime);
        int minutes = Mathf.FloorToInt(60f * (24f * normalizedTime - hours));

        // Display the time in the UI
        string timeString = string.Format("{0:00}:{1:00}", hours, minutes);
        timeText.text = "Time: " + timeString;
    }

    void UpdateSunRotation()
    {
        // Rotate the directional light to simulate the sun's movement
        float sunRotationAngle = (currentTime / secondsInFullDay) * 360f;
        sun.transform.rotation = Quaternion.Euler(new Vector3(sunRotationAngle, 90f, 0f));
    }

    public void PauseButton()
    {
        isPaused = true;
        fishController.FreezePlayer();
        Debug.Log("Pause Button Pressed");
    }

    public void PlayButton()
    {
        isPaused = false;
        fishController.UnfreezePlayer();
        float currentTimePercentage = currentTime / secondsInFullDay;

        // Set the new seconds in full day for 2x speed and adjust the current time
        secondsInFullDay = 240.0f;
        currentTime = currentTimePercentage * secondsInFullDay;
        Debug.Log("Play Button Pressed");
    }

    public void FastForwardButton()
    {
        if (isPaused == true) // Ensure the game isn't paused when super-fast-forwarding
        {
            // Calculate the current time percentage in the day
            float currentTimePercentage = currentTime / secondsInFullDay;

            // Set the new seconds in full day for 2x speed and adjust the current time
            secondsInFullDay = 120.0f;
            currentTime = currentTimePercentage * secondsInFullDay;

            Debug.Log("2X Button Pressed");
        }
        else
        {
            Debug.Log("Game Is Paused, Cannot Speed Up Time!");
        }
    }

    public void SuperFastForwardButton()
    {
        if (isPaused == true) // Ensure the game isn't paused when super-fast-forwarding
        {
            // Calculate the current time percentage in the day
            float currentTimePercentage = currentTime / secondsInFullDay;

            // Set the new seconds in full day for 3x speed and adjust the current time
            secondsInFullDay = 60.0f;
            currentTime = currentTimePercentage * secondsInFullDay;

            Debug.Log("3x Button Pressed");
        }
        else
        {
            Debug.Log("Game Is Paused, Cannot Speed Up Time!");
        }
    }

}
