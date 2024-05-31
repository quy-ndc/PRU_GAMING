using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject nextLevelIndicator;
    public List<Quest> quests = new List<Quest>();
    public GameObject questPrefab;
    public Transform questSpawnPoint;
    public float yOffset = 60f;
    public int questToPass;
    public int questDone = 0;

    public static QuestManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (nextLevelIndicator != null)
        {
            nextLevelIndicator.SetActive(false);
        }
    }

    private void Update()
    {
        bool allQuestsFinished = true;

        foreach (var quest in quests)
        {
            quest.UpdateProgress();
        }

        foreach (var quest in quests)
        {
            quest.UpdateProgress();
            if (quest.questState != QuestState.Finished)
            {
                allQuestsFinished = false;
            }
        }

        nextLevelIndicator.SetActive(allQuestsFinished && nextLevelIndicator && questDone >= questToPass);

        for (int i = quests.Count - 1; i >= 0; i--)
        {
            if (quests[i].questState == QuestState.Finished)
            {
                quests[i].FinishQuest(); // Hide or deactivate the quest UI element
                quests.RemoveAt(i);
            }
        }
    }


    public void AddQuest(string questInfo, DetectionZone detectionZone)
    {
        // Instantiate the quest UI element
        GameObject questUI = Instantiate(questPrefab, new Vector2(questSpawnPoint.transform.position.x, questSpawnPoint.transform.position.y - yOffset * quests.Count), Quaternion.identity, questSpawnPoint);
        questUI.SetActive(true);

        // Create a new quest and add it to the list
        Quest newQuest = new Quest(questInfo, detectionZone, questUI);
        quests.Add(newQuest);
    }

    public void StartQuest(int questIndex)
    {
        if (questIndex >= 0 && questIndex < quests.Count)
        {
            quests[questIndex].StartQuest();
        }
    }

    //public void OnQuestFinished()
    //{
    //    nextLevelIndicator.SetActive(true);
    //}

}
