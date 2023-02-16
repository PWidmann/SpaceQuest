using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleric : MonoBehaviour
{
    #region Members
    private QuestManager questManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }
    #endregion

    #region Public Methods
    public void GiveQuest()
    {
        questManager.IntroQuest.Active = true;
        Debug.Log("Quest is now active");
    }
    #endregion
}
