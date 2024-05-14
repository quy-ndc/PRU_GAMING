using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class UIManager : MonoBehaviour
{
    public Canvas gameCanvas;

    public GameObject damageTextPrefab;
    public GameObject blockTextPrefab;

    private void Awake()
    {
        gameCanvas = FindObjectOfType<Canvas>();
        CharacterEvents.characterDamaged += (CharacterTookDamange);
        CharacterEvents.characterBlocked += (CharacterBlocked);
    }

    private void OnEnable()
    {
        CharacterEvents.characterDamaged += (CharacterTookDamange);
        CharacterEvents.characterBlocked += (CharacterBlocked);
    }

    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= (CharacterTookDamange);
        CharacterEvents.characterBlocked -= (CharacterBlocked);

    }

    public void CharacterTookDamange(GameObject character, float damange)
    {
        Vector3 spawnPossition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPossition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = damange.ToString();
    }

    public void CharacterBlocked(GameObject character, string text)
    {
        Vector3 spawnPossition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(blockTextPrefab, spawnPossition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();

        tmpText.text = text;
    }

}
