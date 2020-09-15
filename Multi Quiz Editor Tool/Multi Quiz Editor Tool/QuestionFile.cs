using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Multi_Quiz_Editor_Tool
{

    public class Category
    {
        public int id;
        public string name;
    }

    public class Answer {
        public string text;
        public bool isCorrectAnswer;
    }

    public class Question
    {
        public enum QuestionType { Text, Image, Video, Audio }
        public QuestionType type;
        public string questionText;
        public Category category;
        public List<Answer> answers;

    }

    public class QuestionFile
    {
        public class QuestionConfiguration
        {
            public int defaultScoreToadd;
            public int questionsToAskPerCategory;
        }

        public QuestionConfiguration configuration;
        public List<Category> categories;
        public List<Question> questions;

        public static QuestionFile LoadFromFile(string file)
        {

            var fileToString = File.ReadAllText(file);
            var root = SimpleJSON.JSON.Parse(fileToString);

            QuestionFile finalFile = new QuestionFile();

            // Configuration
            {
                finalFile.configuration = new QuestionConfiguration();
                finalFile.configuration.defaultScoreToadd = root["configuration"]["defaultScore"].AsInt;
                finalFile.configuration.questionsToAskPerCategory = root["configuration"]["questionsToAsk"].AsInt;
            }

            // categories
            {
                Dictionary<int, string> categoryMap = new Dictionary<int, string>();
                var categoriesNode = root["categories"];
                for (int i = 0; i < categoriesNode.Count; i++)
                {
                    var id = categoriesNode[i]["id"].AsInt;
                    var name = categoriesNode[i]["name"];
                    if (categoryMap.ContainsKey(id) == false && categoryMap.ContainsValue(name) == false)
                    {
                        categoryMap.Add(id, name);
                    }
                    else
                    {
                        Debug.Fail("Error loading category with id " + id + " and name " + name);
                    }
                }
                finalFile.categories = new List<Category>();
                foreach (var tmp in categoryMap)
                {
                    Category c = new Category();
                    c.id = tmp.Key;
                    c.name = tmp.Value;
                    finalFile.categories.Add(c);
                }
            }

            // Questions
            {
                finalFile.questions = new List<Question>();
                var questionsNode = root["questions"];
                for (int i = 0; i < questionsNode.Count; i++)
                {
                    var question = questionsNode[i];
                    Question q = new Question();
                    q.questionText = question["question"];
                    q.type = (question["type"] != null) ? (Question.QuestionType) Enum.Parse(typeof(Question.QuestionType), question["type"], true) : Question.QuestionType.Text;
                    var cat = finalFile.categories.Find(item => item.id == question["category"].AsInt);
                    q.category = (cat == null) ? new Category{ id = -1, name = "Unknow" } : cat;
                    q.answers = new List<Answer>();
                    for (int a = 0; a < question["answer"].Count; a++)
                    {
                        var answer = question["answer"][a];
                        Answer answ = new Answer();
                        answ.text = answer["value"];
                        answ.isCorrectAnswer = answer["correct"] != null && answer["correct"].AsBool;
                        q.answers.Add(answ);
                    }
                    finalFile.questions.Add(q);
                }
            }

            return finalFile;
        }

    }
}
