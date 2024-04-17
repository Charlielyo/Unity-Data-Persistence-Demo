using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public class GameSettings
{
    public float musicVolume = 1f;
    public int screenResolutionIndex = 0;
    public bool fullScreen = true;
    public int qualityLevel = 2;
}

public class JSONDemo : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public Dropdown resolutionDropdown;
    public Toggle fullScreenToggle;
    public Dropdown qualityDropdown;

    private string filePath;
    private GameSettings settings = new GameSettings();

    void Start()
    {
        filePath = Application.persistentDataPath + "/settings.json";
        LoadSettings();
        ApplySettings();
    }

    public void SaveSettings()
    {
        settings.musicVolume = musicVolumeSlider.value;
        settings.screenResolutionIndex = resolutionDropdown.value;
        settings.fullScreen = fullScreenToggle.isOn;
        settings.qualityLevel = qualityDropdown.value;

        string jsonData = JsonUtility.ToJson(settings);
        File.WriteAllText(filePath, jsonData);
    }

    public void LoadSettings()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            settings = JsonUtility.FromJson<GameSettings>(jsonData);
        }
    }

    public void ApplySettings()
    {
        musicVolumeSlider.value = settings.musicVolume;
        AudioListener.volume = settings.musicVolume;

        resolutionDropdown.value = settings.screenResolutionIndex;
        Resolution[] resolutions = Screen.resolutions;  
        resolutionDropdown.options.Clear();
        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData($"{resolution.width} x {resolution.height}"));
        }
        Screen.SetResolution(resolutions[settings.screenResolutionIndex].width, resolutions[settings.screenResolutionIndex].height, settings.fullScreen);

        fullScreenToggle.isOn = settings.fullScreen;

        qualityDropdown.value = settings.qualityLevel;
        QualitySettings.SetQualityLevel(settings.qualityLevel);
    }
}