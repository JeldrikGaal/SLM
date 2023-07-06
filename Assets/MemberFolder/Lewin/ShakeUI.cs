using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShakeUI : MonoBehaviour
{
    // The duration of the shaking effect
    public float shakeDuration = 0.5f;

    // The magnitude of the shaking effect
    public float shakeMagnitude = 0.7f;

    // Original position of the UI element
    private Vector3 originalPos;
    private bool shake;
    void Awake()
    {
        originalPos = transform.localPosition;
    }

    private void Update()
    {
        //Debug.Log("s: " + shake);
    }

    public void StartShake()
    {
        shake = true;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
