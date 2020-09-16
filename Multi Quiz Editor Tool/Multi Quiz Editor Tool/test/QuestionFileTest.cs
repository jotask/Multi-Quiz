using Multi_Quiz_Editor_Tool.test.tests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_Quiz_Editor_Tool.test
{
    static class QuestionFileTest
    {
        public static bool TestQuestionFile(QuestionFile file)
        {
            List<QFTest> tests = new List<QFTest>();
            foreach (Type mytype in System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(mytype => mytype.GetInterfaces().Contains(typeof(QFTest))))
            {
                var test = (QFTest)Activator.CreateInstance(mytype);
                tests.Add(test);
            }

            foreach (var test in tests)
            {
                var result = test.Execute(file);
                if (result == false)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
