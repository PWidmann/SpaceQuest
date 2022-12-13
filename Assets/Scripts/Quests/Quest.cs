using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum MainQuestType { Collect, Defend, Attack, Rescue, Following }


public class MainQuest : MonoBehaviour
{
    private MainQuestType mainQuestType;
    private PlanetType planetSpecificQuest;
    private string questName;
    private string description;
    private int reward;
    private string questGiverName;

    public MainQuestType QuestType { get => mainQuestType; set => mainQuestType = value; }
    public PlanetType PlanetSpecificQuest { get => planetSpecificQuest; set => planetSpecificQuest = value; }
    public string QuestName { get => questName; set => questName = value; }
    public string Description { get => description; set => description = value; }
    public int Reward { get => reward; set => reward = value; }
    public string QuestGiverName { get => questGiverName; set => questGiverName = value; }
    
}
