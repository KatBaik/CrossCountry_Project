using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishMenu : MonoBehaviour
{
    public void HandleMainMenuButtonClickEvent()
    {
        MenuManager.GoToMenu(MenuName.MainMenu);
    }

    public void HandleBackQuitButtonOnClickEvent()
    {
        Application.Quit();
    }
}
