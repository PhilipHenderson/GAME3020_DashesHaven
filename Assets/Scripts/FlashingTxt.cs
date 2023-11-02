using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class FlashingTxt : MonoBehaviour
{
    public float fadeDuration = 1.0f; // Adjust as needed.
    private TextMeshProUGUI textComponent;

    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0.0f); // Start with text fully transparent
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        Color startColor = textComponent.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // Fades in to full opacity.

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            textComponent.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the text is fully faded in.
        textComponent.color = targetColor;

        // Wait for a moment (you can adjust the duration as needed).
        yield return new WaitForSeconds(1.0f);

        // Reverse the fading effect by swapping the start and target colors.
        startColor = targetColor;
        targetColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f); // Fades back out.

        elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            textComponent.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the text is fully faded out.
        textComponent.color = targetColor;

        // Restart the fading process.
        StartCoroutine(FadeText());
    }
}