using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	public Texture splashScreen;
	public Texture whiteScreen;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			SceneManager.LoadScene("dgh_prototype");
		}
	}

	void OnGUI() {
		//Splashscreen
		//GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), whiteScreen);
		//GUI.DrawTexture(new Rect(Screen.width/2 - 150, Screen.height / 2 - 150, 300, 300), splashScreen);
	}
}
