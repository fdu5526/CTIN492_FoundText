using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CirclesManager : MonoBehaviour {
	char [][] daysData;
	Timer dayTimer;
	float prevPercentTimePassed;
	Timer stepTimer;
	int currentDay;

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

		 currentDay = 0;
		 prevPercentTimePassed = 0f;
		 dayTimer = new Timer(30f);
		 dayTimer.Reset();
		 stepTimer = new Timer(StepTimeBasedOnDay);

		 daysData = CSVParser.Parse("Data/days");
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

	float StepTimeBasedOnDay {
		get {
			//TODO increase rate as days go by
			return (float)currentDay;
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

	// increase the scale of a sphere, based on request
	void ActivateTask (int dayDataIndex) {
		int i = 0;
		char[] dayData = daysData[Mathf.Min(currentDay, daysData.Length - 1)];		
		switch (dayData[dayDataIndex]) {
			case 'w':
				i = (int)UnityEngine.Random.Range(9, 12);
				break;
			case 'e':
				i = (int)UnityEngine.Random.Range(16, 20);
				break;
			case 's':
				i = (int)UnityEngine.Random.Range(4, 8);
				break;
			case 'm':
				i = (int)UnityEngine.Random.Range(12, 16);
				break;
			default:
				i = (int)Char.GetNumericValue(dayData[dayDataIndex]);
				break;
		}

		circles[i].ChangeScalePercent(3f);

	}


	void CheckTime () {
		if (!dayTimer.IsOffCooldown) {
		
			float p = dayTimer.PercentTimePassed;
			if (p < 0.03f) {
				ActivateTask(0);
			} else if (p < 0.0625f) {
				ActivateTask(1);
			}



			prevPercentTimePassed = dayTimer.PercentTimePassed;
		} else {
			//TODO move to next day
			currentDay++;
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
		} else {
			CheckTime();
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
