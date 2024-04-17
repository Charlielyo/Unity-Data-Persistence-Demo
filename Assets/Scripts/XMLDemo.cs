using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using UnityEngine.UI;
using System.Diagnostics;

[System.Serializable]
public class GameProgress
{
    public int currentLevel = 1;
    public float playerHealth = 100f;
    public int[] coinsCollected = new int[10];
    public WeaponData equippedWeapon;
}

[System.Serializable]
public class WeaponData
{
    public string name = "Pistol";
    public int damage = 10;
    public int ammoAmount = 100;
}

public class XMLDemo : MonoBehaviour
{
    public Text currentLevelText;
    public Slider playerHealthSlider;
    public Text[] coinTexts;
    public InputField weaponNameInputField;
    public InputField weaponDamageInputField;
    public InputField weaponAmmoInputField;
    public Button saveProgressButton;
    public Button openFileButton;

    private string filePath;
    private GameProgress progress;

    void Start()
    {
        filePath = Application.persistentDataPath + "/progress.xml";
        LoadProgress();
        UpdateUI();

        saveProgressButton.onClick.AddListener(SaveProgress);
        openFileButton.onClick.AddListener(OpenFile);

        weaponNameInputField.onEndEdit.AddListener(UpdateWeaponName);
        weaponDamageInputField.onEndEdit.AddListener(UpdateWeaponDamage);
        weaponAmmoInputField.onEndEdit.AddListener(UpdateWeaponAmmo);
    }

    public void SaveProgress()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GameProgress));
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            serializer.Serialize(stream, progress);
        }
    }

    private void LoadProgress()
    {
        if (File.Exists(filePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameProgress));
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                progress = (GameProgress)serializer.Deserialize(stream);
            }
            UpdateUI();
        }
        else
        {
            ResetGameProgress();
        }
    }

    private void ResetGameProgress()
    {
        progress = new GameProgress();
        UpdateUI();
    }

    public void IncreasePlayerHealth(float amount)
    {
        progress.playerHealth = Mathf.Clamp(progress.playerHealth + amount, 0f, 100f);
        playerHealthSlider.value = progress.playerHealth;
        SaveProgress();
    }

    public void CollectCoin(int index)
    {
        progress.coinsCollected[index]++;
        SaveProgress();
        UpdateCoinsUI();
    }

    private void UpdateUI()
    {
        UpdateCurrentLevelUI();
        UpdatePlayerHealthUI();
        UpdateCoinsUI();
        UpdateWeaponUI();
    }

    private void UpdateCurrentLevelUI()
    {
        currentLevelText.text = $"Current Level: {progress.currentLevel}";
    }

    private void UpdatePlayerHealthUI()
    {
        playerHealthSlider.value = progress.playerHealth;
    }

    private void UpdateCoinsUI()
    {
        for (int i = 0; i < coinTexts.Length; i++)
        {
            coinTexts[i].text = progress.coinsCollected[i].ToString();
        }
    }

    private void UpdateWeaponUI()
    {
        weaponNameInputField.text = progress.equippedWeapon.name;
        weaponDamageInputField.text = progress.equippedWeapon.damage.ToString();
        weaponAmmoInputField.text = progress.equippedWeapon.ammoAmount.ToString();
    }

    private void UpdateWeaponName(string name)
    {
        progress.equippedWeapon.name = name;
        SaveProgress();
    }

    private void UpdateWeaponDamage(string damageString)
    {
        if (int.TryParse(damageString, out int damage))
        {
            progress.equippedWeapon.damage = damage;
            SaveProgress();
        }
    }

    private void UpdateWeaponAmmo(string ammoString)
    {
        if (int.TryParse(ammoString, out int amount))
        {
            progress.equippedWeapon.ammoAmount = amount;
            SaveProgress();
        }
    }

    private void OpenFile()
    {
        Process.Start("explorer.exe", filePath.Replace("/", "\\"));
    }
}