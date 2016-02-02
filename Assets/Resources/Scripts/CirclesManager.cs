﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CirclesManager : MonoBehaviour {
	string [][] daysData;
	Timer dayTimer;
	Timer endOfDayTimer;
	int taskIndex;
	int prevRandTask;
	Timer taskTimer;
	int currentDay;

	Circle[] circles;
	InputField inputField;

	GameObject blackBackground;
	GameObject gameOver;
	GameObject pressEnter;
	GameObject gameStart;
	HealthIndicator healthIndicator;

	Slider stressBar;
	AudioSource[] audios;
	bool isGameOver;

	// Use this for initialization
	void Start () {
		 circles = GetComponentsInChildren<Circle>();
		 inputField = GameObject.Find("Canvas/InputField").GetComponent<InputField>();
		 stressBar = GameObject.Find("Canvas/Stress").GetComponent<Slider>();
		 audios = GetComponents<AudioSource>();

		 blackBackground = GameObject.Find("Canvas/BlackBackground");
		 gameOver = GameObject.Find("Canvas/BlackBackground/GameOver");
		 pressEnter = GameObject.Find("Canvas/BlackBackground/PressEnter");
		 gameStart = GameObject.Find("Canvas/BlackBackground/GameStart");
		 healthIndicator = GameObject.Find("Canvas/HealthIndicator").GetComponent<HealthIndicator>();

		 gameOver.SetActive(false);
		 isGameOver = true;
		 healthIndicator.SetTransparency(0f);

		 currentDay = 0;
		 taskIndex = 0;
		 dayTimer = new Timer(30f);
		 endOfDayTimer = new Timer(4f);
		 dayTimer.Reset();
		 taskTimer = new Timer(StepTimeBasedOnDay);
		 prevRandTask = 0;

		 daysData = CSVParser.Parse("Data/days");
	}

	// search through texts, find one to decrease in size
	public void EnterText (string s) {
		s = s.Trim();

		if (s.Length == 0) {
			return;
		}

		bool success = false;
		for (int i = 0; i < circles.Length; i++) {
			if (circles[i].Text.Equals(s)) {
				circles[i].ChangeScalePercent(0.2f);
				audios[3].Play();
				audios[1].Play();
				success = true;
				break;
			}
		}
		if (!success) {
			audios[4].Play();
		}

		// if working at night, increase stress
		if (dayTimer.IsOffCooldown && 
				!endOfDayTimer.IsOffCooldown &&
				!s.Equals("sleep")) {
			stressBar.value += 5f;
		}
	}

	float StepTimeBasedOnDay {
		get {
			return Mathf.Max(2f - ((float)currentDay * 0.35f), 0.8f);
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
		int i = -1;
		string ct = CurrentTask;
		audios[0].Play();

		if (ct == "w") {
			i = (int)UnityEngine.Random.Range(9, 12);
			while (i == prevRandTask) {
				i = (int)UnityEngine.Random.Range(9, 12);
			}
			prevRandTask = i;
		} else if (ct == "e") {
			i = (int)UnityEngine.Random.Range(16, 20);
			while (i == prevRandTask) {
				i = (int)UnityEngine.Random.Range(16, 20);
			}
			prevRandTask = i;
		} else if (ct == "s") {
			i = (int)UnityEngine.Random.Range(4, 8);
			while (i == prevRandTask) {
				i = (int)UnityEngine.Random.Range(4, 8);
			}
			prevRandTask = i;
		} else if (ct == "m") {
			i = (int)UnityEngine.Random.Range(12, 16);
			while (i == prevRandTask) {
				i = (int)UnityEngine.Random.Range(12, 16);
			}
			prevRandTask = i;
		} else {
			i = System.Int32.Parse(ct);
		}
		
		circles[i].ChangeScalePercent(3f);
	}

	// check whether we need to activate new tasks
	void CheckTime () {
		if (!dayTimer.IsOffCooldown) {
		
			float p = dayTimer.PercentTimePassed;
			float[] thresholds = {0.03f, 0.0625f, 0.1f, 0.25f, 0.3f, 0.45f, 0.6f, 0.7f, 0.8f, 0.9f };

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
		if (isGameOver) {

		} else {
			healthIndicator.SetTransparency(stressBar.value / 100f);
			float area = Area;
			if (area > 4f) {
				stressBar.value += 0.1f;
			} else if (area > 2f) {
				stressBar.value += 0.05f;
			} else {
				stressBar.value -= 0.05f;
			}

			if (stressBar.value >= 100f) {
				isGameOver = true;
				healthIndicator.SetTransparency(0f);
				blackBackground.SetActive(true);
				gameOver.SetActive(true);
				pressEnter.SetActive(true);
			} else {
				CheckTime();
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isGameOver) {
			if (Input.GetKeyDown(KeyCode.Return)) {
				// reset everything
				isGameOver = false;
				stressBar.value = 0f;
				blackBackground.SetActive(false);
				gameStart.SetActive(false);
				gameOver.SetActive(false);
				pressEnter.SetActive(false);
				currentDay = 0;
				taskIndex = 0;
				dayTimer.Reset();
				taskTimer = new Timer(StepTimeBasedOnDay);
				inputField.text = "";
				for (int i = 0; i < circles.Length; i++) {
					circles[i].Reset();
				}
			}
		} else {
			if (Input.GetKeyDown(KeyCode.Return)) {
				EnterText(inputField.text);
				inputField.text = "";
			}

			if (Input.anyKeyDown) {
				float p = UnityEngine.Random.Range(0.8f, 1.2f);
				audios[2].pitch = p;
				audios[2].Play();
			}

			inputField.Select();
	 		inputField.ActivateInputField();
		}
	}
}
