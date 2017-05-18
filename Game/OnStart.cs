using UnityEngine;
using System.Collections;

public class OnStart : MonoBehaviour
{

	private Data data;
	private bool muteMusic;

	// Use this for initialization
	void Start ()
	{
		data = GameObject.Find ("Data").GetComponent<Data> ();
		muteMusic = data.getMuteMusic ();
		if (muteMusic)
			setMute ();
	}

	private void setMute ()
	{
		GameObject.Find ("Music").GetComponent<AudioSource> ().Stop ();
	}

	void setBackground ()
	{
		GameObject background = GameObject.Find ("Background");
		
		SpriteRenderer sr = background.GetComponent<SpriteRenderer> ();

		background.transform.localScale = new Vector2 (1, 1);

		float width = sr.sprite.bounds.size.x;
		float height = sr.sprite.bounds.size.y;

		float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		background.transform.localScale = new Vector2 (worldScreenWidth / width, worldScreenHeight / height);
		GameObject wall = GameObject.Find ("Wall");
		sr = wall.GetComponent<SpriteRenderer> ();

		wall.transform.localScale = new Vector2 (1, 1);

		width = sr.sprite.bounds.size.x;
		height = sr.sprite.bounds.size.y;

		worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
		float mod = 0.45f * worldScreenHeight / height;
		wall.transform.localScale = new Vector2 ((worldScreenWidth / width) + mod, (worldScreenHeight / height) + mod);
	}
}
