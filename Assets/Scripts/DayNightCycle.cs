using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DayNightCycle : MonoBehaviour
{
    public Light sun;
    public TMP_Text timeText;
    public float secondsInFullDay = 120f; // Adjust the length of a full day/night cycle

    private float currentTime = 0f;

    void Update()
    {
        UpdateTime();
        UpdateSunRotation();
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
        sun.transform.rotation = Quaternion.Euler(new Vector3(sunRotationAngle, 0f, 0f));
    }
}
