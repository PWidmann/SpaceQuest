using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool questActive = true;
    private QuestManager questManager;

    private void Start()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }
    public void PickUp()
    {
        if (QuestManagerCheck())
        {
            Destroy(transform.gameObject);
        }
    }

    private bool QuestManagerCheck()
    {
        bool updated = false;

        if (questManager.IntroQuest.QuestType == QuestType.CollectMaterial && questManager.IntroQuest.Active)
        {
            questManager.IntroQuest.CurrentQuestTracking += 1;
            Debug.Log("quest tracking updated. current count: " + questManager.IntroQuest.CurrentQuestTracking + "/" + questManager.IntroQuest.QuestGoal);
            updated = true;
        }

        return updated;
    }
}
