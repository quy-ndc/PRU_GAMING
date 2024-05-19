using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject quest;
    public GameObject tracker;
    public GameObject nextLevelPortal;
    public QuestState questState;
   

    public static QuestManager Instance;

    public enum QuestState
    {
        NotStarted,
        InProgress,
        Finished
    }

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
        questState = QuestState.NotStarted;
        nextLevelPortal.SetActive(false);
    }

    private void Start()
    {
        quest.SetActive(false);
    }

    private void Update()
    {
        if (questState == QuestState.InProgress)
        {
            quest.transform.Find("QuestProgress").GetComponent<TextMeshProUGUI>().text = "(" + tracker.GetComponent<DetectionZone>().colliderAmount.ToString() + " remaining)";

        }
        if (tracker.GetComponent<DetectionZone>().colliderAmount == 0 && questState == QuestState.InProgress)
        {
            OnQuestFinished();
        }
    }

    public void OnQuestReceived(string questInfo)
    {
        quest.SetActive(true);
        quest.transform.Find("QuestText").GetComponent<TextMeshProUGUI>().text = questInfo;
        questState = QuestState.InProgress;
    }

    public void OnQuestFinished()
    {
        quest.SetActive(false);
        nextLevelPortal.SetActive(true);
        questState = QuestState.Finished;
    }

}
