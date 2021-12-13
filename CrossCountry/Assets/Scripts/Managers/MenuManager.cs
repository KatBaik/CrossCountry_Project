using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static MenuName;

public static class MenuManager
{
    public static void GoToMenu(MenuName command)
    {
        switch (command)
        {
            case MenuName.NewTrek:
                StaticTrekData.IsLoaded = false;
                SceneManager.LoadScene("GameplayScene");
                break;
            case MenuName.Repeat:
                StaticTrekData.IsLoaded = true;
                SceneManager.LoadScene("GameplayScene");
                break;
            case MenuName.Help:
                break;
            case MenuName.MainMenu:
                SceneManager.LoadScene("StartScene");
                break;
            case MenuName.Exit:
                break;
            case MenuName.Stop:
                break;
            case MenuName.Resume:
                break;
        }
    }
}
