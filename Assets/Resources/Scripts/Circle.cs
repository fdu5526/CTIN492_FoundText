﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Collider2D))]
public class Circle : MonoBehaviour {

	float maxScale = 2f;
	float scale;
	string text;

	GameObject display;

	Rigidbody2D rigidbody2d;
	Collider2D collider2d;

	IEnumerator zoomCoroutine;

	// Use this for initialization
	void Start () {
		rigidbody2d = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<Collider2D>();
		scale = UnityEngine.Random.Range(0.2f, 0.25f);
		display = transform.Find("Display").gameObject;
		text = display.transform.Find("Text").GetComponent<Text>().text;
		transform.position = new Vector3(transform.position.x, transform.position.y, -scale);
		transform.localScale = new Vector2(scale, scale);
	}

	public float Area { get {return scale * scale; } }

	public string Text { get { return text; } }

	public void ChangeScalePercent (float percent) {
		if (zoomCoroutine != null) {
			StopCoroutine(zoomCoroutine);
		}
		float target = Mathf.Min(scale * percent, maxScale);
		zoomCoroutine = ChangeScale(target);
		StartCoroutine(zoomCoroutine);
	}



	IEnumerator ChangeScale (float target) {
		while (Mathf.Abs(scale - target) > 0.1f) {
			scale = Mathf.Lerp(scale, target, 0.2f);
			transform.localScale = new Vector2(scale, scale);
			transform.position = new Vector3(transform.position.x, transform.position.y, -scale);
			yield return new WaitForSeconds(0.01f);
		}
	}
	 
	// Update is called once per frame
	void Update () {

	}
}
