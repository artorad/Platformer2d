using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelFinish : MonoBehaviour
{
    [SerializeField] private int _nextLevelBuildIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SessionData.CommitSessionCoins();
            GameDataManager.UnlockLevel(_nextLevelBuildIndex);
            GameDataManager.SaveData();

            SceneManager.LoadScene(_nextLevelBuildIndex);
        }
    }
}