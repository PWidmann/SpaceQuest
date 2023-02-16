using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class QuestManager: MonoBehaviour
{
    #region Members
    [SerializeField] private GameObject questGiverNPC;
    [SerializeField] private GameObject questTextPanel;
    [SerializeField] private Text questWindowTextIntro;
    [SerializeField] private Text questWindowTextConfrontation;
    [SerializeField] private Text acceptButtonText;
    [SerializeField] private Compass compass;

    private GameGUI playerGUI;
    private PlanetGenerator planetGenerator;
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
    public bool IntroCompleted { get => introCompleted; set => introCompleted = value; }
    public bool ConfrontationCompleted { get => confrontationCompleted; set => confrontationCompleted = value; }
    public Text AcceptButtonText { get => acceptButtonText; set => acceptButtonText = value; }
    #endregion

    #region Unity Methods
    private void Start()
    {
        questGiverNames = ReadAndLoadQuestGiverNames();
        playerGUI = GameObject.Find("PlayerGUI").GetComponent<GameGUI>();
        planetGenerator = GameObject.Find("PlanetGenerator").GetComponent<PlanetGenerator>();
    }

    public void Update()
    {
        if (introStarted)
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > 1f)
            {
                if (introQuest.Active)
                {
                    if (introQuest.CurrentQuestTracking == introQuest.QuestGoal)
                    {
                        IntroCompleted = true;
                    }
                }

                if (confrontationQuest.Active)
                {
                    if (confrontationQuest.CurrentQuestTracking == confrontationQuest.QuestGoal)
                    {
                        confrontationCompleted = true;
                    }
                }
            }
        }
    }
    #endregion

    #region Public Methods
    public void GenerateQuests()
    {
        introQuest = GenerateIntroQuest();
        confrontationQuest = GenerateConfrontationQuest();
        FillIntroQuestText();
        SpawnQuestGiver();

        Debug.Log("QuestGiver: " + IntroQuest.QuestGiverName);
        Debug.Log("Introquest generated: " + IntroQuest.QuestType.ToString());
        Debug.Log("Objectives needed: " + IntroQuest.QuestGoal);
        Debug.Log("Confrontation quest generated: " + confrontationQuest.QuestType.ToString());
        
    }
    public void SetQuestAcceptTextIntro(string text)
    {
        questWindowTextIntro.text = text;
    }
    public void SetQuestAcceptTextFollowQuest(string text)
    {
        questWindowTextConfrontation.text = text;
    }
    public void UpdateQuestTracker()
    {
        // Intro Quest
        if (introQuest.Active)
        {
            switch (introQuest.QuestType)
            {
                case QuestType.CollectMaterial:
                    playerGUI.SetQuestTrackerText("Materials to collect: " + introQuest.CurrentQuestTracking + "/" + introQuest.QuestGoal);
                    break;
                case QuestType.KillEnemies:
                    playerGUI.SetQuestTrackerText("Kill enemies: " + introQuest.CurrentQuestTracking + "/" + introQuest.QuestGoal);
                    break;
            }

            playerGUI.SetQuestTrackerActive(true);
        }

        // Follow Up Quest
        if (confrontationQuest.Active)
        {
            switch (confrontationQuest.QuestType)
            {
                case QuestType.Boss:
                    playerGUI.SetQuestTrackerText("Kill the boss " + confrontationQuest.CurrentQuestTracking + "/" + confrontationQuest.QuestGoal);
                    break;
                case QuestType.Rescue:
                    playerGUI.SetQuestTrackerText("Rescue a friend");
                    break;
                case QuestType.Protect:
                    playerGUI.SetQuestTrackerText("Protect the shaman!");
                    break;
                case QuestType.EnemyCamp:
                    playerGUI.SetQuestTrackerText("Clear the camp");
                    break;
            }
        }

        if (confrontationCompleted)
        {
            playerGUI.SetQuestTrackerActive(false);
        }
    }
    public void SetQuestPanelActive(bool active)
    {
        questTextPanel.SetActive(active);
    }
    public GameObject SpawnNPC_RandomPlanetPos(GameObject _go, string _name)
    {
        GameObject npc = Instantiate(_go);
        npc.name = _name;
        npc.transform.position = RandomPlanetSpawnPoint();
        return npc;
    }
    #endregion

    #region Private Methods
    private Quest GenerateIntroQuest()
    {
        Quest quest;
        int rnd = UnityEngine.Random.Range(0, 2);
        switch (rnd)
        {
            case 0:
                quest = new Quest(QuestType.CollectMaterial);
                quest = SelectQuestGiverName(quest);
                quest.QuestGoal = UnityEngine.Random.Range(5, 11);
                break;
            case 1:
                quest = new Quest(QuestType.KillEnemies);
                quest = SelectQuestGiverName(quest);
                quest.QuestGoal = UnityEngine.Random.Range(5, 11);
                break;
            default:
                quest = new Quest(QuestType.CollectMaterial);
                quest = SelectQuestGiverName(quest);
                quest.QuestGoal = UnityEngine.Random.Range(5, 11);
                break;
        }

        return quest;
    }
    private Quest GenerateConfrontationQuest()
    {
        int rnd = UnityEngine.Random.Range(0, 4);
        Quest quest;
        quest = new Quest(QuestType.Boss);
        quest.QuestGoal = 1;

        return quest;
    }
    private Quest SelectQuestGiverName(Quest quest)
    {
        Quest withIntro = quest;
        withIntro.QuestGiverName = questGiverNames[UnityEngine.Random.Range(0, questGiverNames.Length - 1)];

        return withIntro;
    }
    private void SpawnQuestGiver()
    {
        GameObject questGiver = SpawnNPC_RandomPlanetPos(questGiverNPC, IntroQuest.QuestGiverName);
        questGiver.transform.parent = GameObject.Find("Planet").transform;
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

        questWindowTextIntro.text = text;
    }
    public void AcceptQuestButton()
    {
        if (!introQuest.Active && !introCompleted)
        {
            SetQuestPanelActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetPlayerIsInControl(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            introQuest.Active = true;
            
            UpdateQuestTracker();
            introStarted = true;
            Debug.Log("Intro quest is now active");
        }

        if (introCompleted && !confrontationStarted)
        {
            SetupFollowUpQuest();
            SetQuestPanelActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetPlayerIsInControl(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            confrontationQuest.Active = true;
            confrontationQuest.QuestGoal = 1;
            confrontationQuest.CurrentQuestTracking = 0;
            
            UpdateQuestTracker();
            confrontationStarted = true;
            Debug.Log("Follow up quest is now active");
        }

        // If confrontational quest is completed
        if (confrontationCompleted)
        {
            SetQuestPanelActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SetPlayerIsInControl(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("Confrontation quest completed");
            compass.QuestGiverIndicator.GetComponent<RawImage>().enabled = false;
        }
    }
    private void SetupFollowUpQuest()
    {
        // Spawn Boss Enemy
        GameObject bossEnemy = Instantiate(planetGenerator.CurrentPlanetConfiguration.boss);
        bossEnemy.transform.position = RandomPlanetSpawnPoint();
        bossEnemy.GetComponent<SimpleEnemyController>().BossEnemy = true;
        bossEnemy.GetComponent<SimpleEnemyController>().ActivateNPC(GameObject.FindGameObjectWithTag("Player"));
        compass.FollowUpQuestTarget = bossEnemy;
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
        // Read names from file
        string filePath = "Assets/GameDataInput/QuestGiverNames.txt";
        string fileContents = File.ReadAllText(filePath);

        // Split the file contents by line to get the individual names
        string[] names = fileContents.Split('\n');

        return names;
    }
    #endregion
}