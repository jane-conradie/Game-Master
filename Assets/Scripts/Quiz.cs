using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using System;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions;
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;

    [Header("Buttons")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Progress")]
    [SerializeField] Slider progressBar;

    public bool isComplete;

    AudioSource clickSFX;

    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        clickSFX = FindObjectOfType<AudioSource>();

        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;

        if (timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }

            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            // common practice
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    void GetNextQuestion()
    {
        if (questions.Count > 0)
        {
            SetButtonState(true);
            setDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        }
    }

    void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        currentQuestion = questions[index];

        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
    }

    private void setDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }

    void DisplayQuestion()
    {
        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    public void OnAnswerSelected(int index)
    {
        clickSFX.Play();
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.ResetTimer();
        scoreText.text = "score: " + scoreKeeper.CalculateScore() + "%";
    }

    void DisplayAnswer(int index)
    {
        correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
        Image buttonImage;

        if (index == correctAnswerIndex)
        {
            questionText.text = "Correct";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            questionText.text = "Sorry, the correct answer was:\n" + currentQuestion.GetAnswer(correctAnswerIndex);
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }
}
