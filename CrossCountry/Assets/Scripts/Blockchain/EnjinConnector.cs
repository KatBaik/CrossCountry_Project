using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnjinConnector : MonoBehaviour
{
    public string platformUrl;
    public int appId;
    public string appSecret;

    public string developerName;
    public Enjin.SDK.DataTypes.User developer;

    public string playerName;
    public Enjin.SDK.DataTypes.User player;

    public static string GetURI(string itemName)
    {
        return Enjin.SDK.Core.Enjin.GetCryptoItemURI(itemName);
    }
    
    public static Enjin.SDK.Core.CryptoItem GetItem(string itemName)
    {
        return Enjin.SDK.Core.Enjin.GetToken(itemName);
    }

    public void MeltItem(string itemName, int quantity, int identity)
    {
        Enjin.SDK.Core.Enjin.MeltTokens(identity, itemName, quantity, (requestData) => { }, true);
    }
    
    public void MintNFTItem(string itemName, int senderId, string address)
    {
        Enjin.SDK.Core.Enjin.MintNonFungibleItem(senderId, new string[] { address }, itemName);
    }
    
    
    
    // Start is called before the first frame update
    private void Start()
    {
        Enjin.SDK.Core.Enjin.StartPlatform(platformUrl, appId, appSecret);
        developer = Enjin.SDK.Core.Enjin.GetUser(developerName);
        Login(playerName);
    }

    private void OnDestroy()
    {
        Enjin.SDK.Core.Enjin.CleanUpPlatform();
    }

    private void Login(string name)
    {
        Enjin.SDK.Core.Enjin.CreatePlayer(name);
        player = Enjin.SDK.Core.Enjin.GetUser(name);
        for (int i = 0; i<player.identities.Length; i++)
        {
            Enjin.SDK.Core.Identity identity = player.identities[i];
            Enjin.SDK.Core.Enjin.CreateIdentity(identity);
        }
        Enjin.SDK.Core.Enjin.AuthPlayer(name);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
