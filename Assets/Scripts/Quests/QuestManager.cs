using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;



public class QuestManager: MonoBehaviour
{
    [SerializeField] private GameObject questGiverNPC;
    [SerializeField] private GameObject questGiverBeacon;
    [SerializeField] private Compass compass;

    private PlanetScriptableObject currentPlanetConfiguration;
    private Quest introQuest;
    private Quest confrontationQuest;
    private bool introCompleted = false;
    private bool confrontationCompleted = false;
    
    private string[] questGiverNames;

    public Quest IntroQuest { get => introQuest; set => introQuest = value; }
    public Quest ConfrontationQuest { get => confrontationQuest; set => confrontationQuest = value; }

    private void Start()
    {
        questGiverNames = ReadAndLoadQuestGiverNames();
    }


    public void GenerateQuests()
    {
        introQuest = GenerateIntroQuest();

        SpawnQuestGiver();

        //switch (planetType)
        //{
        //    case PlanetType.Green:
        //        break;
        //    case PlanetType.Desert:
        //        break;
        //    case PlanetType.Poison:
        //        break;
        //    case PlanetType.Lava:
        //        break;
        //    case PlanetType.Ice:
        //        break;
        //}

        Debug.Log("Introquest generated: " + IntroQuest.QuestType.ToString());
        Debug.Log("Objectives needed: " + IntroQuest.QuestGoal);
        Debug.Log("QuestGiver: " + IntroQuest.QuestGiverName);
    }

    private void SpawnQuestGiver()
    {
        if (IntroQuest.QuestGiverName == "Beacon")
        {
            GameObject questGiver = SpawnNPC_RandomPlanetPos(questGiverBeacon, "Beacon");
            questGiver.transform.parent = GameObject.Find("Planet").transform;
        }
        else
        {
            GameObject questGiver = SpawnNPC_RandomPlanetPos(questGiverNPC, IntroQuest.QuestGiverName);
            questGiver.transform.parent = GameObject.Find("Planet").transform;
        }
    }

    private void GenerateGreenQuest()
    {
        introQuest = GenerateIntroQuest();
    }

    private Quest GenerateIntroQuest()
    {
        Quest quest;
        int rnd = UnityEngine.Random.Range(0, 2);
        if (rnd == 0)
        {
            quest = new Quest(QuestType.CollectMaterial);
        }
        else
        {
            quest = new Quest(QuestType.KillEnemies);
        }

        quest = QuestIntroSelect(quest);
        quest.QuestGoal = UnityEngine.Random.Range(5, 11);
        return quest;
    }

    private Quest QuestIntroSelect(Quest quest)
    {
        Quest withIntro = quest;
        int rnd = UnityEngine.Random.Range(0, 2);
        if (rnd == 0)
        {
            withIntro.QuestStart = QuestStart.Beacon;
            withIntro.QuestGiverName = "Beacon";
        }
        else
        {
            withIntro.QuestStart = QuestStart.NPC;
            withIntro.QuestGiverName = questGiverNames[UnityEngine.Random.Range(0, questGiverNames.Length - 1)];
        }

        return withIntro;
    }

    public GameObject SpawnNPC_RandomPlanetPos(GameObject _go, string _name)
    {
        GameObject npc = Instantiate(_go);
        npc.name = _name;
        npc.transform.position = RandomPlanetSpawnPoint();
        return npc;
    }

    private Vector3 RandomPlanetSpawnPoint()
    {
        // Planet terrain max height + margin
        float radius = 300f;

        // Generate a random angle for the x and y axes.
        float angleX = Random.Range(0.0f, 360.0f);
        float angleY = Random.Range(0.0f, 360.0f);

        // Calculate the x, y, and z coordinates of the point on the sphere using the input radius and the generated angles.
        float x = radius * Mathf.Sin(angleX) * Mathf.Cos(angleY);
        float y = radius * Mathf.Sin(angleX) * Mathf.Sin(angleY);
        float z = radius * Mathf.Cos(angleX);

        // Planet radius point
        Vector3 point = new Vector3(x, y, z);

        // Create a raycast from the planet radius point to Vector3.Zero.
        Ray ray = new Ray(point, Vector3.zero - point);
        Vector3 output = Vector3.zero;
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        output = hit.point;

        // Return the point on the sphere as a Vector3.
        return output;
    }

    private string[] ReadAndLoadQuestGiverNames()
    {
        string filePath = "Assets/GameDataInput/QuestGiverNames.txt";
        string fileContents = File.ReadAllText(filePath);

        // Split the file contents by line to get the individual names
        string[] names = fileContents.Split('\n');

        return names;
    }

}