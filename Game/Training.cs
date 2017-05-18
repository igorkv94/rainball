/*using UnityEngine;
using System.Collections;

public class Training : MonoBehaviour {

	private bool trainMode = false;
	private GameObject showIt;
	private GameObject tapIt;
	private GameObject grats;
	private Data data;

	void Start () {
		data = GameObject.Find ("Data").GetComponent<Data> ();
		trainMode = data.needTraining ();
		grats = GameObject.Find ("Grats");
		if (!trainMode){
			count = Random.RandomRange (1, maxCount);
			GameObject.Destroy (grats);
		}
		else{
			count = 1;
			setArrow ();
			setTapToTag ();
			points.enabled = false;
			grats.SetActive(false);
		}
	}

	private void trainStep(){
		GameObject.Destroy (tapIt);
		GameObject.Destroy (showIt);
		if (step == 4) {
			skill = -1;
			for (int i = 0; i < circlesLength; i++)
				circles [i].GetComponent<Circle> ().SendMessage("setInteractable", false);
			counts [rightTagNum].enabled = false;
			grats.SetActive(true);
			data.trainingDone();
		}
		if (step == 3) {
			step++;
			firstSkill ();
		}
		if (step == 2) {
			step++;
			setTapToTag ();
			for (int i = 0; i < circlesLength; i++)
				circles [i].GetComponent<Circle> ().SendMessage("setInteractable", true);
		}
		if (step == 1) {
			circles [rightTagNum].transform.tag = "Loose";
			circles [rightTagNum].GetComponent<Circle> ().destr ();
			nextStep ();
			giveSkill.speed = 3;
			timerChoiceSlowdown = 1;
			giveSkill.Play ("rotate");
			timerChoiceDur = 3;
			timerChoice = timerChoiceDur;
			setTapToBuf ();
			for (int i = 0; i < circlesLength; i++)
				circles [i].GetComponent<Circle> ().SendMessage("setInteractable", false);
		}
		if (step == 0) {
			circles [rightTagNum].transform.tag = "Loose";
			circles [rightTagNum].GetComponent<Circle> ().destr ();
			nextStep ();
			setTapToTag ();
			setArrow ();
		}
	}

	private void setArrow(){
		showIt = GameObject.Instantiate (Resources.Load("prefabs/showIt") as GameObject);
		showIt.transform.parent = GameObject.Find ("Canvas").transform;
		showIt.transform.position = new Vector2(blocks [num].transform.position.x, blocks [num].transform.position.y-2*blocks [num].rectTransform.rect.height);
	}

	private void setTapToTag(){
		tapIt = GameObject.Instantiate (Resources.Load("prefabs/tapIt") as GameObject);
		tapIt.GetComponent<Button> ().onClick.AddListener (() => tapHand());
		tapIt.transform.parent = GameObject.Find ("Canvas").transform;
		tapIt.transform.position = circles [rightTagNum].transform.position;
	}

	private void setTapToBuf(){
		tapIt = GameObject.Instantiate (Resources.Load("prefabs/tapIt") as GameObject);
		tapIt.GetComponent<Button> ().interactable = false;
		tapIt.GetComponent<Button> ().onClick.AddListener (() => tapHand());
		tapIt.transform.parent = GameObject.Find ("Canvas").transform;
		tapIt.transform.position = giveSkill.transform.position;
	}
}*/
