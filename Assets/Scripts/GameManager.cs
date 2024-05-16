using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        Question,

    }

    public GameState currentState;
    public GameState previousState;

    [Header("UI")]
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private GameObject questionScreen;

    public static GameManager Instance;

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

    //private void Update()
    //{
    //    switch (currentState)
    //    {
    //        case GameState.Gameplay:
    //            break;
    //        case GameState.Paused:
    //            break;
    //        case GameState.GameOver:
    //            break;
    //        default:
    //            Debug.LogWarning("No Such state exist");
    //            break;
    //    }
    //}

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            PlayerController.Instance.animator.SetBool("canMove", false);
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            PlayerController.Instance.animator.SetBool("canMove", true);
        }
    }

    public void MetWithQuestion()
    {
        ChangeState(GameState.Question);
        questionScreen.SetActive(true);
        Time.timeScale = 0f;
        GameObject questionObject = GameObject.FindGameObjectWithTag("QuestionText");
        GameObject[] answers = GameObject.FindGameObjectsWithTag("QuestionAnswer");
        GameObject correctAnswer = GameObject.FindGameObjectWithTag("CorrectAnswer");
        QuestionList questions = new QuestionList();
        List<Question> questionList = questions.questions;

        if (questionScreen.GetComponentInChildren<TextMeshProUGUI>() != null)
        {
            questionObject.GetComponentInChildren<TextMeshProUGUI>().text = questionList[SceneManager.GetActiveScene().buildIndex].questionText;
            for (int i = 0; i < answers.Length;  i++)
            {
                answers[i].GetComponentInChildren<TextMeshProUGUI>().text = questionList[SceneManager.GetActiveScene().buildIndex].answers[i];
            }
            correctAnswer.GetComponentInChildren<TextMeshProUGUI>().text = questionList[SceneManager.GetActiveScene().buildIndex].correctAnswer;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Ensure time scale is reset to normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnAnswerCorrect()
    {
        previousState = currentState;
        ChangeState(GameState.Gameplay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnAnswerIncorrect()
    {
        RestartGame();
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
