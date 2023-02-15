using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleric : MonoBehaviour
{
    private QuestManager questManager;


    private void Start()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }
    public void GiveQuest()
    {
        questManager.IntroQuest.Active = true;
        Debug.Log("Quest is now active");
    }
}
