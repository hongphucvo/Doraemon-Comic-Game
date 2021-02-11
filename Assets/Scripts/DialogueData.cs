using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A dialogue is a sequence where one or more characters are talking

[System.Serializable]
public class DialogueData
{
	public string nameText; // only used if you want to introduce the quiz sequence with a name like "Find the problem"
	public SpeechData[] speeches; // represents a list of each character's speech. 1st speach is speach 0!
	public int jumpToSequence=-1;	// Sequence id of the current chapter to which the story will jump if this answer is chosen.
									// default value -1 => continue to next sequence.
}
