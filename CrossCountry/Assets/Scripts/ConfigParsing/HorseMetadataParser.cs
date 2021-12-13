using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HorseMetadataParser
{
	string name;
	string description;
	float maxCanter;
	float minCanter;
	float maxTrot;
	float minTrot;
	float maxWalk;
	float endurance;
	float personalStamina;
	float jumpingHeight;
	float jumpingWidth;
	float skittishness;
	float obedience;
	float trust;
	float speedQuant;
	float rotateDegreesPerSecond;
	float lookAhead;
	float jumpHorizontalSpeed;
	float enduranceMinForJump;
	float enduranceMinForRunOut;

    public static HorseMetadataParser CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<HorseMetadataParser>(jsonString);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}
