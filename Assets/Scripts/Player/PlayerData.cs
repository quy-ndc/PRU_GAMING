using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float health;
    public Vector2 position;
    public bool isFacingRight;
    public bool isAlive;
    public int currentLevel;

    public PlayerData(PlayerController player)
    {
        health = player.Health;
        position = player.rb.position;
        isFacingRight = player.IsFacingRight;
        isAlive = player.IsAlive;
        currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }
}