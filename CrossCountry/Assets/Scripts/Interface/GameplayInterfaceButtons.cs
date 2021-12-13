using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayInterfaceButtons : MonoBehaviour
{
    [SerializeField]
    GameObject pausePrefab;

    public void HandleStopButtonClickEvent()
    {
        Time.timeScale = 0;
        Instantiate(pausePrefab, transform.position, Quaternion.identity);
    }
}
