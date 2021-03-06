using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Text txtEasy;
    public Text txtHard;
    public Text txtMouseSensitivity;
    public Slider sliderMouseSensitivity;

    public Text txtGameDifficulty;
    public Slider sliderGameDifficulty;

    public Button btnStartGame;
    public Button btnControls;
    public Button btnExitGame;
    public Button btnGoBack;
    public GameObject panelControls;

    static bool isFirstTimeLoad = true;

    void Start()
    {
        if (isFirstTimeLoad)
        {
#if (UNITY_STANDALONE)
            PlayerStats.MouseSensitivity = 90f;
#elif (UNITY_WEBGL)
            PlayerStats.MouseSensitivity = 15f;
#endif
        }

        sliderMouseSensitivity.value = PlayerStats.MouseSensitivity;
        txtMouseSensitivity.text = "Mouse Sensitivity: " + PlayerStats.MouseSensitivity.ToString("0.00");

        sliderGameDifficulty.value = PlayerStats.GameDifficulty;
        txtGameDifficulty.text = "Game Difficulty: " + PlayerStats.GameDifficulty.ToString("0.00");

        isFirstTimeLoad = false;
    }

    public void OnStartGame()
    {
        SceneManager.LoadScene("Game Scene");
        Time.timeScale = 1;
    }

    public void OnExitGame()
    {
#if (UNITY_STANDALONE) 
        Application.Quit();
#elif (UNITY_WEBGL)
        Application.OpenURL("about:blank");
#endif
    }

    public void OnControls()
    {
        panelControls.SetActive(true);
        btnGoBack.gameObject.SetActive(true);

        btnStartGame.gameObject.SetActive(false);
        btnControls.gameObject.SetActive(false);
        btnExitGame.gameObject.SetActive(false);
        sliderGameDifficulty.gameObject.SetActive(false);
        txtGameDifficulty.gameObject.SetActive(false);
        txtEasy.gameObject.SetActive(false);
        txtHard.gameObject.SetActive(false);
    }

    public void OnMouseSensitivitySliderValueChanged()
    {
        PlayerStats.MouseSensitivity = sliderMouseSensitivity.value;
        txtMouseSensitivity.text = "Mouse Sensitivity: " + PlayerStats.MouseSensitivity.ToString("0.00");
    }

    public void OnGameDifficultySliderValueChanged()
    {
        PlayerStats.GameDifficulty = sliderGameDifficulty.value;
        txtGameDifficulty.text = "Game Difficulty: " + PlayerStats.GameDifficulty.ToString("0.00");
    }

    public void OnGoBack()
    {
        panelControls.SetActive(false);
        btnGoBack.gameObject.SetActive(false);

        btnStartGame.gameObject.SetActive(true);
        btnControls.gameObject.SetActive(true);
        btnExitGame.gameObject.SetActive(true);
        sliderGameDifficulty.gameObject.SetActive(true);
        txtGameDifficulty.gameObject.SetActive(true);
        txtEasy.gameObject.SetActive(true);
        txtHard.gameObject.SetActive(true);
    }
}
