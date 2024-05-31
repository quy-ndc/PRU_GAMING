using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject levelButtons;
    public GameObject pauseButtons;
    // Start is called before the first frame update
    void Start()
    {
        levelButtons.SetActive(false);//Ẩn menu khi bắt đầu

    }
    public void ShowLevelButtons()
    {
        levelButtons.SetActive(true);
        pauseButtons.SetActive(false);
    }

    // Hàm để hiển thị menu chọn level
    public void ToggleLevelMenu()
    {
        levelButtons.SetActive(!levelButtons.activeSelf);  // Toggle trạng thái hiển thị của menu
    }
    public void HideLevelButtons()
    {
        pauseButtons.SetActive(true);
        levelButtons.SetActive(false);
    }
}
