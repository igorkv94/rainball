using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnManager : MonoBehaviour
{

	private Data data;
	private GameObject play;
	private GameObject gameLoading;
	private Animator animator;

	void Start ()
	{
		data = GameObject.Find ("Data").GetComponent<Data> ();
		if (data.canLoad ()) {
			data.loadGame ();
			GameObject.Find ("Count").GetComponent<Text> ().text = data.getPoints ().ToString ();
		} else
			GameObject.Find ("Count").GetComponent<Text> ().text = 0 + "";
		play = GameObject.Find ("Start");
		gameLoading = GameObject.Find ("GameLoading");
		gameLoading.SetActive (false);
		animator = GameObject.Find ("Canvas").GetComponent<Animator> ();
		if (data.getStart ()) {
			animator.Play ("onStart");
		}
	}

	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

	public void onGo ()
	{
		loadGame ();
	}

	private void loadGame ()
	{
		gameLoading.SetActive (true);
		animator.Play ("onGameLoad");
		StartCoroutine (asyncLoad ());
	}

	IEnumerator asyncLoad ()
	{
		var async = Application.LoadLevelAsync ("Game");
		while (!async.isDone) {
			yield return null;
		}
	}

	public void goToRules ()
	{
		Application.LoadLevel ("Rules");
	}
}

