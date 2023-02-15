using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum QuestType { CollectMaterial, CollectLava, KillEnemies, Boss, EnemyCamp, Rescue, Protect}
public enum QuestStart { NPC, Beacon }

public class Quest : MonoBehaviour
{
    private QuestStart questStart;
    private QuestType questType;
    private bool active = false;
    private string questName;
    private string description;
    private int reward;
    private string questGiverName;
    private GameObject questobjective;
    private int currentQuestTracking;
    private int questGoal;

    public Quest(QuestType quest)
    {
        questType = quest;
        reward = 100;
        currentQuestTracking = 0;
    }

    public string QuestName { get => questName; set => questName = value; }
    public string Description { get => description; set => description = value; }
    public int Reward { get => reward; set => reward = value; }
    public string QuestGiverName { get => questGiverName; set => questGiverName = value; }
    public QuestStart QuestStart { get => questStart; set => questStart = value; }
    public QuestType QuestType { get => questType; }
    public GameObject Questobjective { get => questobjective; set => questobjective = value; }
    public int CurrentQuestTracking { get => currentQuestTracking; set => currentQuestTracking = value; }
    public int QuestGoal { get => questGoal; set => questGoal = value; }
    public bool Active { get => active; set => active = value; }
}
