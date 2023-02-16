using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    private QuestManager questManager;
    private PlayerController playerController;
    private void Start()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }
    public void GiveQuest()
    {
        if (!questManager.IntroStarted)
        {
            questManager.IntroStarted = true;
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerController.SetPlayerIsInControl(false);
            questManager.SetQuestPanelActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
