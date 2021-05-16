using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NPCSpeechBubble : MonoBehaviour
{
    public Canvas canvas;
    private Text speechText;
    public float speed = 4f;
    private Coroutine runSpeech;
    private bool bubbleActive;

    public UnityEvent onSpeechOver;

    private bool bubbleVisible;

    void Awake()
    {
        if (canvas == null)
        {
            canvas = GetComponentInChildren<Canvas>();
        }
        speechText = canvas.GetComponentInChildren<Text>();
    }

    void Start()
    {
        speechText.text = "";
        UpdateBubbleVisibility();
    }

    public void SetBubbleVisibility(bool state)
    {
        bubbleVisible = state;
        UpdateBubbleVisibility();
    }

    private void UpdateBubbleVisibility()
    {
        canvas.gameObject.SetActive(bubbleActive && bubbleVisible);
    }

    public void DoSpeech(string speech)
    {
        StopSpeech();
        runSpeech = StartCoroutine(RunSpeechSlowly(speech, speed));
    }

    private IEnumerator RunSpeechSlowly(string speech, float runSpeed)
    {
        speechText.text = "";
        bubbleActive = true;
        UpdateBubbleVisibility();
        runSpeed = 1f / runSpeed;
        for (int i = 0; i < speech.Length; i++)
        {
            speechText.text = string.Concat(speechText.text, speech[i]);
            yield return Yielders.Get(runSpeed);
        }
        yield return Yielders.Get(1f);
        onSpeechOver?.Invoke();
        StopSpeech();
    }

    public void StopSpeech()
    {
        if (runSpeech != null)
        {
            StopCoroutine(runSpeech);
            runSpeech = null;
        }
        speechText.text = "";
        bubbleActive = false;
        UpdateBubbleVisibility();
    }
}
