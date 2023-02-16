using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    #region Members
    public bool questActive = true;
    private QuestManager questManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }
    #endregion

    #region public Methods
    public void PickUp()
    {
        if (QuestManagerCheck())
        {
            Destroy(transform.gameObject);
        }
    }
    #endregion

    #region Private Methods
    private bool QuestManagerCheck()
    {
        bool updated = false;

        if (questManager.IntroQuest.QuestType == QuestType.CollectMaterial && questManager.IntroQuest.Active)
        {
            if (questManager.IntroQuest.CurrentQuestTracking < questManager.IntroQuest.QuestGoal)
            {
                questManager.IntroQuest.CurrentQuestTracking += 1;
                questManager.UpdateQuestTracker();
                updated = true;
            }
        }

        return updated;
    }
    #endregion
}
