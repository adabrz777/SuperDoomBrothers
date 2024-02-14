using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControler : MonoBehaviour
{
	// DO USTAWIENIA WIDZIMISIĘ
	public float WalkingSpeed = 6.0f;
	public float TimeToMaxAcceleration = 0.3f;
	public float Friction = 1.2f;
	public float SprintMultiplier = 1.2f;
	public float jumpHeight = 0f;

	// AKTUALNE WARTOŚCI
	private float SprintMultiplierActual = 1f;
	public int isGrounded = 1;
	public bool canMove = true;
	public bool levelEnded = false;
	private bool touchedPole = false;
	public bool canBeKilled = true;

	private bool playerHuried = false;

	private PipeObject touchingPipe = null;



	// MARIO DIE
	public bool isDead = false;
	private float deadFor = 0f;
	private bool deadFromHole = false;
	public bool deadFromEnemy = false;

	private Vector2 initialYPosition;
	private Vector2 finalYPosition;

	public bool beenOnTop = false;




	// WARTOŚCI DLA MARIOSUPERPOWERS

	//INFO DLA UI
	public TMP_Text Mario_txt;
	public TMP_Text Coins_txt;
	public TMP_Text World_txt;
	public TMP_Text Time_txt;
	public TMP_Text Lives_txt;

	// INFO O POZIOMIE
	private string levelName;
	private float timeLeft;
	public int collectedCoins = 0;
	public int score = 0;


	// RUCH
	public float accelerationHorizontal = 0f;
	public float velocityHorizontal = 0f;
	public float velocityVertical = 0f;

	private bool canBump = true;

	private float jumpAfterKillForce = 0f;

	// RELATIONS
	public DataContainer dataContainer;

	public MarioSoundsAndMusic myAudioPlayer;

	private Animator myAnimator;
	public Rigidbody2D myRigidbody;

	public enum MarioVariantNames { Default, Big, White, Dead, Growing };
	public MarioVariantNames marioVariant;
	public enum AnimationNames { Idle, Walk, Run, Stop, Jump, Swim, SwimUp, Climbing, Crouch, Throw };
	public AnimationNames animationNr = AnimationNames.Idle;

	private string filePath;

	void Start()
	{
		if(dataContainer.pipe_toWorld != "")
		{
			GetComponentInParent<MarioSuperpowers>().pipeOut();
			dataContainer.pipe_toWorld = "";
		}


		myRigidbody = GetComponentInParent<Rigidbody2D>();
		myAnimator = GetComponentInParent<Animator>();
		myAudioPlayer = GetComponentInParent<MarioSoundsAndMusic>();


		switch (GameObject.Find("LevelInfo").GetComponent<LevelInfo>().mapVariant)
		{
			case LevelInfo.MapVariant.Overworld:
				myAudioPlayer.playMusic(MarioSoundsAndMusic.MusicNames.Overworld);
				break;

			case LevelInfo.MapVariant.Underground:
				myAudioPlayer.playMusic(MarioSoundsAndMusic.MusicNames.Underground);
				break;

			case LevelInfo.MapVariant.Underwater:
				myAudioPlayer.playMusic(MarioSoundsAndMusic.MusicNames.Underwater);
				break;
		}







		filePath = Path.Combine(Application.persistentDataPath, "data.json");

		timeLeft = GameObject.Find("LevelInfo").GetComponent<LevelInfo>().initialLevelTime;
		levelName = GameObject.Find("LevelInfo").GetComponent<LevelInfo>().levelName;

		loadGame();
	}

	void Update()
	{
		timeLeft -= Time.deltaTime;


		if (canMove)
			movementUpdate();

		if (!isDead)
			checkForDead();

		if (isDead)
			marioDie();

		if (!levelEnded)
			animationUpdate();

		if (levelEnded)
			endLevel();

		UIUpdate();


		if (timeLeft <= 95 && !playerHuried)
		{
			switch (GameObject.Find("LevelInfo").GetComponent<LevelInfo>().mapVariant)
			{
				case LevelInfo.MapVariant.Overworld:
					myAudioPlayer.playMusic(MarioSoundsAndMusic.MusicNames.HuriedOverworld);
					break;

				case LevelInfo.MapVariant.Underground:
					myAudioPlayer.playMusic(MarioSoundsAndMusic.MusicNames.HuriedUnderground);
					break;

				case LevelInfo.MapVariant.Underwater:
					myAudioPlayer.playMusic(MarioSoundsAndMusic.MusicNames.HuriedUnderwater);
					break;
			}
		}

	}

	private void UIUpdate()
	{
		World_txt.text = levelName;
		Mario_txt.text = score.ToString();

		Lives_txt.text = dataContainer.playerHp.ToString();

		string zeros = "";

		while (Mario_txt.text.Length < 6)
		{
			zeros = "0" + zeros;
			Mario_txt.text = zeros + score.ToString();
		}

		Coins_txt.text = collectedCoins.ToString();

		Time_txt.text = ((int)timeLeft).ToString();

	}

	public void jumpAfterKill(float force = 10f)
	{
		jumpAfterKillForce = force;
	}

	private void movementUpdate()
	{
		// PŁYNNE ZMIENIANIE KIERUNKU
		if (Input.GetKey(KeyCode.A))
			accelerationHorizontal -= Time.deltaTime;


		if (Input.GetKey(KeyCode.D))
			accelerationHorizontal += Time.deltaTime;

		if (Input.GetKey(KeyCode.LeftShift))
			SprintMultiplierActual = SprintMultiplier;
		else
			SprintMultiplierActual = 1f;

		if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded >= 1)
		{
			myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y + jumpHeight);

			switch (marioVariant)
			{
				case MarioVariantNames.Default:
					myAudioPlayer.playSound(MarioSoundsAndMusic.SoundNames.JumpSmallSound);
					break;

				case MarioVariantNames.Big:
				case MarioVariantNames.White:
					myAudioPlayer.playSound(MarioSoundsAndMusic.SoundNames.JumpBigSound);
					break;
			}
		}


		if((Input.GetKeyDown(KeyCode.S)) || Input.GetKeyDown(KeyCode.LeftControl) && touchingPipe != null){
			Debug.Log("Wszedłeś do " + touchingPipe);

			dataContainer.pipe_world = touchingPipe.world;
			dataContainer.pipe_pipeNr = touchingPipe.pipeNr;

			dataContainer.pipe_toWorld = touchingPipe.toWorld;
			dataContainer.pipe_toPipeNr = touchingPipe.toPipeNr;

			dataContainer.pipe_pipeType = (int)touchingPipe.pipeType;
			dataContainer.pipe_pipeLay = (int)touchingPipe.pipeLay;

			transform.GetComponent<MarioSuperpowers>().pipeIn();
		}


		accelerationHorizontal = Mathf.Clamp(accelerationHorizontal, -1 * TimeToMaxAcceleration, TimeToMaxAcceleration);



		useGravity();




		// OBLICZANIE SZYBKOŚCI
		velocityHorizontal = accelerationHorizontal / TimeToMaxAcceleration * WalkingSpeed * SprintMultiplierActual;
		velocityVertical = myRigidbody.velocity.y;

		if (jumpAfterKillForce != 0)
		{
			velocityVertical = jumpAfterKillForce;
			jumpAfterKillForce = 0f;
		}




		myRigidbody.velocity = new Vector2(velocityHorizontal, velocityVertical);
	}

	private void checkForDead()
	{

		if (deadFromHole)
			finalYPosition = new Vector2(transform.position.x, transform.position.y + 7f);

		if (deadFromEnemy)
		{
			if(marioVariant != MarioVariantNames.Default)
			{
				deadFromEnemy = false;
				GetComponent<MarioSuperpowers>().shrink();
			}
			else if(marioVariant == MarioVariantNames.Default)
			{
				if (canBeKilled)
				{
					finalYPosition = new Vector2(transform.position.x, transform.position.y + 2f);
					initialYPosition = new Vector2(transform.position.x, transform.position.y + 2f);
				}
				else
					deadFromEnemy = false;
			}

		}


		if (timeLeft <= 0)
			finalYPosition = new Vector2(transform.position.x, transform.position.y + 2f);




		if (
			deadFromHole ||
			deadFromEnemy ||
			timeLeft <= 0
			)
		{
			myRigidbody.bodyType = RigidbodyType2D.Static;
			myRigidbody.gravityScale = 0;
			initialYPosition = new Vector2(transform.position.x, transform.position.y);

			isDead = true;
			dataContainer.playerHp--;
			saveGame();
			myAudioPlayer.playMusic(MarioSoundsAndMusic.MusicNames.MarioDiesMusic);
		}



	}

	private void marioDie()
	{


		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		float step = 12f;

		if (deadFromHole)
			step = 12f * Time.deltaTime;

		if (deadFromEnemy)
			step = 6f * Time.deltaTime;

		deadFor += Time.deltaTime;

		if (!beenOnTop)
			transform.position = Vector2.MoveTowards(transform.position, finalYPosition, step);


		if (transform.position.y >= finalYPosition.y)
			beenOnTop = true;


		if (beenOnTop)
		{
			transform.position = Vector2.MoveTowards(transform.position, initialYPosition, step);
		}



		if (beenOnTop && transform.position.y <= initialYPosition.y && deadFor >= 3.5f)
		{
			isDead = false;
			dataContainer.playerMarioVariant = (int)MarioVariantNames.Default;


			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


		}

		
	}

	private void animationUpdate()
	{
		if (canMove)
		{
			// OBRACANIE SPRITA
			if (Input.GetAxis("Horizontal") > 0)
				GetComponent<SpriteRenderer>().flipX = true;


			if (Input.GetAxis("Horizontal") < 0)
				GetComponent<SpriteRenderer>().flipX = false;
		}



		// ANIMACJA
		if (isGrounded != 0)
			animationNr = AnimationNames.Idle;

		if (Mathf.Abs(myRigidbody.velocity.x) > 0f)
			animationNr = AnimationNames.Walk;

		if (Mathf.Abs(myRigidbody.velocity.x) > 7f)
			animationNr = AnimationNames.Run;

		if (myRigidbody.velocity.x == 0 && isGrounded != 0)
			animationNr = AnimationNames.Idle;



		if (isGrounded == 0)
			animationNr = AnimationNames.Jump;

		if (isDead)
			marioVariant = MarioVariantNames.Dead;




		myAnimator.SetInteger("Variant", (int)marioVariant);
		myAnimator.SetFloat("Animation", (float)animationNr);



	}

	void useGravity()
	{
		if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
		{
			if (Mathf.Abs(accelerationHorizontal) < TimeToMaxAcceleration / 100)
			{
				accelerationHorizontal = 0f;
			}
			else if (accelerationHorizontal > 0f)
			{
				accelerationHorizontal -= TimeToMaxAcceleration * Time.deltaTime * Friction;
			}
			else
			{
				accelerationHorizontal += TimeToMaxAcceleration * Time.deltaTime * Friction;
			}
		}




	}

	public void GroundDetected()
	{
		isGrounded += 1;
	}

	public void GroundLost()
	{
		isGrounded -= 1;
		canBump = true;
	}

	public void DownDetected(Collision2D collision)
	{


	}

	public void CeilDetected(Collision2D collision)
	{


		if (canBump && myRigidbody.velocity.y >= 0f)
		{


			if (collision.gameObject.name.Contains("DestroyableBlock") && marioVariant == 0)
			{
				myAudioPlayer.playSound(MarioSoundsAndMusic.SoundNames.BumpSound);
				collision.gameObject.GetComponent<DestroyableBricks>().playingAnimation = true;
				canBump = false;
			}

			else if (collision.gameObject.name.Contains("DestroyableBlock") && marioVariant != 0)
			{
				myAudioPlayer.playSound(MarioSoundsAndMusic.SoundNames.BrickSmashSound);
				collision.gameObject.GetComponent<DestroyableBricks>().bigMarioHit();
				canBump = false;
			}

			else if (collision.gameObject.name.Contains("QuestionMarkBlock"))
			{
				myAudioPlayer.playSound(MarioSoundsAndMusic.SoundNames.BumpSound);
				collision.gameObject.GetComponent<QuestionMarkBlock>().playingAnimation = true;
				canBump = false;
			}
		}


	}

	public void DownLost(Collision2D collision)
	{


	}

	public void CeilLost(Collision2D collision)
	{

	}

	private void loadGame()
	{
		marioVariant = (MarioVariantNames)dataContainer.playerMarioVariant;
		timeLeft = GameObject.Find("LevelInfo").GetComponent<LevelInfo>().initialLevelTime;



		try
		{
			string jsonData = File.ReadAllText(filePath);

			dataContainer.FromJson(jsonData);
		}
		catch (FileNotFoundException)
		{
			Debug.Log("Pierwszy raz uruchomiono grę");
		}

	}

	private void OnApplicationQuit()
	{
		dataContainer.playerMarioVariant = (int)marioVariant;
		dataContainer.playerActualLevel = GameObject.Find("LevelInfo").GetComponent<LevelInfo>().levelName;

		saveGame();
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name.Contains("Pipe"))
		{
			if (collision.gameObject.GetComponent<PipeObject>().pipeLay == PipeObject.PipeLayNames.Horizontal)
			{
				dataContainer.pipe_world = collision.gameObject.GetComponent<PipeObject>().world;
				dataContainer.pipe_pipeNr = collision.gameObject.GetComponent<PipeObject>().pipeNr;

				dataContainer.pipe_toWorld = collision.gameObject.GetComponent<PipeObject>().toWorld;
				dataContainer.pipe_toPipeNr = collision.gameObject.GetComponent<PipeObject>().toPipeNr;

				dataContainer.pipe_pipeType = (int)collision.gameObject.GetComponent<PipeObject>().pipeType;
				dataContainer.pipe_pipeLay = (int)collision.gameObject.GetComponent<PipeObject>().pipeLay;




				transform.GetComponent<MarioSuperpowers>().pipeIn();
				Debug.Log("Wszedłeś do " + touchingPipe);
			}
			else
			{
				touchingPipe = collision.gameObject.GetComponent<PipeObject>();

			}
		}

		if (collision.gameObject.name.Contains("Hole"))
			deadFromHole = true;


		if (collision.gameObject.name.Contains("CastleEntrance"))
			playerEnterCastle();

	}

	private void playerEnterCastle()
	{
		SceneManager.LoadScene("WIN");
	}



	private void OnTriggerExit2D(Collider2D collision)
	{
		//if (collision.gameObject.name.Contains("Pipe"))
			//touchingPipe = null;
	}


	public void saveGame()
	{
		string jsonData = dataContainer.ToJson();

		File.WriteAllText(filePath, jsonData);



	}



	public void endLevel()
	{
		Debug.Log($"Congrats, You did this world in {GameObject.Find("LevelInfo").GetComponent<LevelInfo>().initialLevelTime - timeLeft} seconds!");
		canMove = false;



		if (isGrounded == 0 && !touchedPole)
		{
			myAnimator.SetFloat("Animation", (float)AnimationNames.Climbing);
			//myRigidbody.bodyType = RigidbodyType2D.Kinematic;
			//myRigidbody.gravityScale = 0f;

			myRigidbody.velocity = new Vector2(0, -6f);

		}
		else
		{
			touchedPole = true;

			myAnimator.SetFloat("Animation", (float)AnimationNames.Walk);

			//myRigidbody.bodyType = RigidbodyType2D.Kinematic;
			//myRigidbody.gravityScale = 3.2f;

			myRigidbody.velocity = new Vector2(2f, -4.5f);
		}




	}


}
