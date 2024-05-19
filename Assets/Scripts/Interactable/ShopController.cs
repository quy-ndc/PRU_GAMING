using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopController : MonoBehaviour
{
    public bool isInRange = false;
    public int currentLine = 0;
    public int questGivenLine;
    public List<string> lines = new List<string>();
    GameObject questIndicator;
    public bool hasTalked = false;

    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        questIndicator = transform.Find("QuestIndicator").gameObject;
    }

    private void Update()
    {
        questIndicator.SetActive(!hasTalked);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInRange = false;
    }

    public void OnTalkedTo(InputAction.CallbackContext context)
    {
        if (context.started && isInRange)
        {
            if (QuestManager.Instance.questState == QuestManager.QuestState.Finished)
            {
                CharacterEvents.characterTalk.Invoke(gameObject, new Vector2(0f, -0.5f), "Jump down the river to progress");
            }
            else
            {
                CharacterEvents.characterTalk.Invoke(gameObject, new Vector2(0f, -0.5f), lines[currentLine]);
                hasTalked = true;
                if (currentLine == questGivenLine)
                {
                    QuestManager.Instance.OnQuestReceived("Kill all the monsters in the cave");
                }
                if (currentLine < lines.Count - 1)
                {
                    currentLine++;
                }
            }
        }
    }
}
