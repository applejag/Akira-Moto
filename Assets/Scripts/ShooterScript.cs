using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShooterScript : MonoBehaviour {

    public List<WeaponScript> weapons = new List<WeaponScript>();

    [Space]

    public bool isEnemy = true;

	// Update is called once per frame
	void Update () {
	    foreach (var weapon in weapons) {
            weapon.Attack(isEnemy);
        }
	}
}
