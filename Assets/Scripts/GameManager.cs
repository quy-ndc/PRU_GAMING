using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.IO;
using System;


public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        Question,
        ChestOpened
    }

    public GameState currentState;
    public GameState previousState;



    [Header("UI")]
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private GameObject questionScreen;

    public static GameManager Instance;

    private string _quest;
    public string Quest
    {
        get
        {
            return _quest;
        }
        set
        {
            _quest = value;
        }
    }

    public class SaveData
    {
        public string currentState;
        public string previousState;
        public string quest;
        // Thêm các biến khác tại đây nếu bạn muốn lưu thêm thông tin
    }

    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            currentState = currentState.ToString(),
            previousState = previousState.ToString(),
            quest = Quest
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("Game Saved at: " + Application.persistentDataPath + "/savefile.json");
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            currentState = (GameState)Enum.Parse(typeof(GameState), data.currentState);
            previousState = (GameState)Enum.Parse(typeof(GameState), data.previousState);
            Quest = data.quest;
            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.LogError("No save file found at: " + path);
        }
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
        DisableScreen();
    }
    public void QuitGame()
    {
        Application.Quit();
         #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
         #endif
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            Time.timeScale = 0f;
            previousState = currentState;
            ChangeState(GameState.Paused);
            pauseScreen.SetActive(true);
            PlayerController.Instance.animator.SetBool("canMove", false);
        }
    }
  

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            Time.timeScale = 1f;
            ChangeState(previousState);
            pauseScreen.SetActive(false);
            PlayerController.Instance.animator.SetBool("canMove", true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MetWithQuestion()
    {
        Time.timeScale = 0f;
        ChangeState(GameState.Question);
        questionScreen.SetActive(true);
        GameObject questionObject = GameObject.FindGameObjectWithTag("QuestionText");
        if (questionObject == null)
        {
            Debug.LogError("QuestionText object not found.");
            return;
        }

        GameObject[] answers = GameObject.FindGameObjectsWithTag("QuestionAnswer");
        if (answers.Length == 0)
        {
            Debug.LogError("QuestionAnswer objects not found.");
            return;
        }

        GameObject correctAnswer = GameObject.FindGameObjectWithTag("CorrectAnswer");
        if (correctAnswer == null)
        {
            Debug.LogError("CorrectAnswer object not found.");
            return;
        }
        QuestionList questions = new QuestionList();
        List<Question> questionList = questions.questions;

        if (questionScreen.GetComponentInChildren<TextMeshProUGUI>() != null)
        {
            questionObject.GetComponentInChildren<TextMeshProUGUI>().text = questionList[SceneManager.GetActiveScene().buildIndex].questionText;
            for (int i = 0; i < answers.Length; i++)
            {
                answers[i].GetComponentInChildren<TextMeshProUGUI>().text = questionList[SceneManager.GetActiveScene().buildIndex].answers[i];
            }
            correctAnswer.GetComponentInChildren<TextMeshProUGUI>().text = questionList[SceneManager.GetActiveScene().buildIndex].correctAnswer;
        }
    }

    public void OnAnswerCorrect()
    {
        previousState = GameState.Gameplay;
        ChangeState(GameState.Gameplay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnAnswerIncorrect()
    {
        RestartGame();
    }

    public void OnChestOpened()
    {
        Debug.Log("ADD A DAMN EVENT PLS");
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started && (currentState == GameState.Paused || currentState == GameState.Gameplay))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void DisableScreen()
    {
        pauseScreen.SetActive(false);
        questionScreen.SetActive(false);
    }
}
