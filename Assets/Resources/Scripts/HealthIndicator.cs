using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthIndicator : MonoBehaviour {

	Animator animator;
	CanvasGroup cg;
	AudioSource[] audios;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		animator.SetBool("On", true);
		cg = GetComponent<CanvasGroup>();
		audios = GetComponents<AudioSource>();
	}


	public void SetTransparency (float t) {
		float a = 0f;
		if (t > 0.8f) {
			a = 1f;
		} else if (t > 0.5f) {
			a = t;
		}
		audios[0].volume = a * 0.4f;
		cg.alpha = a;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
