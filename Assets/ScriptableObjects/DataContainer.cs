using UnityEngine;

[CreateAssetMenu(fileName = "DataContainer", menuName = "ScriptableObjects/Data Container")]
public class DataContainer : ScriptableObject
{
	public int playerCoins = 0;
	public int playerScore = 0;

	public int playerHp = 3;
	public int playerLives = 3;
	public int playerMarioVariant = 0;

	public string playerActualLevel = "1-1";

	//pipe
	public string pipe_world;
	public string pipe_pipeNr;

	public string pipe_toWorld;
	public string pipe_toPipeNr;

	public int pipe_pipeType;
	public int pipe_pipeLay;



	public void resetToDefault()
	{
		playerCoins = 0;
		playerScore = 0;

		playerHp = 3;
		playerLives = 3;
		playerMarioVariant = 0;

		playerActualLevel = "1-1";

		//pipe
		pipe_world = "";

		pipe_pipeNr = "";


		pipe_toWorld = "";
		pipe_toPipeNr = "";

		pipe_pipeType = 0;
		pipe_pipeLay = 0;
	}

	public string ToJson()
	{
		return JsonUtility.ToJson(this);
	}

	public void FromJson(string jsonData)
	{
		JsonUtility.FromJsonOverwrite(jsonData, this);
	}
}
