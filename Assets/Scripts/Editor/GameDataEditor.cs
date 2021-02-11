using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class GameDataEditor :  EditorWindow
{

    public GameData gameData;
	Vector2 scrollPos;
    private string gameDataProjectFilePath = "/StreamingAssets/gameData.json";

    [MenuItem ("Window/Game Data Editor")]
    static void Init()
    {
        EditorWindow.GetWindow (typeof(GameDataEditor)).Show ();
    }

    void OnGUI()
    {
		scrollPos =
            EditorGUILayout.BeginScrollView(scrollPos);
        if (gameData != null) 
        {
            SerializedObject serializedObject = new SerializedObject (this);
            SerializedProperty serializedProperty = serializedObject.FindProperty ("gameData");
			EditorGUIUtility.labelWidth = 240.0f;
            EditorGUILayout.PropertyField (serializedProperty, true);

            serializedObject.ApplyModifiedProperties ();

            if (GUILayout.Button ("Save data"))
            {
                SaveGameData();
            }
        }

        if (GUILayout.Button ("Load data"))
        {
            LoadGameData();
        }
		EditorGUILayout.EndScrollView();
    }

    private void LoadGameData()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists (filePath)) {
            string dataAsJson = File.ReadAllText (filePath);
            gameData = JsonUtility.FromJson<GameData> (dataAsJson);
        } else 
        {
            gameData = new GameData();
			gameData.chapters= new ChapterData[1];
        }
    }

    private void SaveGameData()
    {

        string dataAsJson = JsonUtility.ToJson (gameData);

        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText (filePath, dataAsJson);

    }
}