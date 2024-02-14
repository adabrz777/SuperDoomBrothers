using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionMarkBlock : MonoBehaviour
{
    public bool playingAnimation = false;
    private bool beenOnTop = false;
    private Vector2 initialYPosition;
    private Vector2 finalYPosition;
    private BoxCollider2D myCollider;

    public int numberOfCoins = 1;
    public bool mushroom = false;
    public bool levelUp = false;

    public enum InitialLook { Normal, Bricks, Invisible };
    public InitialLook initialLook = InitialLook.Normal;

    public Sprite hittedSprite;
    public Sprite bricksSprite;
    private SpriteRenderer mySpriteRenderer;

    public GameObject BlockCoin;
    public GameObject RedMushroom;
    public GameObject GreenMushroom;

    private MarioSoundsAndMusic mySoundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        initialYPosition = new Vector2(transform.position.x, transform.position.y);
        finalYPosition = new Vector2(transform.position.x, initialYPosition.y + 0.5f);
        mySpriteRenderer = GetComponentInParent<SpriteRenderer>();
        myCollider = GetComponentInParent<BoxCollider2D>();

        mySoundPlayer = GameObject.Find("Player").GetComponent<MarioSoundsAndMusic>();

        if (initialLook == InitialLook.Bricks)
            mySpriteRenderer.sprite = bricksSprite;
        else if (initialLook == InitialLook.Invisible)
            mySpriteRenderer.sprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (playingAnimation)
            playAnimation();
    }

    private void doMyAction()
	{
        if (levelUp)
		{
            Instantiate(GreenMushroom);
            levelUp = false;
        }
        else if (mushroom)
		{
            Instantiate(RedMushroom, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            mushroom = false;
        }
        else if (numberOfCoins >= 1)
		{
            Instantiate(BlockCoin, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            numberOfCoins -= 1;

            GameObject.Find("Player").GetComponent<PlayerControler>().collectedCoins++;
            
        }
            


        if(numberOfCoins <= 0 && !mushroom && !levelUp)
		{
            mySpriteRenderer.sprite = hittedSprite;
		}
	}

    private void playAnimation()
    {
        if(mySpriteRenderer.sprite != hittedSprite)
		{
            float step = 5f * Time.deltaTime;


            if (!beenOnTop)
                transform.position = Vector2.MoveTowards(transform.position, finalYPosition, step);


            if (transform.position.y >= finalYPosition.y)
                beenOnTop = true;


            if (beenOnTop)
            {
                transform.position = Vector2.MoveTowards(transform.position, initialYPosition, step);
            }



            if (beenOnTop && transform.position.y <= initialYPosition.y)
            {
                playingAnimation = false;
                beenOnTop = false;

                doMyAction();
            }

            myCollider.offset = new Vector2(0f, initialYPosition.y - transform.position.y);
        }

    }

	
}
