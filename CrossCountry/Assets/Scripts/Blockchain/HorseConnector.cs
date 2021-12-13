using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HorseConnector : MonoBehaviour
{
    public EnjinConnector enjinConnector;
    public string itemId = "3880000000001801";
    HorseConfigFileIdentifiedEvent horseConfigFileIdentifiedEvent = new HorseConfigFileIdentifiedEvent();

    public void AddHorseConfigFileIdentifiedEventListener(UnityAction listener)
    {
        horseConfigFileIdentifiedEvent.AddListener(listener);
    }


    private void GetHorseData()
    {
        Enjin.SDK.Core.CryptoItem item;
        
        int id = enjinConnector.developer.id;
        //string address = enjinConnector.developer.identities[0].wallet.ethAddress;
        //int id = enjinConnector.player.id;
        string address = enjinConnector.player.identities[0].wallet.ethAddress;
        //enjinConnector.MintNFTItem("3880000000001801", id, address);
        //enjinConnector.MintNFTItem("3880000000001801", 19212, address);
        //enjinConnector.MintNFTItem(itemNames[0], id, address);
        item = EnjinConnector.GetItem(itemId);
        string fileName = System.String.Join("_", (item.name).Split(' ')) + ".csv";
        /*Enjin.SDK.Core.Enjin.SetCryptoItemURI(19212, item, "https://jsonkeeper.com/b/LRQT",
            (requestData) => { });*/

        //string uri = EnjinConnector.GetURI(itemNames[0]);
        //string uri = EnjinConnector.GetURI("3880000000001801");
        //Debug.Log(uri);
        StaticTrekData.HorseConfigFile = fileName;
        horseConfigFileIdentifiedEvent.Invoke();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddHorseConfigFileIdentifiedInvoker(this);
        enjinConnector = gameObject.GetComponent<EnjinConnector>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (StaticTrekData.HorseConfigFile == "")
        {
            GetHorseData();
        }        
    }
}
