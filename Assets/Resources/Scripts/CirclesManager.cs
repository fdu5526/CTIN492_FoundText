using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CirclesManager : MonoBehaviour {
	Circle[] circles;
	InputField inputField;


	// Use this for initialization
	void Start () {
		 circles = GetComponentsInChildren<Circle>();
		 inputField = GameObject.Find("Canvas/InputField").GetComponent<InputField>();
	}

	// search through texts, find one to decrease in size
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
		if (Input.GetKeyDown(KeyCode.Return)) {
			EnterText(inputField.text);
			inputField.text = "";
		}

		inputField.Select();
 		inputField.ActivateInputField();
		
	}
}
