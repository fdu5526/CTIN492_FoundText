using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Collider2D))]
public class Circle : MonoBehaviour {

	float scale;

	Rigidbody2D rigidbody2d;
	Collider2D collider2d;

	GameObject display;
	string text;

	// Use this for initialization
	void Start () {
		rigidbody2d = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<Collider2D>();
		scale = UnityEngine.Random.Range(0.1f, 1f);
		display = transform.Find("Display").gameObject;
		text = display.transform.Find("Text").GetComponent<Text>().text;
		transform.position = new Vector3(transform.position.x, transform.position.y, -scale);
		transform.localScale = new Vector2(scale, scale);
	}

	public float Area { get {return scale * scale; } }

	public string Text { get { return text; } }

	public void ChangeScalePercent (float percent) {
		scale *= percent;
		transform.localScale = new Vector2(scale, scale);
		transform.position = new Vector3(transform.position.x, transform.position.y, -scale);
	}
	 
	// Update is called once per frame
	void Update () {

	}
}
