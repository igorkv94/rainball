using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuffEffects : MonoBehaviour
{
	public bool[] Debuff{ get; private set; }

	public bool[] Buff{ get; private set; }

	private const float timerDebuff1Max = 0.5f;
	private const float timerDebuff2Max = 2;
	private int oneBuff = -1;
	private float timerDebuff = 0;
	private float[] force;
	private GameObject background;
	private Frame frame;
	private int circlesLength;
	//for third buff
	private Text effectSum;

	#region buffs & debuffs

	/*
        Debuffs:
            0 - set all count of clicks on circle to maxCount for 1 round;
            1 - set the dissapeared effect on circle
            2 - set the effect of change colors
            3 - downscale circle
            4 - set the background to current color

        Buffs:
            0 - set all count of clicks on circle to 1 for 1 round;
            1 - reduce the speed for 1 round
            2 - 2x the score for 1 round
            3 - increase the score for clicks
    */

	#endregion

	void Awake ()
	{
		Debuff = new bool[5];
		Buff = new bool[4];		
	}

	// Use this for initialization
	void Start ()
	{
		unsetEffects ();
		background = GameObject.Find ("Background");
		frame = GetComponent<Frame> ();
		circlesLength = frame.CirclesLength;
		force = new float[circlesLength];
		effectSum = GameObject.Find ("EffectSum").GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update ()
	{
		checkDebuff ();	
	}

	public void chooseEffects ()
	{
		if (Random.Range (1, 100) < 51) {
			int k = Random.Range (1, 100) / 20;
			if (Random.Range (1, 100) < 51) {
				k /= 25;
				oneBuff = 1;
				Buff [k] = true;
			} else {
				k /= 20;
				oneBuff = 2;
				Debuff [k] = true;
			}
			Debug.Log ("buf " + k);
		} else {
			oneBuff = 0;
			int k = Random.Range (1, 100) / 25;
			int k1 = 0;
			if (k == 0)
				k1 = Random.Range (20, 100) / 20;
			else
				k1 = Random.Range (1, 100) / 20;

			Buff [k] = true;
			Debuff [k1] = true;
			Debug.Log ("bufs " + k + " " + k1);
		}
	}

	public void applySomeEffectsAfterRound ()
	{
		if (Buff [1]) {
			GameObject[] circles = frame.Circles;
			for (int i = 0; i < circlesLength; i++) {
				force [i] = circles [i].GetComponent<Circle> ().forceVal;
				circles [i].GetComponent<Circle> ().forceVal = force [i] * 0.3f;
				circles [i].GetComponent<Circle> ().resetForce (true);
			}
		}
		if (Buff [2]) {
			frame.doublePoint ();
			effectSum.text = "2×";
		}
		if (Buff [3]) {
			frame.bufPoint++;
			effectSum.text = frame.bufPoint + "";
		}
	}

	public void applySomeEffects ()
	{
		if (Debuff [3]) {
			frame.Circles [frame.RightTagNum].GetComponent<Animator> ().Play ("debuff");
		}
		if (Debuff [4]) {
			background.SetActive (false);
			Camera.main.backgroundColor = frame.getBlockColor ();
		}
	}


	public void unsetEffects ()
	{
		unsetNegative ();
		unsetPositive ();
	}

	public void unsetNegative ()
	{
		if (Debuff [1]) {
			if (!frame.Circles [frame.RightTagNum].GetComponent<Circle> ().enabled)
				debuffDisap ();
		}
		if (Debuff [2]) {
			for (int i = 0; i < frame.Circles.Length; i++) {
				frame.Circles [i].GetComponent<Circle> ().unsetSecDebuff ();
			}
		}
		if (Debuff [3]) {
			frame.Circles [frame.RightTagNum].GetComponent<Animator> ().Play ("default");
		}
		if (Debuff [4]) {
			background.SetActive (true);
			Camera.main.backgroundColor = Color.blue;
		}
		for (int i = 0; i < Debuff.Length; i++) {
			Debuff [i] = false;
		}

		timerDebuff = 0;
	}

	private void checkDebuff ()
	{
		if (Debuff [1]) {
			timerDebuff -= Time.deltaTime;
			if (timerDebuff <= 0) {
				debuffDisap ();
				timerDebuff = timerDebuff1Max;
			}
		}

		if (Debuff [2]) {
			timerDebuff -= Time.deltaTime;
			if (timerDebuff <= 0) {
				debuffChCol ();
				timerDebuff = timerDebuff2Max;
			}
		}
	}

	private void debuffDisap ()
	{
		bool check = frame.Circles [frame.RightTagNum].GetComponent<Circle> ().enabled;
		frame.Circles [frame.RightTagNum].GetComponent<Circle> ().enabled = !check;
		SpriteRenderer[] sprs = frame.Circles [frame.RightTagNum].GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer spr in sprs)
			spr.enabled = !check;
	}

	private void debuffChCol ()
	{
		frame.Circles [frame.RightTagNum].GetComponentsInChildren<SpriteRenderer> () [0].sprite = null;
		frame.Circles [frame.RightTagNum].transform.tag = "Loose";
		frame.randSprites ();
		frame.setRightTag ();
		for (int i = 0; i < frame.Circles.Length; i++) {
			frame.Circles [i].GetComponent<Circle> ().setSecDebuff ();
		}
	}


	private void unsetPositive ()
	{
		if (Buff [1]) {
			GameObject[] circles = frame.Circles;
			for (int i = 0; i < circlesLength; i++) { 
				circles [i].GetComponent<Circle> ().forceVal = force [i];
				circles [i].GetComponent<Circle> ().resetForce (true);
			}
		}
		if (Buff [2]) {
			frame.unsetDoublePoint ();
			int bufPoint = frame.bufPoint;
			if (bufPoint != 0)
				effectSum.text = frame.bufPoint + "";
			else
				effectSum.text = "";				
		}
		for (int i = 0; i < Buff.Length; i++) {
			Buff [i] = false;
		}

	}

	public void firstDebuff ()
	{
		timerDebuff = timerDebuff2Max;
		debuffDisap ();
	}
}
