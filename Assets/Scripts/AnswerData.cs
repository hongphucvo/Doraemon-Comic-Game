using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnswerData
{
    public string answerText;
	public bool isCorrect;
	public int jumpToSequence=-1;	// Sequence id of the current chapter to which the story will jump if this answer is chosen.
									// default value -1 => continue to next sequence.
}