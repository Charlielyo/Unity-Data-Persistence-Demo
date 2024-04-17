using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //���� PlayerPrefs �д洢���ݵļ���
    private const string ScoreKey = "PlayerScore";
    private const string VolumeKey = "GameVolume";
    private const string HighScoreKey = "HighScore";
    private const string PlayerNameKey = "PlayerName";

    //�洢��ҵķ�������������߷���������,�Լ�������ʱ�洢���ص�����
    private int playerScore;
    private float gameVolume;
    private int highScore;
    private string playerName = "Player";

    private int savedPlayerScore;
    private float savedGameVolume;
    private int savedHighScore;
    private string savedPlayerName;

    //������ʾ������������������������ʾ������ֵ������������Ƶ� UI Ԫ��
    public Text scoreText;
    public Slider volumeSlider;
    public Text volumeValueText;
    public InputField playerNameInput;

    void Start()
    {
        //����֮ǰ��������ݲ����� UI
        LoadData();
        UpdateUI();
    }

    void Update()
    {
        //��ⰴ�¿ո��,�������,�����ӷ��������� UI
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerScore += 10;
            Debug.Log("Score: " + playerScore);
            UpdateUI();
        }
    }

    private void SaveData()
    {
        //����ǰ�ķ�������������߷�����������Ʊ��浽 PlayerPrefs ��
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
        //���ڴ� PlayerPrefs �м���֮ǰ��������ݡ����Ƚ����ص����ݴ洢����ʱ������,Ȼ����Щֵ����ʵ�ʵı�����
        //��������ȷ�����ص����ݲ��ᱻ����
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
        //������ʾ������������������������ֵ��������Ƶ� UI Ԫ��
        scoreText.text = "Score: " + playerScore;
        volumeSlider.value = gameVolume;
        volumeValueText.text = gameVolume.ToString("0.00");
        playerNameInput.text = playerName;
    }

    private void ApplyVolume()
    {
        //Ӧ�õ�ǰ����������,������ֻ�Ǵ�ӡ��־
        Debug.Log("Game volume set to: " + gameVolume);
    }

    public void SetVolume(float volume)
    {
        //�������������¼��ص�����,���ڸ�������ֵ��Ӧ����������
        gameVolume = volume;
        ApplyVolume();
        UpdateUI();
    }

    public void SaveSettings()
    {
        //���浱ǰ����Ϸ����
        SaveData();
    }

    public void LoadSettings()
    {
        //����֮ǰ���������,Ȼ����� UI ��Ӧ����������
        LoadData();
        UpdateUI();
        ApplyVolume();
    }

    public void DeleteData()
    {
        //ɾ�����б��������,������ΪĬ��ֵ����ʹ�� PlayerPrefs.DeleteAll() ɾ�����б���ļ�ֵ�ԡ�
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
        //��������������¼��ص�����,���ڸ���������Ʋ��������ݡ�
        playerName = name;
        SaveData();
        UpdateUI();
    }
}