using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;

public class Quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO question;
    [SerializeField] GameObject[] answerButtons;

    int correctAnswerIndex;
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    void Start()
    {
        questionText.text = question.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = question.GetAnswer(i);
        }
    }

    public void OnAnswerSelected(int index)
    {
        correctAnswerIndex = question.GetCorrectAnswerIndex();
        Image buttonImage = null;

        if (index == correctAnswerIndex)
        {
            questionText.text = "Correct";
            buttonImage = GetButtonImage(buttonImage, index);
            SetButtonSprite(buttonImage);
        }
        else
        {
            questionText.text = "Sorry, the correct answer was:\n" + question.GetAnswer(correctAnswerIndex);
            buttonImage = GetButtonImage(buttonImage, correctAnswerIndex);
            SetButtonSprite(buttonImage);
        }
    }

    public Image GetButtonImage(Image buttonImage, int index)
    {
        buttonImage = answerButtons[index].GetComponent<Image>();

        return buttonImage;
    }

    public void SetButtonSprite(Image buttonImage)
    {
        buttonImage.sprite = correctAnswerSprite;
    }
}
