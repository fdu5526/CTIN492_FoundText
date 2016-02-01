using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CirclesManager : MonoBehaviour {
	string [][] daysData;
	Timer dayTimer;
	Timer endOfDayTimer;
	int taskIndex;
	Timer taskTimer;
	int currentDay;

	Circle[] circles;
	InputField inputField;

	Slider stressBar;
	AudioSource[] audios;

	// Use this for initialization
	void Start () {
		 circles = GetComponentsInChildren<Circle>();
		 inputField = GameObject.Find("Canvas/InputField").GetComponent<InputField>();
		 stressBar = GameObject.Find("Canvas/Stress").GetComponent<Slider>();
		 audios = GetComponents<AudioSource>();

		 currentDay = 0;
		 taskIndex = 0;
		 dayTimer = new Timer(30f);
		 endOfDayTimer = new Timer(3f);
		 dayTimer.Reset();
		 taskTimer = new Timer(StepTimeBasedOnDay);

		 daysData = CSVParser.Parse("Data/days");
	}

	// search through texts, find one to decrease in size
	public void EnterText (string s) {
		for (int i = 0; i < circles.Length; i++) {
			if (circles[i].Text.Equals(s)) {
				circles[i].ChangeScalePercent(0.2f);
				audios[1].Play();
				break;
			}
		}
	}

	float StepTimeBasedOnDay {
		get {
			return Mathf.Max(2f - ((float)currentDay * 0.3f), 0.8f);
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

	string[] CurrentDayData {
		get {
			return daysData[Mathf.Min(currentDay, daysData.Length - 1)];
		}
	}

	string CurrentTask {
		get {
			return CurrentDayData[taskIndex];
		}
	}

	bool IsTaskRandomGenerated {
		get {
			string s = CurrentTask;
			return (s == "w" || s == "s" || 
							s == "m" || s == "e");
		}
		
	}

	// increase the scale of a sphere, based on request
	void ActivateTask () {
		int i = 0;
		string ct = CurrentTask;
		audios[1].Play();

		if (ct == "w") {
			i = (int)UnityEngine.Random.Range(9, 12);
		} else if (ct == "e") {
			i = (int)UnityEngine.Random.Range(16, 20);
		} else if (ct == "s") {
			i = (int)UnityEngine.Random.Range(4, 8);
		} else if (ct == "m") {
			i = (int)UnityEngine.Random.Range(12, 16);
		} else {
			i = System.Int32.Parse(ct);
		}
		circles[i].ChangeScalePercent(3f);
	}

	// check whether we need to activate new tasks
	void CheckTime () {
		if (!dayTimer.IsOffCooldown) {
		
			float p = dayTimer.PercentTimePassed;
			float[] thresholds = {0.03f, 0.0625f, 0.1f, 0.3125f, 0.3f, 0.625f, 0.7f, 0.8125f, 0.9f, 1f };

			// find whether we need to activate a new task
			bool newTask = false;
			for (int i = 0; i < thresholds.Length; i++) {
				if (taskIndex == i && p > thresholds[i]) {
					ActivateTask();
					taskIndex++;
					taskTimer.Reset();
					newTask = true;
					break;
				}
			}

			// only activate based on timer if we are randomly generating tasks
			if (!newTask && taskTimer.IsOffCooldown && 
					IsTaskRandomGenerated) {
				ActivateTask();
				taskTimer.Reset();
			}
		} else if (taskIndex < CurrentDayData.Length) { // straggler left over tasks
			ActivateTask();
			taskIndex++;
			taskTimer.Reset();
			endOfDayTimer.Reset();
		} else if (endOfDayTimer.IsOffCooldown) { // sleep period
			currentDay++;
			taskIndex = 0;
			dayTimer.Reset();
			taskTimer = new Timer(StepTimeBasedOnDay);
		}
	}


	void FixedUpdate () { 
		float area = Area;

		if (area > 4f) {
			stressBar.value += 0.1f;
		} else if (area > 2f) {
			stressBar.value += 0.05f;
		} else {
			stressBar.value -= 0.05f;
		}

		if (stressBar.value >= 100f) {
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
