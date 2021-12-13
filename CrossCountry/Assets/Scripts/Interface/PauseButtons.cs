using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtons : MonoBehaviour
{
    public void HandleResumeClickEvent()
    {
        Time.timeScale = 1;
        Destroy(gameObject);
    }

    public void HandleMainMenuClickEvent()
    {
        Time.timeScale = 1;
        MenuManager.GoToMenu(MenuName.MainMenu);
    }
}
