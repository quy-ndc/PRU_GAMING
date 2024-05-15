using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EventText : MonoBehaviour
{
    public Vector3 moveSpeed = new Vector3(0, 80, 0);
    public float timeToFade = 1f;

    RectTransform textTransform;

    private float timePassed = 0;
    private Color startColor;
    TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }

    private void Update()
    {
        textTransform.position += moveSpeed * Time.deltaTime;

        timePassed += Time.deltaTime;

        if (timePassed < timeToFade)
        {
            float fadeAlpha = startColor.a * (1 - timePassed / timeToFade);
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
