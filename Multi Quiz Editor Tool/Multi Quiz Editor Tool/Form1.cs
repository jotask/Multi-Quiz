using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multi_Quiz_Editor_Tool
{
    public partial class QuizEditorTool : Form
    {

        private QuestionFile questionFile;

        public QuizEditorTool()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName = dlg.FileName;
                questionFile = QuestionFile.LoadFromFile(fileName);
                LoadQuestionFile();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.CheckFileExists = false;
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(saveFileDialog1.FileName);
            }
        }

        private void LoadQuestionFile()
        {

            treeView1.Nodes.Clear();
            treeView2.Nodes.Clear();

            foreach (Category c in questionFile.categories)
            {
                var many = questionFile.questions.Where(item => item.category.id == c.id).Count();
                treeView2.Nodes.Add(c.name + " [" + many + "/" + questionFile.questions.Count + "]");
            }

            foreach (Question q in questionFile.questions)
            {
                treeView1.Nodes.Add(q.questionText);
            }
        }

        private void newQuestionBtn_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            PopulateQuestion(e.Node.Text);
        }
        private void PopulateQuestion(string questionText)
        {
            var q = questionFile.questions.Find(item => item.questionText.Equals(questionText));
            if (q != null)
            {
                // Question text
                richTextBox1.Text = q.questionText;

                // Question type
                comboBox2.Items.Clear();
                foreach (var names in Enum.GetValues(typeof(Question.QuestionType)))
                {
                    comboBox2.Items.Add(names);
                }

                comboBox2.SelectedItem = q.type;

                // Categories
                comboBox1.Items.Clear();
                foreach (var cat in questionFile.categories)
                {
                    comboBox1.Items.Add(cat.name);
                }

                var answersUI = new[] { new { text = richTextBox2, isCorrect = checkBox1 } }.ToList() ;
                answersUI.Clear();
                answersUI.Add(new { text = richTextBox2, isCorrect = checkBox1 });
                answersUI.Add(new { text = richTextBox3, isCorrect = checkBox2 });
                answersUI.Add(new { text = richTextBox4, isCorrect = checkBox3 });
                answersUI.Add(new { text = richTextBox5, isCorrect = checkBox4 });

                // Answers
                for (int i = 0; i < q.answers.Count; i++)
                {
                    Answer ans = q.answers[i];
                    var tmp = answersUI[i];
                    tmp.text.Text = ans.text;
                    tmp.isCorrect.Checked = ans.isCorrectAnswer;
                }
            }
            else
            {
                MessageBox.Show("Question [" + questionText + "] not found!. Something went completly wrong.");
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox4.Checked)
            {
                this.checkBox1.Checked = false;
                this.checkBox2.Checked = false;
                this.checkBox3.Checked = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox3.Checked)
            {
                this.checkBox1.Checked = false;
                this.checkBox2.Checked = false;
                this.checkBox4.Checked = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox2.Checked)
            {
                this.checkBox1.Checked = false;
                this.checkBox3.Checked = false;
                this.checkBox4.Checked = false;
            }
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.checkBox2.Checked = false;
                this.checkBox3.Checked = false;
                this.checkBox4.Checked = false;
            }
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
