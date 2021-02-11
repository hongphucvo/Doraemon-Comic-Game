using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuScreenController : MonoBehaviour {

	private DataController dataController;
	public SimpleObjectPool chapterButtonObjectPool;
	public Transform chapterButtonParent;
	private List<GameObject> chapterButtonGameObjects = new List<GameObject>();
	public static int cid=0;

    void Start()
    {
        dataController = FindObjectOfType<DataController>();    


		for (int i = 0; i < dataController.chapters.Length; i ++)                                
        {
            GameObject chapterButtonGameObject = chapterButtonObjectPool.GetObject();            
            chapterButtonGameObjects.Add(chapterButtonGameObject);
            chapterButtonGameObject.transform.SetParent(chapterButtonParent);
            chapterButtonGameObject.transform.localScale = Vector3.one;

            ChapterButton chapterButton = chapterButtonGameObject.GetComponent<ChapterButton>();
            chapterButton.Setup(dataController.chapters[i].nameText,dataController,i);
           // dataController.PlayClickSound();
            //			chapterButton.GetComponent<Button>().onClick.AddListener(()=>{chapterButton.StartChapter(this.GetComponent<ChapterButton>().cid);});        
        }
	}


}