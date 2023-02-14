using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum IntroductionQuest { CollectMaterial, KillEnemies, SearchGuy }
public enum ConfrontationQuest { Boss, EnemyCamp, Rescue, Protect }
public enum SurpriseQuest { SurpriseBoss, SurpriseEnemyPack }


public class Quest : MonoBehaviour
{
    

    public string questName;
    public string description;
    public int reward;
    public string questGiverName;
    public int questobjectives;

    public string QuestName { get => questName; set => questName = value; }
    public string Description { get => description; set => description = value; }
    public int Reward { get => reward; set => reward = value; }
    public string QuestGiverName { get => questGiverName; set => questGiverName = value; }
    
}
