using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthIndicator : MonoBehaviour {

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}


	public void TurnOn () {
		animator.SetBool("On", true);
	}

	public void TurnOff () {
		animator.SetBool("On", false);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
