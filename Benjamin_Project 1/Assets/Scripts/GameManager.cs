using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public PlayerController player;

    public GameObject PauseMenu;
    public GameObject GameOver;

    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI clipText;
    public TextMeshProUGUI ammoText;
    public Image healthBar;
    public Button restart;

    public bool paused = false;
    public bool gameOver = false;
    public bool deadified = false;
    public bool mainMenuClicked = false;
    public int currentLevel = 1;


    void Start()
    {
        Time.timeScale = 1;

        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            PauseMenu = GameObject.FindGameObjectWithTag("Pause");
            GameOver = GameObject.FindGameObjectWithTag("GameOver");
            

            weaponName = GameObject.Find("Weapon Name").GetComponentInParent<TextMeshProUGUI>();
            clipText = GameObject.Find("Ammo").GetComponentInParent<TextMeshProUGUI>();
            ammoText = GameObject.Find("Mag").GetComponentInParent<TextMeshProUGUI>();

            healthBar = GameObject.Find("HB").GetComponent<Image>();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            PauseMenu.SetActive(false);
            GameOver.SetActive(false);
        }





    }

    // Update is called once per frame
    void Update()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        //Debug.Log("Scene: " +  currentLevel);
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {

            if (paused && !deadified)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                Time.timeScale = 0;

                PauseMenu.SetActive(true);
            }
            else if (!paused && !deadified)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;


                PauseMenu.SetActive(false);
            }

            if (player.health <= 0 && !deadified)
            {
                Dead();
                //deadified = true;
                if (gameOver)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;

                    Time.timeScale = 0;

                    GameOver.SetActive(true);
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;

                    Time.timeScale = 1;

                    GameOver.SetActive(false);
                }

            }


            healthBar.fillAmount = (float)player.health / (float)player.maxHealth;
            if (player.currentWeapon)
            {
                weaponName.text = player.currentWeapon.name;
                clipText.text = "Clip: " + player.currentWeapon.clip + '/' + player.currentWeapon.clipSize;
                ammoText.text = "Ammo: " + player.currentWeapon.ammo + '/' + player.currentWeapon.maxAmmo;
            }
            else
            {
                weaponName.text = "";
                clipText.text = "";
                ammoText.text = "";
            }
        }


    }

    public void LoadLevel(int levelID)
    {
        if (deadified && !mainMenuClicked)
        {
            SceneManager.LoadScene(currentLevel);
            deadified = false;
        }
        else
        {
            if (levelID > SceneManager.sceneCountInBuildSettings)
                Debug.Log("Scene ID too high: " + levelID);
            else
                SceneManager.LoadScene(levelID);
        }
        mainMenuClicked = false;
    }
    public void MainMenu()
    {
        mainMenuClicked = true;
        LoadLevel(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        paused = !paused;
        Cursor.visible = paused;
        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }

        PauseMenu.SetActive(paused);
    }
    public void Dead()
    {
        deadified = !deadified;
        gameOver = !gameOver;
        Cursor.visible = gameOver;
        if (gameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
        GameOver.SetActive(gameOver);
    }
}
