using UnityEngine;
using System.Collections;

public class CameraDropDown : PropertyAttribute {

	public bool fullPath = true;
	public bool hideBool = true;

	public CameraDropDown (bool fullPath = true, bool hideBool = true) {
		this.fullPath = fullPath;
		this.hideBool = hideBool;
	}

}



