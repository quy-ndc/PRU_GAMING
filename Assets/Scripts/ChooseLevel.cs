using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseLevel : MonoBehaviour

{
  /*  public GameObject levelMenu;
    // Start is called before the first frame update
    void Start()
    {
        levelMenu.SetActive(false);//Ẩn menu khi bắt đầu
    }

    // Hàm để hiển thị menu chọn level
    public void ToggleLevelMenu()
    {
        levelMenu.SetActive(!levelMenu.activeSelf);  // Toggle trạng thái hiển thị của menu
    }

    // Hàm được gọi khi một level được chọn
    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene("Level" + levelIndex);
    }*/
    public void OpenLevel(int levelId)
    {
        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelName);
    }
}
