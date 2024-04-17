using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //用作 PlayerPrefs 中存储数据的键名
    private const string ScoreKey = "PlayerScore";
    private const string VolumeKey = "GameVolume";
    private const string HighScoreKey = "HighScore";
    private const string PlayerNameKey = "PlayerName";

    //存储玩家的分数、音量、最高分数和名称,以及用于临时存储加载的数据
    private int playerScore;
    private float gameVolume;
    private int highScore;
    private string playerName = "Player";

    private int savedPlayerScore;
    private float savedGameVolume;
    private int savedHighScore;
    private string savedPlayerName;

    //用于显示分数、设置音量滑动条、显示音量数值和输入玩家名称的 UI 元素
    public Text scoreText;
    public Slider volumeSlider;
    public Text volumeValueText;
    public InputField playerNameInput;

    void Start()
    {
        //加载之前保存的数据并更新 UI
        LoadData();
        UpdateUI();
    }

    void Update()
    {
        //检测按下空格键,如果按下,则增加分数并更新 UI
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerScore += 10;
            Debug.Log("Score: " + playerScore);
            UpdateUI();
        }
    }

    private void SaveData()
    {
        //将当前的分数、音量、最高分数和玩家名称保存到 PlayerPrefs 中
        PlayerPrefs.SetInt(ScoreKey, playerScore);
        PlayerPrefs.SetFloat(VolumeKey, gameVolume);
        PlayerPrefs.SetInt(HighScoreKey, highScore);

        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString(PlayerNameKey, playerName);
        }
        else
        {
            PlayerPrefs.DeleteKey(PlayerNameKey);
        }

        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        //用于从 PlayerPrefs 中加载之前保存的数据。首先将加载的数据存储在临时变量中,然后将这些值赋给实际的变量。
        //这样可以确保加载的数据不会被覆盖
        savedPlayerScore = PlayerPrefs.GetInt(ScoreKey, 0);
        savedGameVolume = PlayerPrefs.GetFloat(VolumeKey, 0.5f);
        savedHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        if (PlayerPrefs.HasKey(PlayerNameKey))
        {
            savedPlayerName = PlayerPrefs.GetString(PlayerNameKey);
        }
        else
        {
            savedPlayerName = "Player";
        }

        playerScore = savedPlayerScore;
        gameVolume = savedGameVolume;
        highScore = savedHighScore;
        playerName = savedPlayerName;
    }

    private void UpdateUI()
    {
        //更新显示分数、音量滑动条、音量数值和玩家名称的 UI 元素
        scoreText.text = "Score: " + playerScore;
        volumeSlider.value = gameVolume;
        volumeValueText.text = gameVolume.ToString("0.00");
        playerNameInput.text = playerName;
    }

    private void ApplyVolume()
    {
        //应用当前的音量设置,在这里只是打印日志
        Debug.Log("Game volume set to: " + gameVolume);
    }

    public void SetVolume(float volume)
    {
        //音量滑动条的事件回调函数,用于更新音量值并应用音量设置
        gameVolume = volume;
        ApplyVolume();
        UpdateUI();
    }

    public void SaveSettings()
    {
        //保存当前的游戏设置
        SaveData();
    }

    public void LoadSettings()
    {
        //加载之前保存的数据,然后更新 UI 并应用音量设置
        LoadData();
        UpdateUI();
        ApplyVolume();
    }

    public void DeleteData()
    {
        //删除所有保存的数据,并重置为默认值。它使用 PlayerPrefs.DeleteAll() 删除所有保存的键值对。
        PlayerPrefs.DeleteAll();
        playerScore = 0;
        gameVolume = 0.5f;
        highScore = 0;
        playerName = "Player";
        UpdateUI();
        ApplyVolume();
    }

    public void SetPlayerName(string name)
    {
        //玩家名称输入框的事件回调函数,用于更新玩家名称并保存数据。
        playerName = name;
        SaveData();
        UpdateUI();
    }
}