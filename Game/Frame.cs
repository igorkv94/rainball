using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SmartLocalization;

public class Frame : MonoBehaviour
{

	private const int circlesLength = 4;
	private const int blocksUILength = circlesLength;

	public int CirclesLength{ get { return circlesLength; } }

	public int BlocksUILength{ get { return blocksUILength; } }

	public int maxCount = 5;
	// less then 5, 1-4
	private const float timerTextAnimMax = 2;
	//Points for click
	private int[] point = new int[] { 6, 10 };
	//Points from buf
	public int bufPoint = 0;
	//for click, for next round
	public int count;
	//Count of the needed clicks
	public GameObject[] Circles{ get; private set; }
	//For tag of circles, set the rigth tag
	public int RightTagNum{ get; private set; }
	//Num of the right tag

	private Data data;
	private GameObject background;
	private SpriteRenderer[] circleParts;
	private List<Sprite> sprites;
	private List<Color> colors = new List<Color> () {
		new Color32 (237, 28, 36, 255),
		new Color32 (79, 185, 72, 255),
		new Color32 (61, 86, 161, 255),
		new Color32 (243, 236, 25, 255)
	};
	// Colors of the circles, sequence as in resourses folder
	private List<string> colorsName = new List<string> () { "red", "green", "blue", "yellow" };
	// Color's name of the circles, sequence as in resourses folder
	private Text[] counts;
	//UIBack count
	private Text rigthCount;
	//UIBack count text for rapid access
	private Text points;
	private Image[] blocks;
	//UIBack colors
	private int curPoints = 0;
	private int step = 0;
	//Number of destroyered circles
	private int num = 0;
	private float timerTextAnim = 0;
	private int fromSkillPoints = 0;

	private Skills skillScript;
	private BuffEffects buffEffects;

	void Start ()
	{
		LanguageManager languageManager = LanguageManager.Instance;
		SmartCultureInfo systemLanguage = languageManager.GetSupportedSystemLanguage ();
		if (systemLanguage != null) {
			languageManager.ChangeLanguage (systemLanguage);    
		}
		if (systemLanguage.languageCode == "ru" || systemLanguage.languageCode == "ru-RU") {
			RectTransform rt = (RectTransform)GameObject.Find ("BestScoreText").transform;
			rt.sizeDelta = new Vector2 (rt.rect.width, 26f);
		}			
		GameObject.Find ("YourScoreText").GetComponent<Text> ().text = languageManager.GetTextValue ("Score");
		GameObject.Find ("BestScoreText").GetComponent<Text> ().text = languageManager.GetTextValue ("BestScore");
		data = GameObject.Find ("Data").GetComponent<Data> ();
		buffEffects = GetComponent<BuffEffects> ();
		sprites = new List<Sprite> (Resources.LoadAll<Sprite> ("parts/"));
		Circles = GameObject.FindGameObjectsWithTag ("Loose").OrderBy (g => g.name).ToArray ();
		initialBorder ();
		setBackground ();
		skillScript = GetComponent<Skills> ();
		GameObject[] array = GameObject.FindGameObjectsWithTag ("Parts").OrderBy (g => g.transform.parent.name).ToArray ();
		circleParts = new SpriteRenderer[array.Length];
		int k = 0;
		foreach (GameObject part in array) {
			circleParts [k] = part.GetComponent<SpriteRenderer> ();
			k++;
		}
		GameObject obj = GameObject.Find ("RowColor");
		points = GameObject.Find ("Count").GetComponentInChildren<Text> ();
		points.text = curPoints.ToString ();
		blocks = new Image[blocksUILength];
		counts = new Text[blocksUILength];
		for (int i = 0; i < blocksUILength; i++) {
			blocks [i] = obj.GetComponentsInChildren<Image> () [i];
			blocks [i].transform.localScale = new Vector2 (blocks [i].transform.localScale.x, blocks [i].transform.localScale.x);
			counts [i] = obj.GetComponentsInChildren<Text> () [i];
			counts [i].enabled = false;
		}
		nextRound ();
		randSprites ();
		setRightTag ();
		rigthCount = counts [step];
		num = step % circlesLength;
		updCount ();
		rigthCount = counts [0];
	}


	private void nextRound ()
	{
		List<Color> colorsCur = new List<Color> (colors);
		List<string> colorsCurName = new List<string> (colorsName);
		int i = 0;
		while (colorsCur.Count > 0) {
			int p = Random.RandomRange (0, colorsCur.Count);
			blocks [i].color = colorsCur [p];
			blocks [i].name = colorsCurName [p];
			colorsCur.RemoveAt (p);
			colorsCurName.RemoveAt (p);
			i++;
		}
	}

	public void randSprites ()
	{
		List<Sprite> spritesCur = new List<Sprite> (sprites);
		List<string> spritesCurName = new List<string> (colorsName);
		int i = 0;
		int j = 0;
		while (spritesCur.Count > 0) {
			int p = Random.RandomRange (0, spritesCur.Count);
			circleParts [i].sprite = spritesCur [p];
			circleParts [i + 1].sprite = spritesCur [p];
			circleParts [i + 2].sprite = spritesCur [p];
			circleParts [i + 3].sprite = spritesCur [p];
			Circles [j].name = spritesCurName [p];
			spritesCur.RemoveAt (p);
			spritesCurName.RemoveAt (p);
			i = i + 4;
			j++;
		}
	}

	//Set the right tag
	public void setRightTag ()
	{
		int tagNum = 0;
		for (int i = 0; i < circlesLength; i++) {
			if (Circles [i].name.Equals (blocks [num].name)) {
				tagNum = i;
				break;
			}
		}
		Circles [tagNum].transform.tag = "Right";
		RightTagNum = tagNum;
	}

	public void addMaxPoints ()
	{
		fromSkillPoints += Mathf.RoundToInt (point [0] + bufPoint);
	}

	private void addPoints (int count)
	{
		if (((int)((curPoints + count) / 100)) > ((int)(curPoints / 100))) {
			skillScript.setSkill ();
		}
		curPoints += count;
		points.text = "+" + count;
		timerTextAnim = timerTextAnimMax;
	}

	public void addFromSkillPoints ()
	{
		addPoints (fromSkillPoints);
		fromSkillPoints = 0;
	}

	public void onClickCircle (float accuracy)
	{
		if (skillScript.Skill != 0) {
			count--;
			int points = point [0] + bufPoint;
			if (count == 0) {
				Circles [RightTagNum].GetComponent<Circle> ().destr ();
				nextStep ();
				addPoints (Mathf.RoundToInt (points * accuracy));
			} else {
				counts [step % circlesLength].text = count.ToString ();
				addPoints (Mathf.RoundToInt (points * accuracy));
			}
		} else {
			skillScript.firstSkill ();
			;
		}
	}

	private void nextStep ()
	{
		//Remove previous values
		rigthCount.enabled = false;
		Circles [RightTagNum].transform.tag = "Loose";
		if (skillScript.Skill == 2)
			skillScript.resetThirdSkill ();
		//next step
		step++;
		//Remove debuff on prev circle
		if (buffEffects.Debuff [1] && !Circles [RightTagNum].GetComponent<Circle> ().enabled) {
			buffEffects.firstDebuff ();            
		}
		num = step % circlesLength;
		setRightTag ();
		//end of the round
		if (num == 0) {
			buffEffects.unsetEffects ();
			buffEffects.chooseEffects ();
			nextRound (); 
			//Remove previous values
			rigthCount.enabled = false;
			Circles [RightTagNum].transform.tag = "Loose";
			setRightTag ();
			addPoints (point [1] + bufPoint);
			buffEffects.applySomeEffectsAfterRound ();
		}
		buffEffects.applySomeEffects ();
		updCount ();
		rigthCount = counts [num];
	}

	private void updCount ()
	{
		//upd text count
		if (buffEffects.Debuff [0])
			count = maxCount - 1;
		else if (buffEffects.Buff [0])
			count = 1;
		else
			count = Random.RandomRange (1, maxCount);
		counts [num].text = count.ToString ();
		counts [num].enabled = true;
	}

	private void showPointAnim ()
	{
		if (timerTextAnim > 0) {
			timerTextAnim -= Time.deltaTime;
			if (timerTextAnim < 1 && timerTextAnim > 0)
				points.color = new Color (0, 0, 0, timerTextAnim / timerTextAnimMax);
			else
				points.color = new Color (0, 0, 0, 1);
			if (timerTextAnim <= 0) {
				points.text = curPoints.ToString ();
				points.color = new Color (0, 0, 0, 1);
			}
		}
	}

	private void setBackground ()
	{
		background = GameObject.Find ("Background");
		SpriteRenderer sr = background.GetComponent<SpriteRenderer> ();

		background.transform.localScale = new Vector2 (1, 1);

		float width = sr.sprite.bounds.size.x;
		float height = sr.sprite.bounds.size.y;

		float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		background.transform.localScale = new Vector2 (worldScreenWidth / width, worldScreenHeight / height);
	}

	private void initialBorder ()
	{
		float sizeC = Circles [0].GetComponent<Circle> ().getSize ();
		Vector2 cameraSize = Camera.main.ScreenToWorldPoint (new Vector2 (Camera.main.pixelWidth, Camera.main.pixelHeight));
		float screenX = cameraSize.x + 2 * sizeC;
		float screenY = cameraSize.y + 2 * sizeC;
		addBorder (screenX + 40, 0, 40, screenY - 40);
		addBorder (-screenX - 40, 0, 40, screenY - 40);
		addBorder (0, screenY + 40, screenX, 40);
		addBorder (0, -screenY - 40, screenX, 40);
	}

	private void addBorder (float posX, float posY, float scaleX, float scaleY)
	{
		GameObject border = GameObject.Instantiate (Resources.Load ("prefabs/border") as GameObject);
		border.transform.localScale = new Vector2 (scaleX, scaleY);
		border.transform.position = new Vector2 (posX, posY);
	}

	void Update ()
	{
		showPointAnim ();
	}

	public void doublePoint ()
	{
		for (int i = 0; i < point.Length; i++) {
			point [i] *= 2;
		}
	}

	public void unsetDoublePoint ()
	{
		for (int i = 0; i < point.Length; i++) {
			point [i] /= 2;
		}
	}

	public int getCurPoints ()
	{
		return curPoints;
	}

	public void applyFourthSkill ()
	{
		if (buffEffects.Debuff [0])
		if (count > 1) {
			count = 1;
			counts [num].text = count.ToString ();
		}
	}

	public Color getBlockColor ()
	{
		return blocks [num].color;
	}
}
