﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Answer
{
    [SerializeField] private string _info;
    public string Info { get { return _info; } set { _info = value; } }

    [SerializeField] private bool _isCorrect;
    public bool IsCorrect { get { return _isCorrect; } set { _isCorrect = value; } }
}

[CreateAssetMenu(fileName = "Question", menuName = "Quiz/Question")]
public class Question : ScriptableObject
{
    public enum AnswerType { Multi, Single }

    [SerializeField] private string _info;
    public string Info { get { return _info; } set { _info = value; } }

    [SerializeField] private string _category;
    public string Category { get { return _category; } set { _category = value; } }

    [SerializeField] Answer[] _answers;
    public Answer[] Answers { get { return _answers; } set { _answers = value; } }

    // Parameters

    [SerializeField] private bool _useTimer = false;
    public bool UseTimer { get { return _useTimer; } set { _useTimer = value; } }

    [SerializeField] private int _timer = 0;
    public int Timer { get { return _timer; } set { _timer = value; } }

    [SerializeField] private AnswerType _answerType = AnswerType.Multi;
    public AnswerType GetAnswerType { get { return _answerType; } set { _answerType = value; } }

    [SerializeField] private int _addScore = 10;
    public int AddScore{ get { return _addScore; } set { _addScore = value; } }

    public List<int> GetCorrectAnswers()
    {
        List<int> correctAnswers = new List<int>();
        for (int i = 0; i < Answers.Length; i++)
        {
            if(Answers[i].IsCorrect)
            {
                correctAnswers.Add(i);
            }
        }
        return correctAnswers;
    }

}
