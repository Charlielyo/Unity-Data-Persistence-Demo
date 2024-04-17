using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.EventSystems;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int playerScore;
    public int playerLevel;
    public bool[] unlockedItems;
    public int gameProgress;
}

public class FileIO : MonoBehaviour
{
    public InputField nameInput;
    public Text scoreText;
    public Text levelText;
    public Button saveButton;
    public Button loadButton;
    public Button openFolderButton;
    public Button deleteButton;

    private string filePath;
    private PlayerData playerData = new PlayerData();

    void Start()
    {
        filePath = Application.persistentDataPath + "/player.dat";
        LoadPlayerData();
        UpdateUI();

        saveButton.onClick.AddListener(SavePlayerData);
        loadButton.onClick.AddListener(LoadPlayerData);
        openFolderButton.onClick.AddListener(OpenPersistentDataFolder);
        deleteButton.onClick.AddListener(DeletePlayerData);
    }

    void Update()
    {
        // 按空格键增加分数
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerData.playerScore += 10;
            UpdateLevel();
            UpdateUI();
        }
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();
    }

    private void SavePlayerData()
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Create);

            // 添加版本控制
            formatter.Serialize(stream, Application.version);
            formatter.Serialize(stream, playerData);

            stream.Close();
            Debug.Log("Player data saved successfully!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save player data: {e.Message}");
        }
    }

    private void LoadPlayerData()
    {
        if (File.Exists(filePath))
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(filePath, FileMode.Open);

                // 检查版本
                string version = formatter.Deserialize(stream) as string;
                if (version != Application.version)
                {
                    Debug.LogWarning("Player data version mismatch. Data may be incompatible.");
                }

                playerData = (PlayerData)formatter.Deserialize(stream);
                stream.Close();
                UpdateUI();
                Debug.Log("Player data loaded successfully!");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load player data: {e.Message}");
            }
        }
    }

    private void UpdateUI()
    {
        nameInput.text = playerData.playerName;
        scoreText.text = $"Score: {playerData.playerScore}";
        levelText.text = $"Level: {playerData.playerLevel}";
    }

    private void UpdateLevel()
    {
        // 简单的等级更新逻辑,每积累100分升一级
        playerData.playerLevel = playerData.playerScore / 100 + 1;
    }

    private void OpenPersistentDataFolder()
    {
        string folderPath = Application.persistentDataPath;
        System.Diagnostics.Process.Start(folderPath);
    }

    private void DeletePlayerData()
    {
        playerData.playerName = "";
        playerData.playerScore = 0;
        playerData.playerLevel = 1;
        playerData.unlockedItems = new bool[5];
        playerData.gameProgress = 0;
        SavePlayerData();
        UpdateUI();
        Debug.Log("Player data deleted successfully!");
    }
}