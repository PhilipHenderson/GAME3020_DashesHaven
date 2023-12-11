using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class FlashingTxt : MonoBehaviour
{
    public float fadeDuration = 1.0f;
    private TextMeshProUGUI textComponent;

    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0.0f);
        StartCoroutine(FadeText());
    }

    private IEnumerator FadeText()
    {
        Color startColor = textComponent.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); 

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            textComponent.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        textComponent.color = targetColor;

        yield return new WaitForSeconds(1.0f);


        startColor = targetColor;
        targetColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f);

        elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            textComponent.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }


        textComponent.color = targetColor;


        StartCoroutine(FadeText());
    }
}