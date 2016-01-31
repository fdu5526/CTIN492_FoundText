using UnityEngine;
using System.Collections;

public class CirclesManager : MonoBehaviour {
	Circle[] circles;

	// Use this for initialization
	void Start () {
		 circles = GetComponentsInChildren<Circle>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
