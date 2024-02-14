using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
    private float timeFromDie = 0f;
    private bool isGoombaDead = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkIfCanActivate();

		if (isActivated)
		{
            if (!isDead)
                walk();

            if (isGoombaDead)
                actionAfterDie();
        }



    }


    private void actionAfterDie()
	{
        timeFromDie += Time.deltaTime;
        if (timeFromDie >= 1f)
            Destroy(this.gameObject);
	}

    override public void afterDie()
	{
        transform.GetComponent<BoxCollider2D>().enabled = false;
        transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        isGoombaDead = true;

        GameObject.Find("Player").GetComponent<PlayerControler>().score += 100;
    }
}
