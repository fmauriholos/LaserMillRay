using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.SceneManagement;

public class ChangeLayout : MonoBehaviour {
	public GameObject landscapeCanvas;
	public GameObject portraitCanvas;

	private ScreenOrientation lastOrientation;
	void Start(){
		UnityEngine.XR.XRSettings.enabled = false;
	}
	// Update is called once per frame
	void Update () {
		
		if (Screen.orientation == ScreenOrientation.Landscape && lastOrientation != ScreenOrientation.Landscape) {
			
			portraitCanvas.SetActive (false);
			landscapeCanvas.SetActive (true);
			lastOrientation = ScreenOrientation.Landscape;
		} else if (Screen.orientation == ScreenOrientation.Portrait && lastOrientation != ScreenOrientation.Portrait){
			portraitCanvas.SetActive (true);
			landscapeCanvas.SetActive (false);
			lastOrientation = ScreenOrientation.Portrait;
		}
	}
	public void LoadFirst(){
		SceneManager.LoadScene (1);
	}
}
