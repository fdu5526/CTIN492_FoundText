using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Collider2D))]
public class Circle : MonoBehaviour {

	Rigidbody2D rigidbody2d;
	Collider2D collider2d;

	// Use this for initialization
	void Start () {
		rigidbody2d = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
