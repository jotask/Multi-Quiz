using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multi_Quiz_Editor_Tool.test.tests
{
    class CategoryTest : QFTest
    {
        public bool Execute(QuestionFile qf)
        {

            // Test for each category to confirm they have an unique id
            HashSet<int> set = new HashSet<int>();
            foreach (var cat in qf.categories)
            {
                var result = set.Add(cat.id);
                if (result == false)
                {
                    MessageBox.Show("the category " + cat.name + " contains a duplicated id.");
                    return false;
                }
            }

            // Test if all quetions have a correct category
            foreach (var question in qf.questions)
            {
                if (set.Contains(question.category.id) == false)
                {
                    MessageBox.Show("The question contains a not valid category. The category id for this questions has not been found.");
                    return false;
                }
            }
            return true;
        }
    }
}
