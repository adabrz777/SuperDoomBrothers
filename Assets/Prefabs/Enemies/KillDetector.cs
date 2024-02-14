using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillDetector : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name.Contains("DownCollider"))
		{
			transform.parent.GetComponent<Enemy>().die(true);
			GameObject.Find("Player").GetComponent<PlayerControler>().jumpAfterKill();
		}
	}
	
}
