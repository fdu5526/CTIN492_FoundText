using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Collider2D))]
public class Circle : MonoBehaviour {

	float scale;

	Rigidbody2D rigidbody2d;
	Collider2D collider2d;

	// Use this for initialization
	void Start () {
		rigidbody2d = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<Collider2D>();
		scale = transform.localScale.x;
	}


	void ChangeScale (float ds) {
		scale += ds;
		transform.localScale = new Vector2(scale, scale);
	}
	 
	// Update is called once per frame
	void Update () {
	}
}
