using UnityEngine;
using UnityEngine.UI;

public class Data : MonoBehaviour
{
	private int points;
	private bool muteSound;
	private bool muteMusic;
	private bool onStart = true;

	public bool getStart ()
	{
		if (onStart) {
			onStart = false;
			return true;
		} else {
			return false;
		}
	}

	public void setPoints (int points1)
	{
		points = points1;
	}

	public int getPoints ()
	{
		return points;
	}

	public void setMuteSound ()
	{
		muteSound = !muteSound;
		PlayerPrefs.SetInt ("sound", muteSound ? 1 : 0);
		PlayerPrefs.Save ();
	}

	public bool getMuteSound ()
	{
		return muteSound;
	}

	public void setMuteMusic ()
	{
		muteMusic = !muteMusic;
		PlayerPrefs.SetInt ("music", muteMusic ? 1 : 0);
		PlayerPrefs.Save ();
	}

	public bool getMuteMusic ()
	{
		return muteMusic;
	}

	public void saveGame (int curPoints)
	{
		if (points < curPoints) {
			points = curPoints;
			PlayerPrefs.SetInt ("points", curPoints);
			PlayerPrefs.Save ();
		}
	}

	public void loadGame ()
	{
		points = PlayerPrefs.GetInt ("points");
	}

	public bool canLoad ()
	{
		if (PlayerPrefs.HasKey ("points"))
			return true;
		return false;
	}

	void Awake ()
	{
		DontDestroyOnLoad (transform.gameObject);
		muteSound = PlayerPrefs.GetInt ("sound") == 1 ? true : false; //Set the Sound from memory
		muteMusic = PlayerPrefs.GetInt ("music") == 1 ? true : false; //Set the Music from memory
	}
}

