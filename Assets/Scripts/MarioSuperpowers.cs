using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarioSuperpowers : MonoBehaviour
{
    private PlayerControler myPlayer;
    private float timeOfGrowing = 0f;
    private float timeOfShrinking = 0f;

    private float timeOfEnteringToPipe = 0f;
    private float timeOfExitingThePipe = 0f;

    private float accelerationHorizontal = 0f;
    private float velocityVertical = 0f;


    // Start is called before the first frame update
    void Start()
    {
        myPlayer = GetComponentInParent<PlayerControler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeOfGrowing > 0)
            getBig();

        if (timeOfShrinking > 0)
            getSmall();

        if (timeOfEnteringToPipe > 0)
            getInPipe();

        if (timeOfExitingThePipe > 0)
            getOutPipe();



    }

    public void grow()
	{
        if(myPlayer.marioVariant == 0)
		{
            myPlayer.myAudioPlayer.playSound(MarioSoundsAndMusic.SoundNames.PowerUpSound);
            timeOfGrowing = 0.8f;
            myPlayer.canMove = false;
            myPlayer.marioVariant = PlayerControler.MarioVariantNames.Growing;
            accelerationHorizontal = myPlayer.accelerationHorizontal;
            velocityVertical = myPlayer.velocityVertical;

            myPlayer.accelerationHorizontal = 0;
            myPlayer.velocityVertical = 0;
		}
		else
		{
            myPlayer.score += 200;
            myPlayer.myAudioPlayer.playSound(MarioSoundsAndMusic.SoundNames.KickSound);
        }
        
    }

    public void shrink()
	{
        Physics.IgnoreLayerCollision(3, 6);
        myPlayer.canBeKilled = false;

        myPlayer.myAudioPlayer.playSound(MarioSoundsAndMusic.SoundNames.PowerUpSound);
        timeOfShrinking = 2f;
        myPlayer.canMove = false;
        myPlayer.marioVariant = PlayerControler.MarioVariantNames.Growing;
        accelerationHorizontal = myPlayer.accelerationHorizontal;
        velocityVertical = myPlayer.velocityVertical;

        myPlayer.accelerationHorizontal = 0;
        myPlayer.velocityVertical = 0;
    }

    private void getSmall()
	{
        timeOfShrinking -= Time.deltaTime;
        
        myPlayer.myRigidbody.gravityScale = 0f;

        if(timeOfShrinking > 1.5f)
            myPlayer.myRigidbody.velocity = new Vector2(0f, 0f);

        if (timeOfShrinking <= 1.5f)
        {
            
            myPlayer.marioVariant = PlayerControler.MarioVariantNames.Default;
            
            //myPlayer.accelerationHorizontal = accelerationHorizontal;
            //myPlayer.velocityVertical = velocityVertical;

            myPlayer.canMove = true;

            myPlayer.myRigidbody.gravityScale = 3.6f;

        }

        if(timeOfShrinking <= 0f)
		{
            myPlayer.canBeKilled = true;
            timeOfShrinking = 0;
            Physics.IgnoreLayerCollision(3, 6, false);
        }
    }

    private void getBig()
	{
        timeOfGrowing -= Time.deltaTime;
        myPlayer.myRigidbody.velocity = new Vector2(0f, 0f);
        myPlayer.myRigidbody.gravityScale = 0f;

        if (timeOfGrowing <= 0f)
		{
            myPlayer.marioVariant = PlayerControler.MarioVariantNames.Big;
            timeOfGrowing = 0;
            myPlayer.accelerationHorizontal = accelerationHorizontal;
            myPlayer.velocityVertical = velocityVertical;

            myPlayer.canMove = true;

            myPlayer.myRigidbody.gravityScale = 3.6f;

        }
    }

    public void pipeIn()
	{
        if(myPlayer.dataContainer.pipe_toWorld != "")
		{
            myPlayer.dataContainer.playerMarioVariant = (int)myPlayer.marioVariant;
            myPlayer.dataContainer.playerCoins = myPlayer.collectedCoins;
            


            myPlayer.myRigidbody.bodyType = RigidbodyType2D.Kinematic;
            myPlayer.myRigidbody.gravityScale = 0;
            myPlayer.GetComponent<SpriteRenderer>().sortingOrder = 0;
            myPlayer.canMove = false;
            

            if (myPlayer.dataContainer.pipe_pipeLay == (int)PipeObject.PipeLayNames.Horizontal)
			{
                myPlayer.myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
                myPlayer.myRigidbody.velocity = new Vector2(2f, 0);
            }
			else
			{
                myPlayer.myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
                myPlayer.myRigidbody.velocity = new Vector2(0, -2f);

			}

            timeOfEnteringToPipe = 1f;
            myPlayer.myAudioPlayer.playSound(MarioSoundsAndMusic.SoundNames.PipeSound);

        }
        
	}



    private void getInPipe()
	{
        timeOfEnteringToPipe -= Time.deltaTime;

        if(timeOfEnteringToPipe <= 0)
		{
            SceneManager.LoadScene(myPlayer.dataContainer.pipe_toWorld);
            timeOfEnteringToPipe = 0;
            
        }
	}

    public void pipeOut()
    {
        

        if (myPlayer.dataContainer.pipe_pipeType == (int)PipeObject.PipeTypeNames.toPipe)
        {
            myPlayer.myRigidbody.bodyType = RigidbodyType2D.Kinematic;
            myPlayer.myRigidbody.gravityScale = 0;
            myPlayer.GetComponent<SpriteRenderer>().sortingOrder = 1;
            myPlayer.canMove = false;

            PipeObject directedPipe = GameObject.FindGameObjectWithTag("Pipe").GetComponent<PipeObject>();

            GameObject[] PipesInScene = GameObject.FindGameObjectsWithTag("Pipe");

            for(int i = 0; i < PipesInScene.Length; i++)
			{
                Debug.Log("Znalaz³em na tej scenie takie rury: " + PipesInScene[i].GetComponent<PipeObject>().pipeNr);

                if (PipesInScene[i].GetComponent<PipeObject>().pipeNr == myPlayer.dataContainer.pipe_toPipeNr)
                    directedPipe = PipesInScene[i].GetComponent<PipeObject>();
            }
                

            
            myPlayer.transform.position = new Vector2(directedPipe.gameObject.transform.position.x, directedPipe.gameObject.transform.position.y - 1f);
            GameObject.Find("Main Camera").transform.position = new Vector3(myPlayer.transform.position.x, GameObject.Find("Main Camera").transform.position.y, -10f);

            if(directedPipe.pipeLay == PipeObject.PipeLayNames.Horizontal)
            {
                myPlayer.myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionY;
                myPlayer.myRigidbody.velocity = new Vector2(-3f, 0f);
            }
            else
            {
                myPlayer.myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
                myPlayer.myRigidbody.velocity = new Vector2(0, 3f);

            }

            timeOfExitingThePipe = 0.5f;
            myPlayer.myAudioPlayer.playSound(MarioSoundsAndMusic.SoundNames.PipeSound);
        }
    }

    private void getOutPipe()
	{
        timeOfExitingThePipe -= Time.deltaTime;

        if (timeOfExitingThePipe <= 0)
        {
            timeOfExitingThePipe = 0;

            myPlayer.myRigidbody.constraints = RigidbodyConstraints2D.None;
            myPlayer.myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;


            myPlayer.myRigidbody.bodyType = RigidbodyType2D.Dynamic;
            myPlayer.myRigidbody.gravityScale = 3.6f;
            myPlayer.GetComponent<SpriteRenderer>().sortingOrder = 5;
            myPlayer.canMove = true;

        }
    }

}
