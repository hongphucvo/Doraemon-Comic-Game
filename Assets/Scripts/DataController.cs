using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour 
{
    public ChapterData[] chapters; //list of all chapters
	public int chapterID=0;
    public PlayerData playerProgress;
	public AudioSource SoundFX;
	public AudioClip clickSound;
    public AudioClip muteSound;

    private string gameDataFileName = "gameData.json";

    void Start()
    {
        DontDestroyOnLoad(gameObject);

		SoundFX=this.GetComponent<AudioSource>();

        LoadGameData();

        LoadPlayerProgress();

        SceneManager.LoadScene("MenuScreen");
    }

    public ChapterData GetCurrentChapterData()
    {
        return chapters[chapterID];
    }

    public void SubmitNewPlayerScore(int newScore)
    {
        // If newScore is greater than playerProgress.highestScore, update playerProgress with the new value and call SavePlayerProgress()
        if(newScore > playerProgress.highestScore)
        {
            playerProgress.highestScore = newScore;
            SavePlayerProgress();
        }
    }

    public int GetHighestPlayerScore()
    {
        return playerProgress.highestScore;
    }

    private void LoadGameData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if(File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);    
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            // Retrieve the allRoundData property of loadedData
            chapters = loadedData.chapters;
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object
    private void LoadPlayerProgress()
    {
        // Create a new PlayerProgress object
        playerProgress = new PlayerData();

        // If PlayerPrefs contains a key called "highestScore", set the value of playerProgress.highestScore using the value associated with that key
        if(PlayerPrefs.HasKey("highestScore"))
        {
            playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
        }
		if(PlayerPrefs.HasKey("currentScore"))
        {
            playerProgress.currentScore = PlayerPrefs.GetInt("currentScore");
        }
		if(PlayerPrefs.HasKey("currentChapterId"))
        {
            playerProgress.currentChapterId = PlayerPrefs.GetInt("currentChapterId");
        }
		if(PlayerPrefs.HasKey("currentSequenceId"))
        {
            playerProgress.currentSequenceId = PlayerPrefs.GetInt("currentSequenceId");
        }

    }

    // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object
    public void SavePlayerProgress()
    {
        // Save the value playerProgress.highestScore to PlayerPrefs, with a key of "highestScore"
        PlayerPrefs.SetInt("highestScore", playerProgress.highestScore);
		PlayerPrefs.SetInt("currentScore", playerProgress.currentScore);
		PlayerPrefs.SetInt("currentChapterId", playerProgress.currentChapterId);
		PlayerPrefs.SetInt("currentSequenceId", playerProgress.currentSequenceId);
    }



	public void PlayClickSound()
    {
           SoundFX.PlayOneShot(clickSound);
          
    }

}