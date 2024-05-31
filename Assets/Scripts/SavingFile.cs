using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Collections;

public class SavingFile : MonoBehaviour
{
    private PlayerData loadedData = null;

    void Start()
    {
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /*
        public void SaveGame()
        {
            PlayerData data = new PlayerData(PlayerController.Instance);
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + "/savegame.json", json);
            Debug.Log("Game Saved at: " + Application.persistentDataPath + "/savegame.json");
        }*/
    public void SaveGame()
    {

     
        

        // Thực hiện quá trình lưu [public void SaveGame() { loadingText.SetActive(true); // Hiển thị dòng text "Đang lưu" // Gọi hàm ẩn dòng text sau 2 giây Invoke("HideLoadingText", 2f); // Thực hiện quá trình lưu] x game
        PlayerData data = new PlayerData(PlayerController.Instance);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savegame.json", json);
        Debug.Log("Game Saved at: " + Application.persistentDataPath + "/savegame.json");

        
    }

  /*  public void HideLoadingText()
    {
        StartCoroutine(HideLoadingCoroutine());
    }

    private IEnumerator HideLoadingCoroutine()
    {
        // Chờ 2 giây
        yield return new WaitForSeconds(2f);

        // Sau khi chờ xong, ẩn Text Mesh Pro
        loadingStatus.enabled = false;
    }*/

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/savegame.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(data.currentLevel, LoadSceneMode.Single);
            asyncLoad.completed += (AsyncOperation operation) =>
            {
                // Đảm bảo scene đã tải xong và `PlayerController` đã sẵn sàng
                if (PlayerController.Instance != null)
                {
                    PlayerController.Instance.Health = data.health;
                    PlayerController.Instance.transform.position = new Vector2(data.position.x, data.position.y);
                    PlayerController.Instance.IsFacingRight = data.isFacingRight;
                    PlayerController.Instance.IsAlive = data.isAlive;
                    Debug.Log("Game Loaded");
                }
            };
        }
        else
        {
            Debug.LogError("No save file found at: " + path);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (loadedData != null)
        {
            PlayerController.Instance.Health = loadedData.health;
            PlayerController.Instance.transform.position = new Vector3(loadedData.position.x, loadedData.position.y, 0);
            PlayerController.Instance.IsFacingRight = loadedData.isFacingRight;
            PlayerController.Instance.IsAlive = loadedData.isAlive;

            loadedData = null; // Clear the loaded data after updating the player
        }
    }
}