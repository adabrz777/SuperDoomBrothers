using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    public bool playingAnimation = false;
    private bool beenOnTop = false;
    private Vector2 initialYPosition;
    private Vector2 finalYPosition;

    // Start is called before the first frame update
    void Awake()
    {
        initialYPosition = new Vector2(transform.position.x, transform.position.y);
        finalYPosition = new Vector2(transform.position.x, initialYPosition.y + 3f);

        playingAnimation = true;

        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (playingAnimation)
            playAnimation();
    }

    private void playAnimation()
    {
        float step = 12f * Time.deltaTime;


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

            Destroy(this.gameObject);
        }
    }
}
