using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name.Contains("Player"))
			GameObject.Find("Player").GetComponent<PlayerControler>().levelEnded = true;
		
	}



}
