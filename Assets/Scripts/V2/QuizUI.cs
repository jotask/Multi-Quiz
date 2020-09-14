using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

namespace v2
{

    public class QuizUI : MonoBehaviour
    {

        [SerializeField] private QuizManager quizManager;

        [SerializeField] private Text questionText;
        [SerializeField] private Image questionImage;
        [SerializeField] private VideoPlayer questionVideo;
        [SerializeField] private AudioSource questionAudio;
        [SerializeField] private List<Button> options;

        [SerializeField] private Color correctColor;
        [SerializeField] private Color wrongColor;
        [SerializeField] private Color normalColor;

        private Question question;
        private bool answered;

        private float audioLength;

        private

        void Awake()
        {
            for (int i = 0; i < options.Count; i++)
            {
                Button b = options[i];
                b.onClick.AddListener(() => OnClick(b));
            }
        }

        void Update()
        {

        }

        public void SetQuestion(Question q)
        {
            this.question = q;
            switch (q.questionType)
            {
                case Question.QuestionType.Text:
                    questionImage.transform.parent.gameObject.SetActive(false);
                    break;
                case Question.QuestionType.Image:
                    ImageHolder();
                    questionImage.transform.gameObject.SetActive(true);
                    questionImage.sprite = question.questionImg;
                    break;
                case Question.QuestionType.Video:
                    ImageHolder();
                    questionVideo.transform.gameObject.SetActive(true);
                    questionVideo.clip = question.questionVideo;
                    questionVideo.Play();
                    break;
                case Question.QuestionType.Audio:
                    ImageHolder();
                    questionAudio.transform.gameObject.SetActive(true);
                    audioLength = question.questionAudio.length;
                    StartCoroutine(PlayAudio());
                    break;
            }

            questionText.text = question.info;

            // TODO shuffle options

            for (int i = 0; i < options.Count; i++)
            {
                options[i].GetComponentInChildren<Text>().text = question.options[i];
                options[i].name = question.options[i];
                options[i].image.color = normalColor;
            }

            answered = false;

        }

        IEnumerator PlayAudio()
        {
            if (question.questionType == Question.QuestionType.Audio)
            {
                questionAudio.PlayOneShot(question.questionAudio);
                yield return new WaitForSeconds(audioLength + 0.5f);
                StartCoroutine(PlayAudio());
            }
            else
            {
                StopCoroutine(PlayAudio());
                yield return null;
            }
        }

        private void ImageHolder()
        {
            questionImage.transform.parent.gameObject.SetActive(true);
            questionImage.transform.gameObject.SetActive(false);
            questionAudio.transform.gameObject.SetActive(false);
            questionVideo.transform.gameObject.SetActive(false);
            questionImage.transform.gameObject.SetActive(false);
        }

        private void OnClick(Button button)
        {
            if (answered == false)
            {
                answered = true;
                bool val = quizManager.Answer(button.name);
                if (val)
                {
                    button.image.color = correctColor;
                }
                else
                {
                    button.image.color = wrongColor;
                }
            }
        }

    }

}