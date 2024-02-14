using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDownDetector : MonoBehaviour
{
    private PlayerControler player;
    private BoxCollider2D myColider;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerControler>();
        myColider = GetComponentInParent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.marioVariant != 0)
		{
            myColider.offset = new Vector2(0f, 0.5f);
            myColider.size = new Vector2(0.6f, 1f);
        }
		else
		{
            myColider.offset = new Vector2(0f, 0.25f);
            myColider.size = new Vector2(0.6f, 0.5f);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
        player.DownDetected(collision);
	}

    

	private void OnCollisionExit2D(Collision2D collision)
	{
        player.DownLost(collision);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if(collision.gameObject.tag == "Ground")
            player.GroundDetected();
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            player.GroundLost();
    }

}
