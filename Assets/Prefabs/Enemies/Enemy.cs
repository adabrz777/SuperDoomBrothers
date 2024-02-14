using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	public Rigidbody2D myRigidbody;
	public Animator myAnimator;
	public virtual int facing { get; set; } = -1;
	public virtual bool isDead { get; set; }  = false;

	public virtual bool isActivated { get; set; } = false;
	public virtual bool walkingEnemy { get; set; } = true;



	public abstract void afterDie();



	public void changeDirection() {
		facing *= -1;
	}

	public virtual void die(bool killedByJump)
	{
		
		myAnimator = transform.GetComponent<Animator>();
		myAnimator.SetBool("dead", true);

		GameObject.Find("Player").GetComponent<MarioSoundsAndMusic>().playSound(MarioSoundsAndMusic.SoundNames.KickSound);

		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		if (!isDead)
			afterDie();
		
		isDead = true;
	}

	protected void walk()
	{
		myRigidbody = transform.GetComponent<Rigidbody2D>();
		myRigidbody.velocity = new Vector2(facing * 2.5f, myRigidbody.velocity.y);

		
	}


	public void checkIfCanActivate()
	{
		if (Mathf.Abs(transform.position.x - GameObject.Find("Player").transform.position.x) <= 13f)
			isActivated = true;
	}












	protected virtual void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.name.Contains("RollingObject"))
			die(false);
	}



	
}
