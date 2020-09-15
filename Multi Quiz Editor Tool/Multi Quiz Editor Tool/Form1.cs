using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
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
                SaveQuestionFile(saveFileDialog1.FileName);
            }
        }

        private void LoadQuestionFile()
        {

            treeView1.Nodes.Clear();
            treeView2.Nodes.Clear();

            textBox1.Text = questionFile.configuration.defaultScoreToadd.ToString();
            textBox2.Text = questionFile.configuration.questionsToAskPerCategory.ToString();

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

        private void SaveQuestionFile(string file)
        {
            MessageBox.Show("Not implemented yet, sorry! ): " + file);
        }

        private void newQuestionBtn_Click(object sender, EventArgs e)
        {
            Form prompt = new Form();
            prompt.Text = "Create new question";
            prompt.AutoSize = true;

            Label questionLabel = new Label() { Dock = DockStyle.Top, Text = "Question" };
            TextBox inputQuestionBox = new TextBox() { Dock = DockStyle.Top, Width = 400 };

            Label categoryLabel = new Label() { Dock = DockStyle.Top, Text = "Category" };
            ComboBox catCombo = new ComboBox { Dock = DockStyle.Top, Width = 400 };
            foreach (var cat in questionFile.categories)
            {
                catCombo.Items.Add(cat.name);
            }
            catCombo.SelectedIndex = 0;

            Label typeLabel = new Label() { Dock = DockStyle.Top, Text = "Question type" };
            ComboBox type = new ComboBox { Dock = DockStyle.Top, Width = 400 };
            foreach (var names in Enum.GetValues(typeof(Question.QuestionType)))
            {
                type.Items.Add(names);
            }
            type.SelectedItem = Question.QuestionType.Text;

            Button confirmation = new Button() { Text = "Ok", Dock = DockStyle.Fill };
            Button exit = new Button() { Text = "Cancel", Dock = DockStyle.Fill };
            confirmation.Click += (senderr, ee) => { prompt.DialogResult = DialogResult.OK; prompt.Close(); };
            exit.Click += (senderr, ee) => { prompt.DialogResult = DialogResult.Cancel; prompt.Close(); };

            GroupBox answerBox = new GroupBox() { Text = "Answers", Dock = DockStyle.Top };
            answerBox.AutoSize = true;

            Label a1L = new Label() { Dock = DockStyle.Top, Text = "Answer 1" };
            TextBox a1T = new TextBox() { Dock = DockStyle.Top };
            CheckBox a1C = new CheckBox() { Dock = DockStyle.Top, Text = "Is CorrectAnswer", Checked = false };
            Label a2L = new Label() { Dock = DockStyle.Top, Text = "Answer 2" };
            TextBox a2T = new TextBox() { Dock = DockStyle.Top };
            CheckBox a2C = new CheckBox() { Dock = DockStyle.Top, Text = "Is CorrectAnswer", Checked = false };
            Label a3L = new Label() { Dock = DockStyle.Top, Text = "Answer 3" };
            TextBox a3T = new TextBox() { Dock = DockStyle.Top };
            CheckBox a3C = new CheckBox() { Dock = DockStyle.Top, Text = "Is CorrectAnswer", Checked = false };
            Label a4L = new Label() { Dock = DockStyle.Top, Text = "Answer 4" };
            TextBox a4T = new TextBox() { Dock = DockStyle.Top };
            CheckBox a4C = new CheckBox() { Dock = DockStyle.Top, Text = "Is CorrectAnswer", Checked = false };

            a1C.CheckedChanged += new System.EventHandler((senderrr, eee) =>
            {
                if (a1C.Checked)
                {
                    a2C.Checked = false;
                    a3C.Checked = false;
                    a4C.Checked = false;
                }
            });
            a2C.CheckedChanged += new System.EventHandler((senderrr, eee) =>
            {
                if (a2C.Checked)
                {
                    a1C.Checked = false;
                    a3C.Checked = false;
                    a4C.Checked = false;
                }
            });
            a3C.CheckedChanged += new System.EventHandler((senderrr, eee) =>
            {
                if (a3C.Checked)
                {
                    a1C.Checked = false;
                    a2C.Checked = false;
                    a4C.Checked = false;
                }
            });
            a4C.CheckedChanged += new System.EventHandler((senderrr, eee) =>
            {
                if (a4C.Checked)
                {
                    a1C.Checked = false;
                    a2C.Checked = false;
                    a3C.Checked = false;
                }
            });

            answerBox.Controls.Add(a4C);
            answerBox.Controls.Add(a4T);
            answerBox.Controls.Add(a4L);
            answerBox.Controls.Add(a3C);
            answerBox.Controls.Add(a3T);
            answerBox.Controls.Add(a3L);
            answerBox.Controls.Add(a2C);
            answerBox.Controls.Add(a2T);
            answerBox.Controls.Add(a2L);
            answerBox.Controls.Add(a1C);
            answerBox.Controls.Add(a1T);
            answerBox.Controls.Add(a1L);

            SplitContainer splitPanel = new SplitContainer();
            splitPanel.Dock = DockStyle.Bottom;
            splitPanel.Panel1.Controls.Add(exit);
            splitPanel.Panel2.Controls.Add(confirmation);

            prompt.Controls.Add(splitPanel);
            prompt.Controls.Add(answerBox);
            prompt.Controls.Add(type);
            prompt.Controls.Add(typeLabel);
            prompt.Controls.Add(catCombo);
            prompt.Controls.Add(categoryLabel);
            prompt.Controls.Add(inputQuestionBox);
            prompt.Controls.Add(questionLabel);

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                Question q = new Question();
                q.questionText = inputQuestionBox.Text;
                q.type = (Question.QuestionType) Enum.Parse(typeof(Question.QuestionType), type.SelectedItem.ToString(), true);
                q.category = questionFile.categories.Find(item => item.name.Equals(catCombo.SelectedItem.ToString(), StringComparison.InvariantCultureIgnoreCase));
                q.answers = new List<Answer>();
                q.answers.Add(new Answer() { text = a1T.Text, isCorrectAnswer = a1C.Checked });
                q.answers.Add(new Answer() { text = a2T.Text, isCorrectAnswer = a2C.Checked });
                q.answers.Add(new Answer() { text = a3T.Text, isCorrectAnswer = a3C.Checked });
                q.answers.Add(new Answer() { text = a4T.Text, isCorrectAnswer = a4C.Checked });
                questionFile.questions.Add(q);
                LoadQuestionFile();
            }
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
                comboBox1.SelectedItem = q.category.name;

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

        private void button2_Click(object sender, EventArgs e)
        {
            Form prompt = new Form();
            prompt.Width = 500;
            prompt.Height = 150;
            prompt.Text = "Create a new category";
            Label textLabel = new Label() { Left = 50, Top = 20, Text = "New category name" };
            TextBox inputBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70 };
            Button exit = new Button() { Text = "Cancel", Left = 50, Width = 100, Top = 70 };
            confirmation.Click += (senderr, ee) => { prompt.DialogResult = DialogResult.OK; prompt.Close(); };
            exit.Click += (senderr, ee) => { prompt.DialogResult = DialogResult.Cancel; prompt.Close(); };
            prompt.Controls.Add(exit);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                // find available catgory id
                List<int> catId = new List<int>();
                foreach (var cat in questionFile.categories)
                {
                    catId.Add(cat.id);
                }
                catId.OrderBy(item => item);
                int newCatId = 0;
                for (int i = 0; i < catId.Count; i++)
                {
                    if (newCatId == catId[i])
                    {
                        newCatId++;
                    }
                    else
                    {
                        break;
                    }
                }
                questionFile.categories.Add(new Category { id = newCatId , name = inputBox.Text });
                LoadQuestionFile();
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            questionFile.configuration.defaultScoreToadd = int.Parse(textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            questionFile.configuration.questionsToAskPerCategory = int.Parse(textBox2.Text);
        }

    }
}
