using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    #region Members
    private QuestManager questManager;
    private PlayerController playerController;
    private GameGUI playerGUI;
    #endregion

    #region Methods
    private void Start()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        playerGUI = GameObject.Find("PlayerGUI").GetComponent<GameGUI>();
    }
    public void GiveQuest()
    {
        // When first get the quest
        if (!questManager.IntroStarted)
        {
            questManager.IntroStarted = true;
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerController.SetPlayerIsInControl(false);
            questManager.SetQuestPanelActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // When intro quest completed
        if (questManager.IntroCompleted && !questManager.ConfrontationStarted)
        {
            questManager.IntroQuest.Active = false;
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerController.SetPlayerIsInControl(false);
            questManager.SetQuestPanelActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (questManager.IntroQuest.QuestType == QuestType.CollectMaterial)
            {
                string text = "Thank you very much for collecting " + questManager.IntroQuest.QuestGoal + " ingridients, that helps me a great deal.";
                questManager.SetQuestAcceptTextIntro(text);
            }
            if (questManager.IntroQuest.QuestType == QuestType.KillEnemies)
            {
                string text = "Thank you very much for taking care of the " + questManager.IntroQuest.QuestGoal + " enemies, now I can focus on my shaman studies.";
                questManager.SetQuestAcceptTextIntro(text);
            }

            switch (questManager.ConfrontationQuest.QuestType)
            {
                case QuestType.Boss:
                    string text = "Since you did such a good job on your last quest, I have a follow-up request. Could you please defeat the big boss creature that is terrorizing the planet?";
                    questManager.SetQuestAcceptTextFollowQuest(text);
                    break;
                case QuestType.Rescue:
                    break;
                case QuestType.Protect:
                    break;
                case QuestType.EnemyCamp:
                    break;
            }
        }

        // When the follow up quest is completed
        if (questManager.ConfrontationCompleted)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerController.SetPlayerIsInControl(false);
            questManager.SetQuestPanelActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            questManager.AcceptButtonText.text = "Ok";

            string text = "You completed my hard task to my full satisfaction. You safed this planet, thank you very much!";
            string text2 = "I will no longer bother you. You can leave this planet now if you like to.";
            questManager.SetQuestAcceptTextIntro(text);
            questManager.SetQuestAcceptTextFollowQuest(text2);
            playerGUI.Compass.GetComponent<Compass>().QuestGiverIndicator.enabled = false;
        }
    }
    #endregion
}
