using UnityEngine;

public static class GameDataManager
{
    private const string COINS_KEY = "TotalCoins";
    private const string LEVEL_PREFIX = "LevelUnl_";

    private static int _cachedCoins = -1;

    public static int TotalCoins
    {
        get
        {
            if (_cachedCoins < 0) _cachedCoins = PlayerPrefs.GetInt(COINS_KEY, 0);
            return _cachedCoins;
        }
        set
        {
            _cachedCoins = value;
            PlayerPrefs.SetInt(COINS_KEY, value);
        }
    }

    public static bool IsLevelUnlocked(int buildIndex)
    {
        if (buildIndex == 1) return true; // Индекс 0 — Меню, 1 — Первый уровень
        return PlayerPrefs.GetInt(LEVEL_PREFIX + buildIndex, 0) == 1;
    }

    public static void UnlockLevel(int buildIndex)
    {
        PlayerPrefs.SetInt(LEVEL_PREFIX + buildIndex, 1);
    }

    public static void SaveData() => PlayerPrefs.Save();
}

public static class SessionData
{
    public static int CurrentLevelCoins { get; set; } = 0;

    public static void CommitSessionCoins()
    {
        GameDataManager.TotalCoins += CurrentLevelCoins;
        CurrentLevelCoins = 0;
        GameDataManager.SaveData();
    }
}