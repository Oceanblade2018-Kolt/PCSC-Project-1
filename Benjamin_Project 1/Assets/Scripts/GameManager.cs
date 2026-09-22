using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public PlayerController player;

    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI clipText;
    public TextMeshProUGUI ammoText;
    public Image healthBar;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        weaponName = GameObject.Find("Weapon Name").GetComponentInParent<TextMeshProUGUI>();
        clipText = GameObject.Find("Ammo").GetComponentInParent<TextMeshProUGUI>();
        ammoText = GameObject.Find("Mag").GetComponentInParent<TextMeshProUGUI>();

        healthBar = GameObject.Find("HB").GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (float) player.health / (float) player.maxHealth;
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
