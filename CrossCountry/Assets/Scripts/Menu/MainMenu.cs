using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject helpPrefab;

    public void HandlePlayNewTrekButtonClickEvent()
    {
        MenuManager.GoToMenu(MenuName.NewTrek);
    }

    public void HandleRepeatButtonOnClickEvent()
    {
        MenuManager.GoToMenu(MenuName.Repeat);
    }

    public void HandleHelpButtonOnClickEvent()
    {
        Instantiate(helpPrefab, transform.position, Quaternion.identity);
    }

    public void HandleBackQuitButtonOnClickEvent()
    {
        Application.Quit();
    }
}
