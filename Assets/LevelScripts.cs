using UnityEngine;
using System.Collections;

public class LevelScripts : MonoBehaviour {

	private GameObject player1;
	private GameObject player2;

	private float restartTimer = -1;
	public float restartTime = 3;

	private GameObject playerWonText;

	// Use this for initialization
	void Start () {
		player1 = GameObject.Find("Player1");
		player2 = GameObject.Find("Player2");
		playerWonText = GameObject.Find("PlayerWonText");
	}
	
	// Update is called once per frame
	void Update () {
		if (restartTimer >= restartTime) {
			PlayerToPositions();
		}

		if (restartTimer >= 0) {
			restartTimer += Time.deltaTime;
		}
	}

	public void RestartLevel() {
		print("yeah");
		restartTimer = 0;
	}

	void PlayerToPositions() {
		restartTimer = -1;
		playerWonText.GetComponent<TextMesh>().text = "";
		player1.active = true;
		player2.active = true;
		player1.transform.position = new Vector3(-7, -0.5f, 0);
		player1.transform.rotation = Quaternion.Euler(0, 0, 0);
		player2.transform.position = new Vector3(7, -0.5f, 0);
		player2.transform.rotation = Quaternion.Euler(0, 180, 0);
	}
}
