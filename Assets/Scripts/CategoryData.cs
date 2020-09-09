using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CategoryData : MonoBehaviour
{

    public MenuManager menuManager;

    public TextMeshProUGUI buttonText;

    public CategoriesManager.Category GategoryData;

    public void Init(CategoriesManager.Category category)
    {
        GategoryData = category;
        buttonText.text = GategoryData.text;
    }

    public void ClickButton()
    {
        menuManager.click(GategoryData);
    }

}
