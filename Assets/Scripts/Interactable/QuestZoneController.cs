using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestZoneController : MonoBehaviour
{
    [SerializeField]
    private bool hasGivenQuest = false;

    [SerializeField]
    private DetectionZone monsterZone;
    [SerializeField]
    private string questToGive;

    private Quest assignedQuest;

    [SerializeField]
    private GameObject wall1;
    [SerializeField]
    private GameObject wall2;


    private void Awake()
    {
        monsterZone = GetComponentInChildren<DetectionZone>();
    }

    private void Update()
    {
        wall1.SetActive(hasGivenQuest && assignedQuest.questState != QuestState.Finished);
        wall2.SetActive(hasGivenQuest && assignedQuest.questState != QuestState.Finished);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasGivenQuest)
        {
            QuestManager.Instance.AddQuest(questToGive, monsterZone);
            int questIndex = QuestManager.Instance.quests.Count - 1;
            QuestManager.Instance.StartQuest(questIndex);
            hasGivenQuest = true;
            assignedQuest = QuestManager.Instance.quests[questIndex];
        }
    }
}
