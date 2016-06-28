using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelScripts : MonoBehaviour {

	private GameObject player1;
	private GameObject player2;

	private int player1RoundsWon = 0;
	private int player2RoundsWon = 0;

	private float restartTimer = -1;
	private float restartTime = 3;

	private GameObject playerWonText;

	private TextMesh countdownText;

	void Awake() {
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {
		player1 = GameObject.Find("Player1");
		player2 = GameObject.Find("Player2");
		playerWonText = GameObject.Find("PlayerWonText");
		countdownText = GameObject.Find("CountdownText").GetComponent<TextMesh>();
		player1.GetComponent<PlayerController>().CantMoveFor(3);
		player2.GetComponent<PlayerController>().CantMoveFor(3);
		StartCountdown();
	}
	
	// Update is called once per frame
	void Update () {

		//könnte auch über eine Coroutine gelöst werden, ohne "Timer"
		if (restartTimer >= restartTime) {
			PlayerToPositions();
		}

		if (restartTimer >= 0) {
			restartTimer += Time.deltaTime;
		}

		//Quit Game and Return to the Main Menu (zur Zeit der SplashScreen)
		/*if (Input.GetKey(KeyCode.Escape)) {
			SceneManager.LoadScene("splashScreen");
		}*/
	}

	public void RestartLevel() {
		restartTimer = 0;
	}

	void PlayerToPositions() {
		restartTimer = -1;
		playerWonText.GetComponent<TextMesh>().text = "";
		player1.active = true;
		player2.active = true;
		player1.transform.position = new Vector3(-7, -0.5f, 0);
		player1.transform.rotation = Quaternion.Euler(0, 90, 0);
		player1.GetComponent<PlayerController>().rigb.velocity = Vector3.zero;
		player1.GetComponent<PlayerController>().anim.Play("Idle");
		player1.GetComponent<PlayerController>().CantMoveFor(3);
		player2.transform.position = new Vector3(7, -0.5f, 0);
		player2.transform.rotation = Quaternion.Euler(0, 270, 0);
		player2.GetComponent<PlayerController>().rigb.velocity = Vector3.zero;
		player2.GetComponent<PlayerController>().anim.Play("Idle");
		player2.GetComponent<PlayerController>().CantMoveFor(3);
		StartCountdown();
	}

	public void SetRoundsWon(string playerWon) {
		Text wonRounds;
		int rounds;
		switch (playerWon) {
		case "Player 1":
			wonRounds = GameObject.Find("Player1RoundsWon").GetComponent<Text>();
			player1RoundsWon++;
			rounds = player1RoundsWon;
			break;
		case "Player 2":
			wonRounds = GameObject.Find("Player2RoundsWon").GetComponent<Text>();
			player2RoundsWon++;
			rounds = player2RoundsWon;
			break;
		default:
			rounds = 0;
			wonRounds = GameObject.Find("Player1RoundsWon").GetComponent<Text>();
			break;
		}
		wonRounds.text = "Rounds Won: " + rounds;

	}

	private void StartCountdown() {
		StartCoroutine(CoCountdown());
	}

	IEnumerator CoCountdown() {
		countdownText.text = "3";
		yield return new WaitForSeconds(1);
		countdownText.text = "2";
		yield return new WaitForSeconds(1);
		countdownText.text = "1";
		yield return new WaitForSeconds(1);
		countdownText.text = "GO";
		yield return new WaitForSeconds(1);
		countdownText.text = "";
	}
}
