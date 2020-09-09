using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CategoriesManager : MonoBehaviour
{

    public GameManager gameManager;

    public GameObject CategoryPrefab;

    public Transform parentCategorySpawner;

    void Start()
    {
        var tmp = GetComponent<MenuManager>();
        foreach (var cat in GetAllCategoriesFromFile())
        {
            var c = Instantiate(CategoryPrefab, parentCategorySpawner);
            var data = c.GetComponent<CategoryData>();
            data.Init(cat.Replace("\"", string.Empty));
            data.menuManager = tmp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    HashSet<String> GetAllCategoriesFromFile()
    {
        List<Question> allQuestions = new List<Question>();
        var fileToString = File.ReadAllText(GameUtility.FileToRead);
        var root = SimpleJSON.JSON.Parse(fileToString);

        HashSet<String> categories = new HashSet<string>();

        // Read questions
        {
            var questionsNode = root["questions"];
            for (int i = 0; i < questionsNode.Count; i++)
            {

                var Node = questionsNode[i];
                var category = Node["category"].ToString().ToLower();
                if (categories.Contains(category) == false)
                {
                    categories.Add(category);
                }
            }

        }

        return categories;
    }

}
