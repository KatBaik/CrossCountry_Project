using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static StaticTrekData;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject trekPrefab;
    GameObject trek;
    [SerializeField]
    GameObject horsePrefab;
    GameObject horse;
    [SerializeField]
    GameObject hudPrefab;
    GameObject hud;

    public void InstantiateHorseAndHUD()
    {
        horse = Instantiate(horsePrefab, new Vector3(StaticTrekData.Start.x, StaticTrekData.HorseHeight, StaticTrekData.Start.z), Quaternion.identity);
        horse.transform.LookAt(StaticTrekData.DirectHorseTo);
        hud = Instantiate(hudPrefab, transform.position, Quaternion.identity);
    }

    public void DealWithFinishEvent()
    {
        SceneManager.LoadScene("FinishScene");
    }

    public void AwakeForHorse()
    {
        if (StaticTrekData.HorseConfigFile == "")
        {
            EventManager.AddHorseConfigFileIdentifiedListener(InstantiateHorseAndHUD);
            gameObject.AddComponent<HorseConnector>();
        }
        else
        {
            InstantiateHorseAndHUD();
        }        
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddSceneFinishedListener(DealWithFinishEvent);
        EventManager.AddTrekCreatedListener(AwakeForHorse);
        StaticTrekData.Start = new Vector3(0, 1, 0);
        trek = Instantiate(trekPrefab, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
