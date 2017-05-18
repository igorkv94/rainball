using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SmartLocalization;

public class PageControll : MonoBehaviour
{

	GameObject firstPage;
	GameObject secondPage;
	GameObject thirdPage;
	GameObject next;
	int page = 0;

	void Start ()
	{
		LanguageManager languageManager = LanguageManager.Instance;		
		SmartCultureInfo systemLanguage = languageManager.GetSupportedSystemLanguage ();
		if (systemLanguage != null) {
			languageManager.ChangeLanguage (systemLanguage);    
		}
		GameObject.Find ("Rules").GetComponent<Text> ().text = languageManager.GetTextValue ("Rules");
		GameObject.Find ("Goal").GetComponent<Text> ().text = languageManager.GetTextValue ("Goal");
		GameObject.Find ("Describe").GetComponent<Text> ().text = languageManager.GetTextValue ("Game Describe");
		GameObject.Find ("Positive").GetComponent<Text> ().text = languageManager.GetTextValue ("Positive");
		GameObject.Find ("DescribePos").GetComponent<Text> ().text = languageManager.GetTextValue ("PositiveDescr");
		GameObject.Find ("Negative").GetComponent<Text> ().text = languageManager.GetTextValue ("Negative");
		GameObject.Find ("DescribeNeg").GetComponent<Text> ().text = languageManager.GetTextValue ("NegativeDescr");
		firstPage = GameObject.Find ("FirstPage");
		secondPage = GameObject.Find ("SecondPage");
		thirdPage = GameObject.Find ("ThirdPage");
		next = GameObject.Find ("Next");
		secondPage.SetActive (false);
		thirdPage.SetActive (false);
	}

	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.Escape)) {
			toMenu ();
		}
	}

	public void nextPage ()
	{
		if (page == 0) {
			firstPage.SetActive (false);
			secondPage.SetActive (true);
			page = 1;
		} else {
			secondPage.SetActive (false);
			thirdPage.SetActive (true);
			next.SetActive (false);
			page = 2;
		}
	}

	public void prevPage ()
	{
		if (page == 2) {
			thirdPage.SetActive (false);
			secondPage.SetActive (true);
			next.SetActive (true);
			page = 1;
		} else {
			if (page == 1) {
				secondPage.SetActive (false);
				firstPage.SetActive (true);
				page = 0;
			} else {
				toMenu ();
			}
		}
	}

	public void toMenu ()
	{
		Application.LoadLevel ("Menu");
	}
}
