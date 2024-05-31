using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public enum QuestState
{
    NotStarted,
    InProgress,
    Finished
}

public class Quest
{
    public string questInfo;
    public int remainingMonsters;
    public DetectionZone detectionZone;
    public GameObject questUI; // Reference to the UI element displaying the quest
    public QuestState questState;

    public Quest(string info, DetectionZone zone, GameObject ui)
    {
        questInfo = info;
        detectionZone = zone;
        questUI = ui;
        remainingMonsters = detectionZone.colliderAmount; // Initialize with current number of colliders
        questState = QuestState.NotStarted; // Initialize quest state
        UpdateUI();
    }

    public void StartQuest()
    {
        questState = QuestState.InProgress;
        UpdateUI();
    }

    public void FinishQuest()
    {
        // Deactivate or hide the quest UI element
        questUI.SetActive(false);
    }

    public void UpdateProgress()
    {
        if (questState == QuestState.InProgress)
        {
            remainingMonsters = detectionZone.colliderAmount;

            if (remainingMonsters <= 0)
            {
                questState = QuestState.Finished;
            }
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        // Assuming the UI element has a TextMeshProUGUI component
        var questText = questUI.transform.Find("QuestText").GetComponent<TMPro.TextMeshProUGUI>();
        questText.text = $"{questInfo} ({remainingMonsters} remaining)";
    }
}

