using UnityEngine;
using System.Collections;

public class CirclesManager : MonoBehaviour {
	Circle[] circles;

	// Use this for initialization
	void Start () {
		 circles = GetComponentsInChildren<Circle>();
	}


	public void EnterText (string s) {
		for (int i = 0; i < circles.Length; i++) {
			if (circles[i].Text.Equals(s)) {
				circles[i].ChangeScalePercent(0.5f);
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
