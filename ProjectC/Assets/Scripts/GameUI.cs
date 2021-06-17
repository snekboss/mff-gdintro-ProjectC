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

    public RectTransform panelPlayerHealth;
    int panelPlayerHealthMaxWidth;
    Image imagePlayerHealth;

    public GameObject panelGameOverScreen;
    public Text txtGameOver;
    public bool isGameOver = false;

    public Player player;

    bool isGamePaused;

    float isReloadingLerpRate = 20f;
    float colorSimilarityEpsilon = 0.1f;
    int colorReloadTargetIndex;
    Color[] colorsReloadFlashing = new Color[2];

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        colorsReloadFlashing[0] = txtReloading.color;
        colorsReloadFlashing[1] = Color.red;

        panelPlayerHealthMaxWidth = (int)panelPlayerHealth.rect.width;
        imagePlayerHealth = panelPlayerHealth.GetComponent<Image>();

        colorReloadTargetIndex = 0;
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        if (player.isDead)
        {
            OnShowPlayerStatsButtonPressed();
            return;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            OnPauseMenuButtonPressed();
        }

        if (player.isReloading() && isGamePaused == false)
        {
            txtReloading.gameObject.SetActive(true);
            PlayReloadFlashingTextAnimThingBro();
        }
        else
        {
            txtReloading.gameObject.SetActive(false);
        }

        // Player health
        float playerHealthRatio = (float)player.playerHealth / player.playerMaxHealth;
        panelPlayerHealth.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, playerHealthRatio * panelPlayerHealthMaxWidth);

        imagePlayerHealth.color = Color.Lerp(Color.red, Color.green, playerHealthRatio);

        // Weapon info
        Weapon curWeapon = player.getCurrentWeapon();
        if (curWeapon != null)
        {
            txtWeaponInfo.text = string.Format("{0}: {1}/{2} ({3})",
                curWeapon.weaponName, curWeapon.curAmmoInMag, curWeapon.magazineCapacity, curWeapon.remainingAmmo);
        }
        else
        {
            txtWeaponInfo.text = "No weapon";
        }
    }

    void OnPauseMenuButtonPressed()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            panelPauseMenu.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
        }
        else
        {
            panelPauseMenu.SetActive(false);
            Time.timeScale = 1;
            Cursor.visible = false;
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

    public void OnShowPlayerStatsButtonPressed()
    {
        panelPauseMenu.SetActive(false);
        panelGameOverScreen.SetActive(true);

        txtGameOver.text = string.Format("Game Over\n\n{0}", PlayerStats.PlayerStatsSingleton.GetPlayerStats());
        Time.timeScale = 0;
        isGameOver = true;
    }

    public void OnGameOverButtonPressed()
    {
        PlayerStats.PlayerStatsSingleton = null;
        SceneManager.LoadScene("Main Menu");
    }
}
