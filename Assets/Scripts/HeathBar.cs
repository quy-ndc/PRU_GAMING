using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeathBar : MonoBehaviour
{
    public Image FillBar;
    public TextMeshProUGUI valueText;

    public void UpdateBar(int curValue, int maxValue)
    {
        FillBar.fillAmount = (float)curValue / (float)maxValue;
        valueText.text = curValue.ToString() + " / " + maxValue.ToString();
    }
}
