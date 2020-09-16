using SimpleJSON;
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
                finalFile.categories.Add(new Category { id = -1, name = "Unknown" });
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
                    q.type = (question["type"] != null) ? (Question.QuestionType)Enum.Parse(typeof(Question.QuestionType), question["type"], true) : Question.QuestionType.Text;

                    if (question["category"].IsNumber == true)
                    {
                        q.category = finalFile.categories.Find(item => item.id == question["category"].AsInt);
                    }
                    else
                    {
                        q.category = finalFile.categories[0];
                    }

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

        public static string GetSaveData(QuestionFile file)
        {
            JSONNode root = new JSONObject();

            JSONObject config = new JSONObject();
            config.Add("defaultScore", new JSONNumber(file.configuration.defaultScoreToadd));
            config.Add("questionsToAsk", new JSONNumber(file.configuration.questionsToAskPerCategory));

            JSONArray categories = new JSONArray();
            foreach (var cat in file.categories)
            {
                JSONNode category = new JSONObject();
                category.Add("id", new JSONNumber(cat.id));
                category.Add("name", new JSONString(cat.name));
                categories.Add(category);
            }

            JSONArray questions = new JSONArray();
            foreach (var question in file.questions)
            {
                JSONNode q = new JSONObject();

                q.Add("category", question.category.id);
                q.Add("question", question.questionText);
                q.Add("type", question.type.ToString());
                // TODO Implement this into the code
                q.Add("link", "");
                q.Add("timer", 0);
                q.Add("score", 10);

                JSONArray answers = new JSONArray();
                foreach (var answer in question.answers)
                {
                    JSONObject a = new JSONObject();
                    a.Add("value", answer.text);
                    a.Add("correct", answer.isCorrectAnswer);
                    answers.Add(a);
                }
                q.Add("answer", answers);

                questions.Add(q);
            }

            root.Add("configuration", config);
            root.Add("categories", categories);
            root.Add("questions", questions);

            return root.ToString();
        }

    }
}
