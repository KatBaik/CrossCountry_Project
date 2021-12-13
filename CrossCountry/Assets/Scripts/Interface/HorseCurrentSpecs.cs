using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorseCurrentSpecs : MonoBehaviour
{
    Horse horse;
    [SerializeField]
    Text speed;
    [SerializeField]
    Text endurance;
    string initialEndurance;
    [SerializeField]
    Text obedience;
    string initialObedience;
    [SerializeField]
    Text trust;
    string initialTrust;
    [SerializeField]
    Text skittishness;


    // Start is called before the first frame update
    void Start()
    {
        horse = ((GameObject.FindGameObjectsWithTag("MyHorse"))[0]).GetComponent<Horse>();
        initialEndurance = " of " + ((int)horse.Endurance).ToString();
        initialObedience = " of " + ((int)(horse.Obedience * 100)).ToString();
        initialTrust = " of " + ((int)(horse.Trust * 100)).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        speed.text = "Speed " + horse.Speed.ToString();
        endurance.text = "Endurance " + ((int)horse.Endurance).ToString() + initialEndurance;
        obedience.text = "Obedience " + ((int)(horse.Obedience * 100)).ToString() + initialObedience;
        trust.text = "Trust " + ((int)(horse.Trust * 100)).ToString() + initialTrust;
        skittishness.text = "Skittishness " + ((int)(horse.Skittishness * 100)).ToString();
    }
}
