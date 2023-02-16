using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;



public class QuestManager: MonoBehaviour
{
    [SerializeField] private GameObject questGiverNPC;
    [SerializeField] private GameObject questTextPanel;
    [SerializeField] private Text questWindowText;

    private PlanetScriptableObject currentPlanetConfiguration;
    private Quest introQuest;
    private Quest confrontationQuest;
    private bool introStarted = false;
    private bool confrontationStarted = false;
    private bool introCompleted = false;
    private bool confrontationCompleted = false;
    private float checkTimer = 0;
    
    private string[] questGiverNames;

    public Quest IntroQuest { get => introQuest; set => introQuest = value; }
    public Quest ConfrontationQuest { get => confrontationQuest; set => confrontationQuest = value; }
    public bool IntroStarted { get => introStarted; set => introStarted = value; }
    public bool ConfrontationStarted { get => confrontationStarted; set => confrontationStarted = value; }

    private void Start()
    {
        questGiverNames = ReadAndLoadQuestGiverNames();
    }

    public void Update()
    {
        if (introStarted)
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > 1f)
            {
                if (introQuest.CurrentQuestTracking == introQuest.QuestGoal)
                {
                    introCompleted = true;
                }
            }
        }
    }


    public void GenerateQuests()
    {
        introQuest = GenerateIntroQuest();

        FillIntroQuestText();

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
        GameObject questGiver = SpawnNPC_RandomPlanetPos(questGiverNPC, IntroQuest.QuestGiverName);
        questGiver.transform.parent = GameObject.Find("Planet").transform;
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

        quest = SelectQuestGiverName(quest);
        quest.QuestGoal = UnityEngine.Random.Range(5, 11);

        


        return quest;
    }

    private void FillIntroQuestText()
    {
        string text = "Hello traveller, my name is " + introQuest.QuestGiverName + ". ";

        if (introQuest.QuestType == QuestType.CollectMaterial)
        {
            text += "I really need " + introQuest.QuestGoal + " ingridients for my shaman rituals. Could you please collect them for me?";
        }
        if (introQuest.QuestType == QuestType.KillEnemies)
        {
            text += "I can't do my research work with all those creatures around. Could you please terminate " + introQuest.QuestGoal + " of them for me?";
        }

        questWindowText.text = text;
    }

    private Quest SelectQuestGiverName(Quest quest)
    {
        Quest withIntro = quest;
        withIntro.QuestGiverName = questGiverNames[UnityEngine.Random.Range(0, questGiverNames.Length - 1)];

        return withIntro;
    }

    public GameObject SpawnNPC_RandomPlanetPos(GameObject _go, string _name)
    {
        GameObject npc = Instantiate(_go);
        npc.name = _name;
        npc.transform.position = RandomPlanetSpawnPoint();
        return npc;
    }

    public void AcceptQuestButton()
    {
        if (!introCompleted)
        {
            SetQuestPanelActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetPlayerIsInControl(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            introQuest.Active = true;
            Debug.Log("Quest is now active");
        }
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

    public void SetQuestPanelActive(bool active)
    {
        questTextPanel.SetActive(active);
    }

}