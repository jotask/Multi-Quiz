using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UIManagerParameters
{
    [Header("AnswersOptions")]
    [SerializeField] float margins;
    public float Margins { get { return margins; } }

    [Header("Resolution Screen Options")]
    [SerializeField] Color correctBgColor;
    public Color CorrectBgColor { get { return correctBgColor; } }

    [SerializeField] Color inCorrectBgColor;
    public Color IncorrectBgColor { get { return inCorrectBgColor; } }

    [SerializeField] Color finalBgColor;
    public Color FinalBgColor { get { return finalBgColor; } }

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

    [SerializeField] Animator resolutionScreenAnimator;
    public Animator ResolutionScreenAnimator { get { return resolutionScreenAnimator; } }


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

    private IEnumerator IE_DisplayTimedResolution;

    private int resolutionStateHashParameter = 0;

    private void Start()
    {
        resolutionStateHashParameter = Animator.StringToHash("ScreenState");
    }

    private void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
        events.DisplayResolutionScreen += DisplayResolution;
    }

    private void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
        events.DisplayResolutionScreen -= DisplayResolution;
    }

    void DisplayResolution(ResolutionScreenType type, int score)
    {
        UpdateResolutionUI(type, score);
        uiElements.ResolutionScreenAnimator.SetInteger(resolutionStateHashParameter, 2); // Popup animation
        uiElements.MainCanvasGroup.blocksRaycasts = false;

        if (type != ResolutionScreenType.Finish)
        {
            if (IE_DisplayTimedResolution != null)
            {
                StopCoroutine(IE_DisplayTimedResolution);
            }
            IE_DisplayTimedResolution = DisplayTimeResolution();
            StartCoroutine(IE_DisplayTimedResolution);
        }

    }

    IEnumerator DisplayTimeResolution()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        uiElements.ResolutionScreenAnimator.SetInteger(resolutionStateHashParameter, 1); // Fade out
        uiElements.MainCanvasGroup.blocksRaycasts = true;
    }

    void UpdateResolutionUI(ResolutionScreenType type, int score)
    {
        var highScore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

        switch (type)
        {
            case ResolutionScreenType.Correct:
                uiElements.ResolutionBackground.color = parameters.CorrectBgColor;
                uiElements.ResolutionStateInfoText.text = "Correct!";
                uiElements.ScoreText.text = "+" + score;
                break;
            case ResolutionScreenType.Incorrect:
                uiElements.ResolutionBackground.color = parameters.IncorrectBgColor;
                uiElements.ResolutionStateInfoText.text = "Wrong!";
                uiElements.ScoreText.text = "-" + score;
                break;
            case ResolutionScreenType.Finish:
                uiElements.ResolutionBackground.color = parameters.FinalBgColor;
                uiElements.ResolutionStateInfoText.text = "Final Score!";
                StartCoroutine(CalculateScore());

                uiElements.FinishedUIElements.gameObject.SetActive(true);
                uiElements.HighScoreText.gameObject.SetActive(true);
                // Display high score
                uiElements.HighScoreText.text = (highScore > events.StartupHighScore ? "<color=yellow>new </color>" : string.Empty) + "Highscore " + highScore;

                break;
        }
    }

    IEnumerator CalculateScore()
    {
        var scoreValue = 0;
        while (scoreValue < events.CurrentFinalScore)
        {
            scoreValue++;
            uiElements.ResolutionScoreText.text = scoreValue.ToString();
            yield return null;
        }
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
