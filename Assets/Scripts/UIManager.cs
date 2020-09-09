using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UIManagerParameters
{
    [Header("AnswersOptions")]
    [SerializeField] float margins;
    public float Margins { get { return margins; } }
}

[System.Serializable]
public struct UIElements
{
    [SerializeField] RectTransform answerContentArea;
    public RectTransform AnswerContentArea { get { return answerContentArea; } }


    [SerializeField] TextMeshProUGUI questionInfoTextObject;
    public TextMeshProUGUI QuestionInfoTextObject { get { return questionInfoTextObject; } }


    [SerializeField] TextMeshProUGUI scoreText;
    public TextMeshProUGUI ScoreText { get { return scoreText; } }

    [Space]


    [SerializeField] Image resolutionBackground;
    public Image ResolutionBackground { get { return resolutionBackground; } }


    [SerializeField] TextMeshProUGUI resolutionStateInfoText;
    public TextMeshProUGUI ResolutionStateInfoText { get { return resolutionStateInfoText; } }

    [SerializeField] TextMeshProUGUI resolutionScoreText;
    public TextMeshProUGUI ResolutionScoreText { get { return resolutionScoreText; } }

    [Space]

    [SerializeField] TextMeshProUGUI highScoreText;
    public TextMeshProUGUI HighScoreText { get { return highScoreText; } }

    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup MainCanvasGroup { get { return mainCanvasGroup; } }

    [SerializeField] RectTransform finishedUIElements;
    public RectTransform FinishedUIElements { get { return finishedUIElements; } }
}


public class UIManager : MonoBehaviour
{
    public enum ResolutionScreenType { Correct, Incorrect, Finish }

    [Header("References")]
    [SerializeField] private GameEvents events;

    [Header("UIElements (Prefabs) ")]
    [SerializeField] private AnswerData answerPrefab;

    [SerializeField] private UIElements uiElements;

    [Space]

    [SerializeField] private UIManagerParameters parameters;

    List<AnswerData> currentAnswers = new List<AnswerData>();

    private void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
    }

    private void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
    }

    void UpdateQuestionUI(Question question)
    {
        uiElements.QuestionInfoTextObject.text = question.Info;
        CreateAnswers(question);

    }

    void CreateAnswers(Question question)
    {
        EraseAnswers();
        float offset = 0f - parameters.Margins;
        for (int i = 0; i < question.Answers.Length; i++)
        {
            AnswerData newAnswer = Instantiate(answerPrefab, uiElements.AnswerContentArea);
            newAnswer.UpdateData(question.Answers[i].Info, i);

            newAnswer.Rect.anchoredPosition = new Vector2(0, offset);

            offset -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);
            uiElements.AnswerContentArea.sizeDelta = new Vector2(uiElements.AnswerContentArea.sizeDelta.x, offset * -1f);

            currentAnswers.Add(newAnswer);

        }
    }

    void EraseAnswers()
    {
        foreach (var answer in currentAnswers)
        {
            Destroy(answer);
        }
        currentAnswers.Clear();
    }


}
