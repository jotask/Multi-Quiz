using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public GameManager gameManager;

    public GameObject mainMenu;
    public GameObject categoriesMenu;
    public GameObject gameMenu;

    private void Awake()
    {
        mainMenu.SetActive(true);
        categoriesMenu.SetActive(false);
        gameMenu.SetActive(false);
    }

    public void GoToCategoriesMenu()
    {
        mainMenu.SetActive(false);
        categoriesMenu.SetActive(true);
        gameMenu.SetActive(false);
    }

    internal void click(string gategoryData)
    {
        GoToGameMenu();
        gameManager.Init(gategoryData);
    }

    public void GoToMainMenu()
    {
        categoriesMenu.SetActive(false);
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
    }
    public void GoToGameMenu()
    {
        categoriesMenu.SetActive(false);
        mainMenu.SetActive(false);
        gameMenu.SetActive(true);
    }

    public void Test(string str)
    {

    }

}
