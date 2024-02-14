using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableBricks : MonoBehaviour
{
    public bool playingAnimation = false;
    private bool beenOnTop = false;
    private Vector2 initialYPosition;
    private Vector2 finalYPosition;
    private BoxCollider2D myCollider;


    // Start is called before the first frame update
    void Start()
    {

        initialYPosition = new Vector2(transform.position.x, transform.position.y);
        finalYPosition = new Vector2(transform.position.x, initialYPosition.y + 0.5f);
        myCollider = GetComponentInParent<BoxCollider2D>();
        

    }

    // Update is called once per frame
    void Update()
    {
        if (playingAnimation)
            playAnimation();
    }

    private void playAnimation()
	{
        float step = 5f * Time.deltaTime;


        if(!beenOnTop)
            transform.position = Vector2.MoveTowards(transform.position, finalYPosition, step);


        if (transform.position.y >= finalYPosition.y)
            beenOnTop = true;


        if (beenOnTop)
		{
            transform.position = Vector2.MoveTowards(transform.position, initialYPosition, step);
        }
            
       

        if(beenOnTop && transform.position.y <= initialYPosition.y)
		{
            playingAnimation = false;
            beenOnTop = false;
        }

        if (transform.position.y < initialYPosition.y)
            transform.position = new Vector2(transform.position.x, initialYPosition.y);

        myCollider.offset = new Vector2(0f, initialYPosition.y - transform.position.y);
	}

    public void bigMarioHit()
	{
        Destroy(this.gameObject);
	}

	
}
