using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CategoryData : MonoBehaviour
{

    public MenuManager menuManager;

    public TextMeshProUGUI buttonText;

    public string GategoryData = string.Empty;

    public void Init(string categoryName)
    {
        GategoryData = categoryName;
        buttonText.text = GategoryData;
    }

    public void ClickButton()
    {
        menuManager.click(GategoryData);
    }

}
