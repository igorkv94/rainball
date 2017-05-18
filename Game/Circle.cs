using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Circle : MonoBehaviour
{

	private BuffEffects buffEffects;
	public GameObject gameObject;
	public AudioClip clipCrash;
	public AudioClip clipCrash1;
	public AudioClip clipGameOver;
	private Vector2 curForce = new Vector2 (50, 50);
	public float forceVal = 10;
	//Value of the force
	private float modif = 1;
	//Add to force every time when the circle touched
	private int[] force = { 50, 50, -50, 50, 50, -50, -50, -50 };
	//Directon pf the force
	private SpriteRenderer foreground;
	private Sprite[] sprites;
	private AudioSource audio;
	private Data data;
	private Frame frame;
	private GameBtnManager gameBtnMan;
	private Rigidbody2D rb;
	private int clicked;
	private bool isLoose = false;
	private bool interactable = true;
	//Size of cirlce
	private float size;
	//UI space with Cirlce size
	private float uispacey;
	private float screenX;
	private float screenY;
	private float z;

	// Use this for initialization
	void Start ()
	{
		buffEffects = GameObject.Find ("ForwUI").GetComponent<BuffEffects> ();
		sprites = Resources.LoadAll<Sprite> ("CircleEffects/");
		data = GameObject.Find ("Data").GetComponent<Data> ();
		frame = GameObject.Find ("ForwUI").GetComponent<Frame> ();
		gameBtnMan = GameObject.Find ("ForwUI").GetComponent<GameBtnManager> (); 
		foreground = GetComponentsInChildren<SpriteRenderer> () [0];
		audio = transform.parent.GetComponent<AudioSource> ();
		rb = gameObject.GetComponent<Rigidbody2D> ();
		transform.Rotate (new Vector3 (0, 0, Random.RandomRange (0, 90)));
		resetForce (true);
		initialData ();
		z = this.transform.position.z;
	}

	private void initialData ()
	{
		size = gameObject.GetComponent<CircleCollider2D> ().bounds.size.x / 2;
		RectTransform img = GameObject.FindGameObjectWithTag ("UI").GetComponent<Image> ().rectTransform;
		Vector2 cameraSize = Camera.main.ScreenToWorldPoint (new Vector2 (Camera.main.pixelWidth, Camera.main.pixelHeight));
		screenX = cameraSize.x;
		screenY = cameraSize.y;
		uispacey = cameraSize.y - (img.rect.height * img.lossyScale.y);
		clicked = 0;
	}

	//Stop circle
	public void stopCircle ()
	{
		rb.velocity = new Vector2 (0, 0);
	}

	//Set force of the circle
	public void resetForce (bool generNew)
	{
		if (generNew) {
			int k = 2 * Random.Range (0, 4);
			curForce = new Vector2 (force [k], force [k + 1]) * forceVal;
		}
		rb.velocity = curForce;
	}

	/*public void setForce()
    {
        curForce = rb.velocity;
        Debug.Log(curForce+ " curForce");
    }*/

	//If touch the circle
	void OnMouseDown ()
	{
		if (interactable) {
			Vector3 clickedPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			if (clickedPosition.y <= uispacey)
			if (this.transform.tag.Equals ("Loose")) {
				isLoose = true;
				Debug.Log ("Loose" + this.name);
			} else if (this.transform.tag.Equals ("Right")) {
				Debug.Log ("Right");
				float distance = Mathf.Sqrt (Mathf.Pow (clickedPosition.x - transform.position.x, 2) +
				                 Mathf.Pow (clickedPosition.y - transform.position.y, 2));
				float accuracy = 1 - distance / size;
				if (accuracy > 0.65) {
					accuracy = 1;
				} else {
					if (accuracy > 0.32) {
						accuracy = 0.66f;
					} else {
						accuracy = 0.33f;
					}
				}

				clicked++;
				if (clicked < 4) {
					if (!data.getMuteSound ())
						audio.PlayOneShot (clipCrash1);
					if (!buffEffects.Debuff [2])
						foreground.sprite = sprites [clicked - 1];
				}
				frame.onClickCircle (accuracy);
				forceVal += modif;
				// modif *= modifDelt;
			}
		}
	}

	public void setInteractable (bool interactable)
	{
		this.interactable = interactable;
	}

	public void breakCircle ()
	{
		frame.addMaxPoints ();
		destr ();
	}

	void OnMouseUp ()
	{
		if (isLoose) {
			if (!data.getMuteSound ())
				audio.PlayOneShot (clipGameOver);
			gameBtnMan.gameOver ();
			isLoose = false;
		}
	}

	public void destr ()
	{
		if (!data.getMuteSound ())
			audio.PlayOneShot (clipCrash);
		foreground.sprite = null;
		clicked = 0;
		GetComponent<Animator> ().Play ("destroy");
		StartCoroutine (refresh ());
	}

	public void setSecDebuff ()
	{
		foreground.sprite = sprites [sprites.Length - 1];
		GetComponent<Animator> ().Play ("debuff2");
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
	}

	public void unsetSecDebuff ()
	{
		foreground.sprite = null;
		GetComponent<Animator> ().Play ("default");
		transform.Rotate (new Vector3 (0, 0, Random.RandomRange (0, 90)));
	}

	//After animation refresh position of the cirlce
	IEnumerator refresh ()
	{
		yield return new WaitForSeconds (1);
		transform.Rotate (new Vector3 (0, 0, Random.RandomRange (0, 90)));
		float x = 0;
		float y = 0;
		if (Random.Range (1, 100) < 51) {
			x = Random.Range (-screenX, screenX);
			y = (Random.Range (0, 1) * 2 - 1) * screenY;
		} else {
			x = (Random.Range (0, 1) * 2 - 1) * screenX;
			y = Random.Range (-screenY, screenY);
		}
		transform.position = new Vector3 (x, y, z);
		resetForce (true);
	}

	public float getSize ()
	{
		return size;
	}

}
		