using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multi_Quiz_Editor_Tool.test.tests
{
    class AnswerTest : QFTest
    {
        public bool Execute(QuestionFile qf)
        {
            // Test for each question answer that contains some text and has at least one correct answer
            foreach (var question in qf.questions)
            {
                int manyCorrectAnswers = 0;
                foreach (var answer in question.answers)
                {
                    if (answer.text == string.Empty || answer.text.Length == 0)
                    {
                        MessageBox.Show("There is one question [" + question.questionText + "]" + "that contains an empty answer");
                        return false;
                    }

                    if (answer.isCorrectAnswer)
                    {
                        manyCorrectAnswers++;
                    }
                }
                if (manyCorrectAnswers == 0)
                {
                    MessageBox.Show("The question [" + question.questionText + "]" + " does not contains any correct answer.");
                    return false;
                }
                if (manyCorrectAnswers != 1)
                {
                    MessageBox.Show("The question [" + question.questionText + "]" + " contains multiple corrects answers. This might not be supported.");
                    return false;
                }
            }
            return true;
        }
    }
}
