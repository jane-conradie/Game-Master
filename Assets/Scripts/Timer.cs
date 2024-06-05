using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToCompleteQuestion = 30f;
    [SerializeField] float timeToShowCorrectAnswer = 10f;

    public bool loadNextQuestion;
    public bool isAnsweringQuestion;
    public float fillFraction;

    float timerValue;

    void Update()
    {
        UpdateTimer();
    }

    public void ResetTimer()
    {
        timerValue = 0;
    }

    void UpdateTimer()
    {
        timerValue -= Time.deltaTime;

        if (timerValue <= 0)
        {
            if (isAnsweringQuestion)
            {
                isAnsweringQuestion = false;
                timerValue = timeToShowCorrectAnswer;
            }
            else
            {
                isAnsweringQuestion = true;
                timerValue = timeToCompleteQuestion;
                loadNextQuestion = true;
            }
        }
        else
        {
            if (isAnsweringQuestion)
            {
                // filling in the timer sprite
                fillFraction = timerValue / timeToCompleteQuestion;
            }
            else
            {
                fillFraction = timerValue / timeToShowCorrectAnswer;
            }
        }
    }
}
