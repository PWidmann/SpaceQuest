using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using TMPro;

public enum CollectQuestItem { Herb, Mineral, Artifact}
public class QuestManager: MonoBehaviour
{
    [SerializeField] private TMP_Text questgiverText;
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questDescriptionText;

    private MainQuestType questType;
    private string[] questGiverNames;
    private string[][] collectQuestsArray;
    private MainQuest currentQuest;
    

    private void Start()
    {
        questGiverNames = ReadAndLoadQuestGiverNames();
        LoadCollectQuests();
    }

    public void GenerateQuestComplete()
    {
        currentQuest = GenerateQuest(MainQuestType.Collect, PlanetType.Green);

        questgiverText.text = "Questgiver: " + currentQuest.QuestGiverName;
        questTitleText.text = "Quest:" + currentQuest.QuestName;
        questDescriptionText.text = currentQuest.Description;
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


        // Return the generated quest
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