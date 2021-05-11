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

    public UnityEvent onSpeechOver;

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
        canvas.gameObject.SetActive(false);
        speechText.text = "";
    }

    public void DoSpeech(string speech)
    {
        runSpeech = StartCoroutine(RunSpeechSlowly(speech, speed));
    }

    private IEnumerator RunSpeechSlowly(string speech, float runSpeed)
    {
        speechText.text = "";
        canvas.gameObject.SetActive(true);

        runSpeed = 1f / runSpeed;
        for (int i = 0; i < speech.Length; i++)
        {
            speechText.text = string.Concat(speechText.text, speech[i]);
            yield return new WaitForSeconds(runSpeed);
        }
        onSpeechOver?.Invoke();

        yield return new WaitForSeconds(2f);
        canvas.gameObject.SetActive(false);
        runSpeech = null;
    }
}
