using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackHelpButton : MonoBehaviour
{
    public void HandleBackClickEvent()
    {
        Destroy(gameObject);
    }
}
