using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CategoriesManager : MonoBehaviour
{

    public struct Category
    {
        public string id;
        public string text;
    }

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
            data.Init(cat);
            data.menuManager = tmp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    HashSet<Category> GetAllCategoriesFromFile()
    {
        List<Question> allQuestions = new List<Question>();
        var fileToString = File.ReadAllText(GameUtility.FileToRead);
        var root = SimpleJSON.JSON.Parse(fileToString);

        HashSet<Category> categories = new HashSet<Category>();

        // Read questions
        {
            var categoriesNode = root["categories"];
            for (int i = 0; i < categoriesNode.Count; i++)
            {
                var category = categoriesNode[i];

                Category cat = new Category();
                cat.id = category["id"].ToString().Replace("\"", string.Empty);
                cat.text = category["name"].ToString().Replace("\"", string.Empty);
                categories.Add(cat);
            }

        }
        return categories;
    }

}
