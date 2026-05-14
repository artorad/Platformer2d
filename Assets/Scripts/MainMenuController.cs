using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [System.Serializable]
    public struct LevelUI
    {
        public Button LevelButton;
        public GameObject LockedIcon;
        public int BuildIndex;
        public int UnlockCost;
    }

    [SerializeField] private LevelUI[] _levels;
    [SerializeField] private Text _totalCoinsText;

    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        _totalCoinsText.text = $"Coins: {GameDataManager.TotalCoins}";

        foreach (var level in _levels)
        {
            bool isUnlocked = GameDataManager.IsLevelUnlocked(level.BuildIndex);

            level.LockedIcon.SetActive(!isUnlocked);
            level.LevelButton.onClick.RemoveAllListeners();

            if (isUnlocked)
            {
                level.LevelButton.onClick.AddListener(() => LoadLevel(level.BuildIndex));
            }
            else
            {
                level.LevelButton.onClick.AddListener(() => TryBuyLevel(level));
            }
        }
    }

    private void TryBuyLevel(LevelUI level)
    {
        if (GameDataManager.TotalCoins >= level.UnlockCost)
        {
            GameDataManager.TotalCoins -= level.UnlockCost;
            GameDataManager.UnlockLevel(level.BuildIndex);
            GameDataManager.SaveData();
            UpdateUI();
        }
    }

    private void LoadLevel(int index)
    {
        SessionData.CurrentLevelCoins = 0;
        SceneManager.LoadScene(index);
    }
}