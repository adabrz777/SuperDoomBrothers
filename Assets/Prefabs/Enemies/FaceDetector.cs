using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDetector : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name.Contains("DownCollider") ||
			collision.gameObject.name.Contains("TopCollider") ||
			collision.gameObject.name.Contains("Player")
			)
			GameObject.Find("Player").GetComponent<PlayerControler>().deadFromEnemy = true;

		else if (!collision.gameObject.name.Contains("EndOfLevel"))
			transform.parent.GetComponent<Enemy>().changeDirection();
	}
	
}
