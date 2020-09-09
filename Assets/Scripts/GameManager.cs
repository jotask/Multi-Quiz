﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    Question[] _questions;
    public Question[] Questions { get { return _questions; } }

    [SerializeField] GameEvents events = null;

    [SerializeField] Animator timerAnimator;
    [SerializeField] TextMeshProUGUI timerTexts;
    [SerializeField] Color timerHalfWaitOutColor = Color.yellow;
    [SerializeField] Color timerAlmostOutColor = Color.red;


    private List<AnswerData> PickedAnswers = new List<AnswerData>();
    private List<int> FinishedQuestions = new List<int>();
    private int currentQuestion = 0;

    private int TimerStateParameterHash = 0;

    private IEnumerator IE_WaitUntilNextRound = null;
    private IEnumerator IE_StartTimer = null;



    private Color timerDefaultColor;

    private bool IsFinished
    {
        get { return FinishedQuestions.Count < Questions.Length ? false : true; }
    }

    private void OnEnable()
    {
        events.UpdateQuestionAnswer += UpdateAnswers;
    }

    private void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpdateAnswers;
    }

    private void Awake()
    {
        events.CurrentFinalScore = 0;
    }

    private void Start()
    {

        events.StartupHighScore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

        timerDefaultColor = timerTexts.color;

        LoadQuestions();

        TimerStateParameterHash = Animator.StringToHash("TimerState");

        int seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        UnityEngine.Random.InitState(seed);

        Display();
    }

    public void UpdateAnswers(AnswerData newAnswer)
    {
        if (Questions[currentQuestion].GetAnswerType == Question.AnswerType.Single)
        {
            foreach (var answer in PickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
                PickedAnswers.Clear();
                PickedAnswers.Add(newAnswer);
            }
        }
        else if (Questions[currentQuestion].GetAnswerType == Question.AnswerType.Multi)
        {
            bool alreadyPicked = PickedAnswers.Exists(item => item == newAnswer);
            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            {
                PickedAnswers.Add(newAnswer);
            }
        }
        else
        {
            Debug.LogError("Oh noo! There is something wrong here. Please check this question answer type! :(");
        }
    }

    public void EraseAnswers()
    {
        PickedAnswers = new List<AnswerData>();
    }

    public void Accept()
    {

        UpdateTimer(false);

        bool isCorrect = CheckAnswers();
        FinishedQuestions.Add(currentQuestion);

        UpdateScore(isCorrect ? Questions[currentQuestion].AddScore : -Questions[currentQuestion].AddScore);

        if (IsFinished)
        {
            SetHighScore();
        }

        var type = IsFinished ? UIManager.ResolutionScreenType.Finish : (isCorrect) ? UIManager.ResolutionScreenType.Correct : UIManager.ResolutionScreenType.Incorrect;

        if (events.DisplayResolutionScreen != null)
        {
            events.DisplayResolutionScreen(type, Questions[currentQuestion].AddScore);
        }

        if (IE_WaitUntilNextRound != null)
        {
            StopCoroutine(IE_WaitUntilNextRound);
        }
        IE_WaitUntilNextRound = WaitUntilNextRound();
        StartCoroutine(IE_WaitUntilNextRound);

    }

    private bool CheckAnswers()
    {
        if (!CompareAnswers())
            return false;
        return true;
    }

    bool CompareAnswers()
    {
        if (PickedAnswers.Count > 0)
        {
            List<int> correctAnswers = Questions[currentQuestion].GetCorrectAnswers();
            List<int> picked = PickedAnswers.Select(item => item.AnswerIndex).ToList();

            var f = correctAnswers.Except(picked).ToList();
            var s = picked.Except(correctAnswers).ToList();

            return !f.Any() && !s.Any();

        }
        return false;
    }

    public void Display()
    {
        EraseAnswers();
        var question = GetRandomQuestion();

        if (events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question);
        }
        else
        {
            Debug.LogWarning("Ups! Something went wrong while trying to display new Questions UI Data. GameEvents.UpdateQuestionsUI is null! :(");
        }

        if(question.UseTimer)
        {
            UpdateTimer(question.UseTimer);
        }

    }

    void UpdateTimer(bool state)
    {
        if (state)
        {
            IE_StartTimer = StartTimer();
            StartCoroutine(IE_StartTimer);

            timerAnimator.SetInteger(TimerStateParameterHash, 2);

        }
        else
        {
            if (IE_StartTimer != null)
            {
                StopCoroutine(IE_StartTimer);
            }

            timerAnimator.SetInteger(TimerStateParameterHash, 1);
        }
    }

    IEnumerator StartTimer()
    {
        var totalTime = Questions[currentQuestion].Timer;
        var timeLeft = totalTime;

        timerTexts.color = timerDefaultColor;

        while (timeLeft > 0)
        {
            timeLeft--;
            if (timeLeft < totalTime / 2f && timeLeft > totalTime / 4f)
            {
                timerTexts.color = timerHalfWaitOutColor;
            }
            else if (timeLeft < totalTime / 4f)
            {
                timerTexts.color = timerAlmostOutColor;
            }

            timerTexts.text = timeLeft.ToString();
            yield return new WaitForSeconds(1f);
        }
        Accept();
    }

    IEnumerator WaitUntilNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
    }

    Question GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;
        return _questions[currentQuestion];
    }

    int GetRandomQuestionIndex()
    {
        var random = 0;
        if (FinishedQuestions.Count < _questions.Length)
        {
            do
            {
                random = UnityEngine.Random.Range(0, _questions.Length);
            } while (FinishedQuestions.Contains(random) || random == currentQuestion);
        }
        return random;
    }

    void LoadQuestions()
    {
        UnityEngine.Object[] objs = Resources.LoadAll("Questions", typeof(Question));
        _questions = new Question[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            _questions[i] = objs[i] as Question;
        }
    }

    private void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;

        if (events.ScoreUpdated != null)
        {
            events.ScoreUpdated();
        }
    }

    void SetHighScore()
    {
        var prevHighScore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        if(prevHighScore < events.CurrentFinalScore)
        {
            PlayerPrefs.SetInt(GameUtility.SavePrefKey, events.CurrentFinalScore);
        }
    }

    public void PlayAgainButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
