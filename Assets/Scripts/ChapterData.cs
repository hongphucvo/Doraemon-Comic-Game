using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a chapter is a game level and is made of sequences.
[System.Serializable]
public class ChapterData
{
	public string nameText; // Chapter's name. Example : Chapter 1 or Day 1 etc.
	public string introductionText;
	public string conclusionText;
	public SequenceData[] sequences; // list of sequences. The first one is number 0
	public DialogueData[] dialogues; // list of dialogues. The first one is number 0
	public QuizData[] quizzes; // list of quizzes. The first one is number 0
}
