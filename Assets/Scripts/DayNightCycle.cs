using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    public Light sun;
    public TMP_Text timeText;
    public float secondsInFullDay = 120f; // length of a full day/night cycle

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

        float normalizedTime = currentTime / secondsInFullDay;
        int hours = Mathf.FloorToInt(24f * normalizedTime);
        int minutes = Mathf.FloorToInt(60f * (24f * normalizedTime - hours));

        string timeString = string.Format("{0:00}:{1:00}", hours, minutes);
        timeText.text = "Time: " + timeString;
    }

    void UpdateSunRotation()
    {
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

        secondsInFullDay = 240.0f;
        currentTime = currentTimePercentage * secondsInFullDay;
        Debug.Log("Play Button Pressed");
    }

    public void FastForwardButton()
    {
        if (isPaused == true)
        {
            float currentTimePercentage = currentTime / secondsInFullDay;

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
        if (isPaused == true)
        {
            float currentTimePercentage = currentTime / secondsInFullDay;

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
