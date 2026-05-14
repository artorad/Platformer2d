using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private Text _coinsText;

    private bool _isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        _pauseMenuUI.SetActive(true);

        int totalDisplay = GameDataManager.TotalCoins + SessionData.CurrentLevelCoins;
        _coinsText.text = $"Total Coins: {totalDisplay}";
    }

    public void Resume()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        _pauseMenuUI.SetActive(false);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}