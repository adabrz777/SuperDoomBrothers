using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Sprite OverworldSprite;
    public Sprite UndergroundSprite;
    public Sprite UnderwaterSprite;


    void Start()
    {
		switch (GameObject.Find("LevelInfo").GetComponent<LevelInfo>().mapVariant)
		{
            case LevelInfo.MapVariant.Overworld:
                transform.GetComponent<SpriteRenderer>().sprite = OverworldSprite;
                break;

            case LevelInfo.MapVariant.Underground:
                transform.GetComponent<SpriteRenderer>().sprite = UndergroundSprite;
                break;

            case LevelInfo.MapVariant.Underwater:
                transform.GetComponent<SpriteRenderer>().sprite = UnderwaterSprite;
                break;
        }
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.name.Contains("Player"))
		{
            GameObject.Find("Player").GetComponent<PlayerControler>().collectedCoins++;
            GameObject.Find("Player").GetComponent<MarioSoundsAndMusic>().playSound(MarioSoundsAndMusic.SoundNames.CoinSound);
            Destroy(gameObject);
        }
            
    }
}
