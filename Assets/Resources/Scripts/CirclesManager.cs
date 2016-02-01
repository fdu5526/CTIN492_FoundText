using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CirclesManager : MonoBehaviour {
	char [][] daysData;
	Timer dayTimer;
	int taskIndex;
	Timer taskTimer;
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
		 taskIndex = 0;
		 dayTimer = new Timer(30f);
		 dayTimer.Reset();
		 taskTimer = new Timer(StepTimeBasedOnDay);

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

	char CurrentTask {
		get {
			char[] dayData = daysData[Mathf.Min(currentDay, daysData.Length - 1)];
			return dayData[taskIndex];
		}
	}

	bool IsTaskRandomGenerated {
		get {
			char c = CurrentTask;
			return (c == 'w' || c == 's' || 
							c == 'm' || c == 'e');
		}
		
	}

	// increase the scale of a sphere, based on request
	void ActivateTask () {
		int i = 0;
		char c = CurrentTask;
		switch (c) {
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
				i = (int)Char.GetNumericValue(CurrentTask);
				break;
		}
		circles[i].ChangeScalePercent(3f);
	}

	void CheckTime () {
		if (!dayTimer.IsOffCooldown) {
		
			float p = dayTimer.PercentTimePassed;
			float[] thresholds = { 0.03f, 0.0625f, 0.13f, 0.3125f, 0.4f, 0.625f, 0.7f, 0.8125f, 0.9f, 1f };

			for (int i = 0; i < thresholds.Length; i++) {
				if (taskIndex == i && p > thresholds[i]) {
					taskIndex++;
					ActivateTask();
					taskTimer.Reset();
					break;
				}
			}
			
			if (taskTimer.IsOffCooldown && IsTaskRandomGenerated) {
				ActivateTask();
				taskTimer.Reset();
			}
			

		} else {
			//TODO move to next day
			currentDay++;
			taskIndex = 0;
			dayTimer.Reset();
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
