using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{

    public Text txtMouseSensitivity;
    public Slider sliderMouseSensitivity;
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
    }

    public void OnSliderValueChanged()
    {
        PlayerStats.MouseSensitivity = sliderMouseSensitivity.value;
        txtMouseSensitivity.text = "Mouse Sensitivity: " + PlayerStats.MouseSensitivity.ToString("0.00");
    }

    public void OnGoBack()
    {
        panelControls.SetActive(false);
        btnGoBack.gameObject.SetActive(false);

        btnStartGame.gameObject.SetActive(true);
        btnControls.gameObject.SetActive(true);
        btnExitGame.gameObject.SetActive(true);
    }
}
