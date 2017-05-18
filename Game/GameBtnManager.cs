using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameBtnManager : MonoBehaviour
{

	private GameObject fail;
	private GameObject menu;
	private GameObject pause;
	private Frame frame;
	private Skills skillScript;
	private Text curScore;
	private Text bestScore;
	private Data data;

	void Start ()
	{
		curScore = GameObject.Find ("curScore").GetComponent<Text> ();
		bestScore = GameObject.Find ("bestScore").GetComponent<Text> ();
		fail = GameObject.Find ("GameOver"); 
		fail.SetActive (false);
		menu = GameObject.Find ("Menu");
		menu.SetActive (false);
		pause = GameObject.Find ("Pause");
		data = GameObject.Find ("Data").GetComponent<Data> ();
		frame = GetComponent<Frame> ();
		skillScript = GetComponent<Skills> ();
	}

	void Update ()
	{

		checkReturn ();
	
	}

	private void checkReturn ()
	{
		if (Input.GetKeyUp (KeyCode.Escape)) {
			if (!fail.activeSelf)
			if (menu.activeSelf)
				onGo ();
			else
				onMenu ();
			else
				onExit ();
		}
	}

	public void onMenu ()
	{
		pause.SetActive (false);
		menu.SetActive (true);
		GameObject[] circles = frame.Circles;
		for (int k = 0; k < circles.Length; k++)
			circles [k].SetActive (false);
	}

	public void onGo ()
	{
		menu.SetActive (false);
		pause.SetActive (true);
		GameObject[] circles = frame.Circles;
		for (int k = 0; k < circles.Length; k++) {
			circles [k].SetActive (true);
			circles [k].GetComponent<Circle> ().resetForce (false);
		}
	}

	public void gameOver ()
	{
		fail.SetActive (true);
		bestScore.text = data.getPoints () + "";
		saveGame ();
		curScore.text = frame.getCurPoints () + "";
		GameObject[] circles = frame.Circles;
		for (int k = 0; k < circles.Length; k++)
			circles [k].SetActive (false);
		if (skillScript.Skill == 2)
			skillScript.resetThirdSkill ();
	}

	public void onExit ()
	{
		saveGame ();
		Application.LoadLevel ("Menu");
	}

	public void onRestart ()
	{
		saveGame ();
		Application.LoadLevel ("Game");
	}

	private void saveGame ()
	{
		data.saveGame (frame.getCurPoints ());
	}
}
