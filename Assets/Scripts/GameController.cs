using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	private DataController dataController;
	public AudioSource SoundFX;
	public AudioClip clickSound;
	
	public Text timeRemainingDisplay;

	private ChapterData currentChapterData;
	private SequenceData currentSequenceData;
	private QuizData quizData;

	//Introduction sequence
	public GameObject chapterStartDisplay;
	public Text introductionText;

	//Dialogue sequence
	public GameObject speechDisplay;
	public Text speechText;
	private int speechIndex;

	//Quiz sequence
	public GameObject questionDisplay;
	public Text questionText;
	public Transform answerButtonParent;
	public SimpleObjectPool answerButtonObjectPool;
	private int questionIndex;

	//Conclusion Sequence
	public GameObject chapterEndDisplay;
	public Text conclusionText;
	public Text scoreDisplay;
	public Text highScoreDisplay;

	//Characters
	public CharacterData[] characters;
	public Character characterLeft;
	public Character characterRight;
	public Character characterLeft1;

	private bool isRoundActive = false;
	private float timeRemaining;

	private List<GameObject> answerButtonGameObjects = new List<GameObject>();

	void Start()
	{
		dataController = FindObjectOfType<DataController>();                                // Store a reference to the DataController so we can request the data we need for this chapter

		currentChapterData = dataController.GetCurrentChapterData();                            // Ask the DataController for the data for the current chapter. At the moment, we only have one chapter - but we could extend this
																								//init player progress
		dataController.playerProgress.currentSequenceId = 0;
		dataController.SavePlayerProgress();


		ShowNextSequence();


	}

	void Update()
	{
		if (isRoundActive && timeRemaining > 0)
		{
			timeRemaining -= Time.deltaTime;                                                // If the chapter is active, subtract the time since Update() was last called from timeRemaining
			UpdateTimeRemainingDisplay();

			if (timeRemaining <= 0f)                                                        // If timeRemaining is 0 or less, the chapter ends
			{
				// EndRound();
			}
		}
	}


	public void ShowNextSequence()
	{
		currentSequenceData = currentChapterData.sequences[dataController.playerProgress.currentSequenceId];

		chapterStartDisplay.SetActive(false);
		speechDisplay.SetActive(false);
		questionDisplay.SetActive(false);
		chapterEndDisplay.SetActive(false);

		characterLeft.gameObject.SetActive(false);
		characterRight.gameObject.SetActive(false);
		characterLeft1.gameObject.SetActive(false);
		characterLeft.speakTail.gameObject.SetActive(false);
		characterLeft.thinkTail.gameObject.SetActive(false);
		characterLeft1.speakTail.gameObject.SetActive(false);
		characterLeft1.thinkTail.gameObject.SetActive(false);
		characterRight.speakTail.gameObject.SetActive(false);
		characterRight.thinkTail.gameObject.SetActive(false);

		for (int i = 0; i < characters.Length; i++)
		{
			if (currentSequenceData.characterIDs.Length > 0)
			{
				if (currentSequenceData.characterIDs[0] != "")
				{ //left character
					if (currentSequenceData.characterIDs[0] == characters[i].idText)
					{
						characterLeft.gameObject.SetActive(true);
						characterLeft.data = characters[i];
						characterLeft.GetComponent<Image>().sprite = characterLeft.data.picture;
					}
				}
			}

			if (currentSequenceData.characterIDs.Length > 1)
			{
				if (currentSequenceData.characterIDs[1] != "")
				{ //right character
					if (currentSequenceData.characterIDs[1] == characters[i].idText)
					{
						characterRight.gameObject.SetActive(true);
						characterRight.data = characters[i];
						characterRight.GetComponent<Image>().sprite = characterRight.data.picture;
					}
				}
			}


			if (currentSequenceData.characterIDs.Length > 2)
			{
				if (currentSequenceData.characterIDs[2] != "")
				{ //left character
					if (currentSequenceData.characterIDs[2] == characters[i].idText)
					{
						characterLeft1.gameObject.SetActive(true);
						characterLeft1.data = characters[i];
						characterLeft1.GetComponent<Image>().sprite = characterLeft1.data.picture;
					}
				}
			}
		}

		switch (currentSequenceData.type)
		{

			case sequenceType.Introduction:
				ShowIntroduction();
				break;
			case sequenceType.Dialogue:

				speechIndex = 0;
				ShowDialogue();
				break;
			case sequenceType.Quiz:
				//currentChapterData.quizzes[currentSequenceData.idNumber];
				//questionPool = currentChapterData.question;                                            // Take a copy of the questions so we could shuffle the pool or drop questions from it without affecting the original RoundData object

				//timeRemaining = currentRoundData.timeLimitInSeconds;                                // Set the time limit for this chapter based on the RoundData object
				UpdateTimeRemainingDisplay();
				questionIndex = 0;

				isRoundActive = true;
				ShowQuiz();
				break;
			case sequenceType.Conclusion:
				ShowConclusion();
				break;
		}

		//prepare for next sequence
		if (dataController.playerProgress.currentSequenceId < (currentChapterData.sequences.Length - 1)) dataController.playerProgress.currentSequenceId++;

	}

	void ShowIntroduction()
	{
		chapterStartDisplay.SetActive(true);
		introductionText.text = currentChapterData.introductionText;
	}

	void ShowDialogue()
	{
		speechDisplay.SetActive(true);

		SpeechData speechData = currentChapterData.dialogues[currentSequenceData.idNumber].speeches[speechIndex];                            // Get the QuestionData for the current question
		speechText.text = speechData.speechText;

		characterLeft.speakTail.gameObject.SetActive(false);
		characterLeft.thinkTail.gameObject.SetActive(false);
		characterRight.speakTail.gameObject.SetActive(false);
		characterRight.thinkTail.gameObject.SetActive(false);
		characterLeft1.speakTail.gameObject.SetActive(false);
		characterLeft1.thinkTail.gameObject.SetActive(false);

		if (speechData.speakingCharacterId == characterLeft.data.idText)
		{ // Left character is speaking
			characterLeft.speakTail.gameObject.SetActive(true);
		}
		else if (speechData.speakingCharacterId == characterRight.data.idText)
		{ // Right character is speaking
			characterRight.speakTail.gameObject.SetActive(true);
		}
		else if (speechData.speakingCharacterId == characterLeft1.data.idText)
		{ // Left character is speaking
			characterLeft1.speakTail.gameObject.SetActive(true);
		}
	}

	void UpdateCharacters()
	{
		//if(c)

	}

	public void SpeechButtonClicked()
	{
		//dataController.PlayClickSound();

		if (speechIndex < (currentChapterData.dialogues[currentSequenceData.idNumber].speeches.Length - 1))                                            // If there are more questions, show the next question
		{
			speechIndex++;
			ShowDialogue();
		}
		else                                                                                // If there are no more questions, the chapter ends
		{
			if (currentChapterData.dialogues[currentSequenceData.idNumber].jumpToSequence >= 0 &&   //prepare to jump to a defined sequence if the id is valid 
				currentChapterData.dialogues[currentSequenceData.idNumber].jumpToSequence < currentChapterData.sequences.Length)
			{
				dataController.playerProgress.currentSequenceId = currentChapterData.dialogues[currentSequenceData.idNumber].jumpToSequence;
			}

			ShowNextSequence();
		}
	}

	void ShowQuiz()
	{
		questionDisplay.SetActive(true);

		RemoveAnswerButtons();

		QuestionData questionData = currentChapterData.quizzes[currentSequenceData.idNumber].questions[questionIndex];                            // Get the QuestionData for the current question
		questionText.text = questionData.questionText;                                        // Update questionText with the correct text

		for (int i = 0; i < questionData.answers.Length; i++)                                // For every AnswerData in the current QuestionData...
		{
			GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();            // Spawn an AnswerButton from the object pool
			answerButtonGameObjects.Add(answerButtonGameObject);
			answerButtonGameObject.transform.SetParent(answerButtonParent);
			answerButtonGameObject.transform.localScale = Vector3.one;

			AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
			answerButton.Setup(questionData.answers[i]);                                    // Pass the AnswerData to the AnswerButton so the AnswerButton knows what text to display and whether it is the correct answer
		}
	}

	void RemoveAnswerButtons()
	{
		while (answerButtonGameObjects.Count > 0)                                            // Return all spawned AnswerButtons to the object pool
		{
			answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
			answerButtonGameObjects.RemoveAt(0);
		}
	}

	public void AnswerButtonClicked(bool isCorrect, int jumpToSequence)
	{
		//dataController.PlayClickSound();

		if (isCorrect)
		{
			dataController.playerProgress.currentScore += currentChapterData.quizzes[currentSequenceData.idNumber].questions[questionIndex].pointsAddedForCorrectAnswer;                    // If the AnswerButton that was clicked was the correct answer, add points
			scoreDisplay.text = dataController.playerProgress.currentScore.ToString();
		}

		bool continueToNextSequence = true;

		if (jumpToSequence >= 0 && jumpToSequence < currentChapterData.sequences.Length)
		{ //prepare to jump to a defined sequence if the id is valid 
			dataController.playerProgress.currentSequenceId = jumpToSequence;
			continueToNextSequence = false;
		}

		if (continueToNextSequence && questionIndex < (currentChapterData.quizzes[currentSequenceData.idNumber].questions.Length - 1))                                            // If there are more questions, show the next question
		{
			questionIndex++;
			ShowQuiz();
		}
		else                                                                                // If there are no more questions, the chapter ends
		{
			ShowNextSequence();
		}
	}

	private void UpdateTimeRemainingDisplay()
	{
		timeRemainingDisplay.text = "Time: " + Mathf.Round(timeRemaining).ToString();
	}

	void ShowConclusion()
	{
		chapterEndDisplay.SetActive(true);
		conclusionText.text = currentChapterData.conclusionText;

		isRoundActive = false;
		dataController.SubmitNewPlayerScore(dataController.playerProgress.currentScore);
		highScoreDisplay.text = "HighScore: " + dataController.GetHighestPlayerScore().ToString();
	}

	public void ReturnToMenu()
	{
		SceneManager.LoadScene("MenuScreen");
	}

	public void PlayClickSound()
	{
		dataController.PlayClickSound();
	}



}