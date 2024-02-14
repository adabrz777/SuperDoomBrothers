using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuManager : MonoBehaviour
{
	public DataContainer dataContainer;

    public void startNewGame()
	{
		string filePath = Path.Combine(Application.persistentDataPath, "data.json");
		File.WriteAllText(filePath, string.Empty);
		dataContainer.resetToDefault();
		SceneManager.LoadScene("1-1");
	}

	public void quitGame()
	{
		Application.Quit();
	}

	public void goToCredits()
	{
		SceneManager.LoadScene("Credits");
	}

	public void goToMainMenu()
	{
		SceneManager.LoadScene("StartMenu");
	}
}
