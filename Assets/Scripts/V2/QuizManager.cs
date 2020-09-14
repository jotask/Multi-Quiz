using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace v2
{
    public class QuizManager : MonoBehaviour
    {

        [SerializeField] private QuizUI quizUI;

        [SerializeField]
        private List<Question> questions;

        private Question selectedQuestion;

        void Start()
        {
            SelectQuestion();
        }

        void SelectQuestion()
        {
            int value = Random.Range(0, questions.Count);
            selectedQuestion = questions[value];
            quizUI.SetQuestion(selectedQuestion);
        }

        public bool Answer(string answer)
        {
            bool correctAnswer = false;
            if (answer == selectedQuestion.correctAnswer)
            {
                correctAnswer = true;
            }
            else
            {

            }
            Invoke("SelectQuestion", 0.4f);
            return correctAnswer;
        }

    }

    [System.Serializable]
    public class Question
    {

        [System.Serializable]
        public enum QuestionType { Text, Image, Video, Audio }

        public string info;
        public List<string> options;
        public string correctAnswer;
        public QuestionType questionType;
        public Sprite questionImg;
        public AudioClip questionAudio;
        public VideoClip questionVideo;
    }

}