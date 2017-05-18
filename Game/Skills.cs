using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Skills : MonoBehaviour
{
	//GiveSkill rotate
	private const float timerChoiceSpeedM = 4;
	private const float timerChoiceSpeedm = 3;
	private const float timerChoiceDurM = 3.5f;
	private const float timerChoiceDurm = 2.5f;
	private const float timerChoiceSlowdownM = 2;
	private const float timerChoiceSlowdownm = 1;
	private float timerChoiceSpeed;
	private float timerChoiceDur;
	private float timerChoiceSlowdown;
	private float timerChoice = 0;

	private float timerForThSkill = 3;
	private float timerForSecSkill = 1f;
	private float timerForFourSkill = 1f;
	private float timerForFirstSkill = 1f;
	private float timerForFirstSkillApp = 5f;
	private float timerForSkill = 0;

	private Animator giveSkill;
	private GameObject bufBut;
	private GameObject skillTimerObj;
	private Slider skillTimerSlider;
	List<GameObject> objs = new List<GameObject> ();
	private BuffEffects bufEffects;
	private Frame frame;

	public int Skill { get; private set; }

	// Use this for initialization
	void Start ()
	{
		Skill = -1;
		giveSkill = GameObject.Find ("Point").GetComponent<Animator> ();
		bufBut = GameObject.Find ("BufBut");
		skillTimerObj = GameObject.Find ("SkillTimer");
		frame = GetComponent<Frame> ();
		skillTimerSlider = skillTimerObj.GetComponentInChildren<Slider> ();
		skillTimerObj.SetActive (false);
		bufEffects = GetComponent<BuffEffects> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		showSkillTimer ();
		showSkillChoice ();
	}

	public void setSkill ()
	{
		timerChoiceSpeed = Random.Range (timerChoiceSpeedm, timerChoiceSpeedM);
		giveSkill.speed = timerChoiceSpeed;
		timerChoiceSlowdown = Random.Range (timerChoiceSlowdownm, timerChoiceSlowdownM);
		giveSkill.Play ("rotate");
		timerChoiceDur = Random.Range (timerChoiceDurm, timerChoiceDurM);
		timerChoice = timerChoiceDur;
	}

	private GameObject firSkillEffect;

	public void firstSkill ()
	{
		Skill = -1;
		skillTimerObj.SetActive (false);
		timerForSkill = 0;
		GameObject[] circles = frame.Circles;
		for (int i = 0; i < circles.Length; i++)
			circles [i].GetComponent<Circle> ().setInteractable (false);
		Vector2 position = circles [frame.RightTagNum].transform.position;
		firSkillEffect = GameObject.Instantiate (Resources.Load ("prefabs/firstSkill") as GameObject);
		firSkillEffect.GetComponent<SkillAction> ().goTo (position);
		firSkillEffect.GetComponent<ParticleSystem> ().Play ();
		StartCoroutine (resetFirstSkill ());
	}

	private IEnumerator resetFirstSkill ()
	{
		yield return new WaitForSeconds (timerForFirstSkill);
		firSkillEffect.GetComponent<ParticleSystem> ().Stop ();
		frame.addFromSkillPoints ();
		GameObject[] circles = frame.Circles;
		for (int i = 0; i < circles.Length; i++)
			circles [i].GetComponent<Circle> ().setInteractable (true);
		Destroy (firSkillEffect);
	}

	private void secondSkill ()
	{	
		Skill = -1;
		Vector2 cameraSize = Camera.main.ScreenToWorldPoint (new Vector2 (Camera.main.pixelWidth, Camera.main.pixelHeight));
		float x = cameraSize.x;
		float y = cameraSize.y;
		Vector2[] position = new Vector2[] {	new Vector2 (Random.Range (0, x), Random.Range (0, y)),
			new Vector2 (Random.Range (0, x), Random.Range (-y, 0)),
			new Vector2 (Random.Range (-x, 0), Random.Range (-y, 0)),
			new Vector2 (Random.Range (-x, 0), Random.Range (0, y))
		};		
		for (int i = 0; i < 4; i++) {
			objs.Add (GameObject.Instantiate (Resources.Load ("prefabs/secondSkill") as GameObject));
			objs [i].GetComponent<CircleCollider2D> ().radius = 120;
			objs [i].SetActive (true);
			objs [i].GetComponent<ParticleSystem> ().Play ();
			objs [i].GetComponent<SkillAction> ().goTo (position [i]);
		}
		StartCoroutine (resetSecondSkill ());
	}

	private IEnumerator resetSecondSkill ()
	{
		yield return new WaitForSeconds (timerForSecSkill);
		frame.addFromSkillPoints ();
		foreach (GameObject obj in objs) {
			Destroy (obj);
		}
		objs.Clear ();
	}

	private void thirdSkill ()
	{
		GameObject[] circles = frame.Circles;
		for (int k = 0; k < circles.Length; k++) {			
			objs.Add (GameObject.Instantiate (Resources.Load ("prefabs/thirdSkill") as GameObject));
			objs [k].transform.position = new Vector2 (circles [k].transform.position.x, circles [k].transform.position.y - 40.8f);
			objs [k].GetComponent<SpriteRenderer> ().sortingLayerName = "Layer " + (k + 1);
			objs [k].SetActive (true);
			circles [k].GetComponent<Circle> ().stopCircle ();
		}
		skillTimerObj.SetActive (true);
		timerForSkill = timerForThSkill;
	}

	public void resetThirdSkill ()
	{
		Skill = -1;
		timerForSkill = 0;
		GameObject[] circles = frame.Circles;
		for (int k = 0; k < circles.Length; k++) {			
			Destroy (objs [k]);
			circles [k].GetComponent<Circle> ().resetForce (false);
		}
		objs.Clear ();
	}

	private void showSkillTimer ()
	{
		if (timerForSkill >= 0) {
			timerForSkill -= Time.deltaTime;
			if (timerForSkill > 0) {
				if (Skill == 2)
					skillTimerSlider.value = timerForSkill / timerForThSkill;
				if (Skill == 0)
					skillTimerSlider.value = timerForSkill / timerForFirstSkillApp;
			} else {
				skillTimerObj.SetActive (false);
				if (Skill == 2)
					resetThirdSkill ();
				if (Skill == 0)
					Skill = -1;					
			}
		}
	}

	private GameObject fourSkillEffect;

	private void fourthSkill ()
	{
		Skill = -1;
		fourSkillEffect = GameObject.Instantiate (Resources.Load ("prefabs/fourthSkill") as GameObject);
		fourSkillEffect.GetComponent<ParticleSystem> ().Play ();
		frame.applyFourthSkill ();
		bufEffects.unsetNegative ();
		StartCoroutine (resetFourthSkill ());
	}

	private IEnumerator resetFourthSkill ()
	{
		yield return new WaitForSeconds (timerForFourSkill);
		fourSkillEffect.GetComponent<ParticleSystem> ().Stop ();
		Destroy (fourSkillEffect);
	}

	public void skillApply ()
	{
		bufBut.GetComponent<Button> ().interactable = false;
		Skill = (int)(bufBut.transform.localEulerAngles.z / 90);
		if (Skill == 0) {
			skillTimerObj.SetActive (true);
			timerForSkill = timerForFirstSkillApp;
		}
		if (Skill == 1) {
			secondSkill ();
		}
		if (Skill == 2) {
			thirdSkill ();
		}
		if (Skill == 3) {
			fourthSkill ();
		}

	}

	private void showSkillChoice ()
	{
		if (timerChoice > 0) {
			timerChoice -= Time.deltaTime;
			if (timerChoice < timerChoiceSlowdown && timerChoice > 0) {
				giveSkill.speed = timerChoiceSpeed * timerChoice / timerChoiceDur;
			}
			if (timerChoice <= 0) {
				giveSkill.speed = 0;
				bufBut.GetComponent<Button> ().interactable = true;
			}
		}
	}
}
