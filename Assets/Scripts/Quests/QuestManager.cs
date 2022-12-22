using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;


public enum CollectQuestItem { Herb, Mineral, Artifact}
public class QuestManager: MonoBehaviour
{
    // Debug stuff
    [SerializeField] private TMP_Text questgiverText;
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questDescriptionText;

    [Header("NPC Prefabs")]
    [SerializeField] private GameObject creaturePrefab;
    [SerializeField] private GameObject clericPrefab;

    [Header("NPC Variant Materials")]
    [SerializeField] private Material[] dragonNightmareMaterials;


    private MainQuestType questType;
    private string[] questGiverNames;
    private string[][] collectQuestsArray;
    private MainQuest currentQuest;




    private void Start()
    {
        
        questGiverNames = ReadAndLoadQuestGiverNames();
        LoadCollectQuests();
    }

    public void GenerateMainQuest()
    {
        currentQuest = GenerateQuest(MainQuestType.Collect, PlanetType.Green);

        questgiverText.text = "Questgiver: " + currentQuest.QuestGiverName;
        questTitleText.text = "Quest:" + currentQuest.QuestName;
        questDescriptionText.text = currentQuest.Description;
    }

    /// <summary>
    /// Has 4 different material variants: 0 = Green, 1 = Blue, 2 = DarkBlue, 3 = Albino
    /// </summary>
    /// <param name="_variantID"></param>
    public void SpawnCreature(int _variantID)
    {
        GameObject npc = SpawnNPC_RandomPlanetPos(creaturePrefab, "Creature");

        npc.GetComponentInChildren<SkinnedMeshRenderer>().material = dragonNightmareMaterials[_variantID];
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

    // Method to generate a random quest
    private MainQuest GenerateQuest(MainQuestType _questType, PlanetType _planetType)
    {
        MainQuest quest = new MainQuest();

        int rnd = Random.Range(0, questGiverNames.Length);
        quest.QuestGiverName = questGiverNames[rnd];

        // Set the quest type based on the input
        quest.QuestType = _questType;

        int rnd2 = Random.Range(0, collectQuestsArray.Length);
        quest.QuestName = collectQuestsArray[rnd2][0];
        quest.Description = collectQuestsArray[rnd2][1];

        return quest;
    }

    private string[] ReadAndLoadQuestGiverNames()
    {
        string filePath = "Assets/GameDataInput/QuestGiverNames.txt";
        string fileContents = File.ReadAllText(filePath);

        // Split the file contents by line to get the individual names
        string[] names = fileContents.Split('\n');

        return names;
    }

    private void LoadCollectQuests()
    {
        // File path of the external file
        string filePath = "Assets/GameDataInput/Collect_Quests.txt"; // Delete "Assets/" when shipping
        // Read the contents of the file as a string
        string fileContents = File.ReadAllText(filePath);
        // Split the file contents by line to get the individual lines
        string[] lines = fileContents.Split('\n');
        // Create a list to store the quest information
        List<string[]> quests = new List<string[]>();
        // Create a new string array to store the quest information
        string[] quest = new string[2];
        int questCount = 0;

        // Loop through the lines and store the quest information in the list
        for (int i = 0; i < lines.Length; i++)
        {
            // Check if the line is a quest name or description
            if (lines[i].StartsWith("[Name]"))
            {
                // Store the quest name in the array
                quest = new string[2];
                quest[0] = lines[i + 1];
            }

            // Check if the next line is the quest description
            if (lines[i].StartsWith("[Description]"))
            {
                // Store the quest description in the array
                quest[1] = lines[i + 1];
                // Add the quest information to the list
                quests.Add(quest);
                questCount++;
            }  
        }

        collectQuestsArray = new string[questCount][];
        collectQuestsArray = quests.ToArray();
    }
}