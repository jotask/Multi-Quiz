﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvents", menuName ="Quiz/GameEvents")]
public class GameEvents : ScriptableObject
{
    public delegate void UpdateQuestionUICallback(Question question);
    public UpdateQuestionUICallback UpdateQuestionUI;

    public delegate void UpdateQuestionAnswerCallback(AnswerData pickedAnswer);
    public UpdateQuestionAnswerCallback UpdateQuestionAnswer;

    public delegate void DisplayResolutionScreenCallback(UIManager.ResolutionScreenType type, int score);
    public DisplayResolutionScreenCallback DisplayResolutionScreen;

    public delegate void ScoreUpdatedCallback();
    public ScoreUpdatedCallback ScoreUpdated;

    [HideInInspector]
    public int CurrentFinalScore = 0;

    [HideInInspector]
    public int StartupHighScore;

}
