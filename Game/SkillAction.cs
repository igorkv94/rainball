using UnityEngine;
using System.Collections;

public class SkillAction : MonoBehaviour
{

	void OnTriggerEnter2D (Collider2D other)
	{
		other.GetComponent<Circle> ().breakCircle ();
	}

	public void goTo (Vector2 position)
	{
		transform.position = position;
	}
}
