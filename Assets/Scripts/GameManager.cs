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

    public GameState currentState;
    public GameState previousState;

    [Header("UI")]
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private GameObject questionScreen;

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
        questionScreen.SetActive(true);
        Debug.Log(questionScreen.GetComponentInChildren<TextMeshProUGUI>());

        if (questionScreen.GetComponentInChildren<TextMeshProUGUI>() != null)
        {
            questionScreen.GetComponentInChildren<TextMeshProUGUI>().text = "hi";
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Ensure time scale is reset to normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
