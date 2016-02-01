using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CirclesManager : MonoBehaviour {
	Circle[] circles;
	InputField inputField;

	Slider healthBar;
	AudioSource[] audios;

	// Use this for initialization
	void Start () {
		 circles = GetComponentsInChildren<Circle>();
		 inputField = GameObject.Find("Canvas/InputField").GetComponent<InputField>();
		 healthBar = GameObject.Find("Canvas/Health").GetComponent<Slider>();
		 audios = GetComponents<AudioSource>();
	}

	// search through texts, find one to decrease in size
	public void EnterText (string s) {
		for (int i = 0; i < circles.Length; i++) {
			if (circles[i].Text.Equals(s)) {
				circles[i].ChangeScalePercent(0.33f);
				audios[1].Play();
				break;
			}
		}
	}

	float Area {
		get {
			float f = 0f;
			for (int i = 0; i < circles.Length; i++) {
				f += circles[i].Area;
			}
			return f;
		}
	}


	void FixedUpdate () { 
		float area = Area;

		// TODO set thresholds for damage here
		if (area > 10f) {
			healthBar.value -= 0.1f;
		} else {
			healthBar.value += 0.1f;
		}

		if (healthBar.value <= 0f) {
			//TODO game over
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
