using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TreeController : MonoBehaviour
{
    [SerializeField]
    private bool isInRange = false;
    private GameObject questIndicator;
    private bool hasGivenQuest = false;

    [SerializeField]
    private DetectionZone monsterZone;
    [SerializeField]
    private string questToGive;

    [SerializeField]
    private string preQuestLine;
    [SerializeField]
    private string postQuestLine;

    private Quest assignedQuest;

    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D col;

    public bool IsGrown
    {
        get
        {
            return animator.GetBool("isGrown");
        }
        set
        {
            animator.SetBool("isGrown", value);
        }
    }

    private void Awake()
    {
        monsterZone = GetComponentInChildren<DetectionZone>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();   
        questIndicator = transform.Find("QuestIndicator").gameObject;
    }

    private void Update()
    {
        questIndicator.SetActive(!hasGivenQuest);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    public void OnTalkedTo(InputAction.CallbackContext context)
    {
        if (context.started && isInRange)
        {
            if (!hasGivenQuest)
            {
                CharacterEvents.characterTalk(gameObject, new Vector2(0.5f, 0f), preQuestLine);
                QuestManager.Instance.AddQuest(questToGive, monsterZone);
                int questIndex = QuestManager.Instance.quests.Count - 1;
                QuestManager.Instance.StartQuest(questIndex);
                assignedQuest = QuestManager.Instance.quests[questIndex];
                hasGivenQuest = true;
            }
            else
            {
                // Check the state of the assigned quest
                if (assignedQuest.questState == QuestState.Finished)
                {
                    IsGrown = true;
                    CharacterEvents.characterTalk(gameObject, new Vector2(0.5f, 0f), postQuestLine);
                    rb.bodyType = RigidbodyType2D.Static;
                    QuestManager.Instance.questDone += 1;
                }
                else
                {
                    CharacterEvents.characterTalk(gameObject, new Vector2(0.5f, 0f), preQuestLine);
                }
            }
        }
    }
}
