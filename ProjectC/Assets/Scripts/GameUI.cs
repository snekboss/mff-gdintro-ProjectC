using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject panelPauseMenu;
    public Button btnExitToMainMenu;
    public Text txtPauseMenu;

    public Text txtWeaponInfo;
    public Text txtReloading;
    public Text playerHealth;

    public Player player;

    bool isGamePaused;

    float isReloadingLerpRate = 20f;
    float colorSimilarityEpsilon = 0.1f;
    int colorReloadTargetIndex;
    Color[] colorsReloadFlashing = new Color[2];


    void OnExitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        colorsReloadFlashing[0] = txtReloading.color;
        colorsReloadFlashing[1] = Color.red;

        colorReloadTargetIndex = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            OnPauseMenuButtonPressed();
        }

        if (player.isReloading())
        {
            txtReloading.gameObject.SetActive(true);
            PlayReloadFlashingTextAnimThingBro();
        }
        else
        {
            txtReloading.gameObject.SetActive(false);
        }
    }

    void OnPauseMenuButtonPressed()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            panelPauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            panelPauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void PlayReloadFlashingTextAnimThingBro()
    {
        txtReloading.color = Color.Lerp(txtReloading.color, colorsReloadFlashing[colorReloadTargetIndex], isReloadingLerpRate * Time.deltaTime);

        if (ColorsAreSimilarEnough())
        {
            colorReloadTargetIndex++;
            colorReloadTargetIndex %= colorsReloadFlashing.Length;
        }
    }

    bool ColorsAreSimilarEnough()
    {
        Color targetCol = colorsReloadFlashing[colorReloadTargetIndex];
        Vector3 src = new Vector3(txtReloading.color.r, txtReloading.color.g, txtReloading.color.b);
        Vector3 dest = new Vector3(targetCol.r, targetCol.g, targetCol.b);

        float distance = Vector3.Distance(src, dest);

        return distance < colorSimilarityEpsilon;
    }
}
