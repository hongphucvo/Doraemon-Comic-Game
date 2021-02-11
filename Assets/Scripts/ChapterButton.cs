using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChapterButton : MonoBehaviour
{
	public Text titleText;
	private int chapterID=0; // chapter id that the button will open
	private DataController dataController;
	

	public void Setup(string title, DataController dC, int cid)
    {
        dataController = dC;
        titleText.text = title;
		chapterID=cid;
    }

	public void StartChapter()
    {
		//dataController.PlayClickSound();
		dataController.chapterID=chapterID;
		dataController.playerProgress.currentScore = 0;
        SceneManager.LoadScene("Game");
    }
}
