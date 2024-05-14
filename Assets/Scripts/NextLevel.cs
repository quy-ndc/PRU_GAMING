using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField]
    public Scene currentScene;
    public int maxScene;

    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && currentScene.buildIndex < maxScene)
        {
            SceneManager.LoadScene(currentScene.buildIndex + 1, LoadSceneMode.Single);
        }
        else
        {
            Debug.Log("Last level reached");
        }
    }
}
