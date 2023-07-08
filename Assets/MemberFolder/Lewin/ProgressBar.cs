using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image fillImage; // Assign this in the inspector, this should be your second image.
    private float progress = 0.0f;

    // Call this function to update the progress.
    public void SetProgress(float progress)
    {
        this.progress = progress;
        UpdateProgress();
    }

    // This function is called in SetProgress, it updates the fillAmount of the image.
    private void UpdateProgress()
    {
        fillImage.fillAmount = progress;
    }
    
    // Call this function to start animating the progress bar fill up.
    public void AnimateFillUp(float targetProgress, float duration)
    {
        StartCoroutine(FillUpAnimation(targetProgress, duration));
    }

    private IEnumerator FillUpAnimation(float targetProgress, float duration)
    {
        float startTime = Time.time;
        float originalProgress = progress;

        while (Time.time < startTime + duration)
        {
            progress = Mathf.Lerp(originalProgress, targetProgress, (Time.time - startTime) / duration);
            UpdateProgress();
            yield return null;
        }

        progress = targetProgress; // Ensure that the progress bar is fully filled.
        UpdateProgress();
    }
}