using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class UIManager : MonoBehaviour
{
    public Canvas gameCanvas;

    public GameObject damageTextPrefab;
    public GameObject blockTextPrefab;
    public Transform chatBubblePrefab;

    private void Awake()
    {
        gameCanvas = FindAnyObjectByType<Canvas>();
    }

    private void Start()
    {
        if (!gameCanvas)
        {
            gameCanvas = FindObjectOfType<Canvas>();
        }
    }

    private void OnEnable()
    {
        CharacterEvents.characterDamaged += (CharacterTookDamange);
        CharacterEvents.characterBlocked += (CharacterBlocked);
        CharacterEvents.characterTalk += (CharacterTalk);
    }

    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= (CharacterTookDamange);
        CharacterEvents.characterBlocked -= (CharacterBlocked);
        CharacterEvents.characterTalk -= (CharacterTalk);
    }

    public void CharacterTookDamange(GameObject character, float damange)
    {
        Vector3 spawnPossition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPossition, Quaternion.identity, gameCanvas?.transform).GetComponent<TMP_Text>();

        tmpText.text = damange.ToString();
    }

    public void CharacterBlocked(GameObject character, string text)
    {
        Vector3 spawnPossition = Camera.main.WorldToScreenPoint(character.transform.position);
        Debug.Log("1 " + spawnPossition);
        TMP_Text tmpText = Instantiate(blockTextPrefab, spawnPossition, Quaternion.identity, gameCanvas?.transform).GetComponent<TMP_Text>();

        tmpText.text = text;
    }

    public void CharacterTalk(GameObject character, Vector2 offset, string text)
    {
        Transform chatBubbleTransform = Instantiate(chatBubblePrefab, character.transform);
        chatBubbleTransform.transform.position = new Vector2(character.transform.position.x + offset.x, character.transform.position.y + offset.y);
        chatBubbleTransform.GetComponent<ChatBubble>().Setup(text);
        Destroy(chatBubbleTransform.gameObject, 3);
    }

}
