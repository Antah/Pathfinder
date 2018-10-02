using System;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    public static UISettings instance = null;
    public static LevelSettings settings = new LevelSettings("default");
    public GameObject msgBox;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public LevelSettings GetSettings()
    {
        return settings;
    }

    public void RandomizeStartAndExit()
    {
        GameManager.instance.NewStartAndFinish();
    }

    public void SaveSettings(GameObject inputField)
    {
        string value = inputField.GetComponent<InputField>().text;
        string msg = FileManager.SaveSettings(value);
        SetMessage(msg);
    }

    public void LoadSettings(GameObject inputField)
    {
        string value = inputField.GetComponent<InputField>().text;
        string msg = FileManager.LoadSettings(value);
        SetMessage(msg);
    }

    public void NextLevel()
    {
        GameManager.instance.NextLevel();
    }

    public void ExitToMenu()
    {
        GameManager.instance.ExitToMenu();
    }

    public void ToggleWindow(GameObject window)
    {
        if (window.active)
            window.SetActive(false);
        else
            window.SetActive(true);
    }

    #region Cellular automaton section
    public void SetSeed(GameObject inputField)
    {
        string value = inputField.GetComponent<InputField>().text;
        settings.seed = value;
        SetMessage("New seed: \"" + value + "\"");
    }

    public void ToggleRandomSeed(GameObject toggle)
    {
        bool value = toggle.GetComponent<Toggle>().isOn;
        settings.useRandomSeed = value;
        string onOff = "off";
        if (value)
            onOff = "on";
        SetMessage("Random seed turned " + onOff);
    }

    public void SetLevelWidth(GameObject inputField)
    {
        string value = inputField.GetComponent<InputField>().text;
        int intValue = Int32.Parse(value);
        if(intValue < 10)
        {
            SetMessage("Level width set to 10, can't be lower");
            settings.levelWidth = 10;
        }
        else
        {
            settings.levelWidth = intValue;
            SetMessage("Level width set to " + value);
        }
    }

    public void SetLevelHeight(GameObject inputField)
    {
        string value = inputField.GetComponent<InputField>().text;
        int intValue = Int32.Parse(value);
        if (intValue < 10)
        {
            SetMessage("Level height set to 10, can't be lower");
            settings.levelHeight = 10;
        }
        else
        {
            settings.levelHeight = intValue;
            SetMessage("Level height set to " + value);
        }
    }

    public void SetWallFillPercent(GameObject slider)
    {
        float value = slider.GetComponent<Slider>().value;
        settings.wallFill = (int)value;
        SetMessage("Obstacles fill set to " + value + "%");
    }

    public void SetAlgorithm(GameObject dropdown)
    {
        Text value = dropdown.GetComponent<Dropdown>().captionText;
        if (value.text == "A*")
        {
            SetMessage("Pathfinding algorithm set to A*");
            GameManager.instance.SetAlgorithm(AlgortihmType.AStar);
        }
        else
        {
            SetMessage("Pathfinding algorithm set to Dijkstra");
            GameManager.instance.SetAlgorithm(AlgortihmType.Dijkstra);
        }

    }

    public void SetMessage(string msg)
    {
        instance.msgBox.GetComponentInChildren<Text>().text = msg;
    }
    #endregion
}

public enum AlgortihmType {AStar, Dijkstra }

public struct LevelSettings
{
    public LevelSettings(string presetSettings = "default")
    {
        seed = "default seed";
        useRandomSeed = true;
        levelWidth = 25;
        levelHeight = 25;
        wallFill = 35;
        algortihm = AlgortihmType.AStar;
    }

    //Seed settings
    public string seed;
    public bool useRandomSeed;

    //Level settings
    public int levelWidth, levelHeight;
    public int wallFill;

    public AlgortihmType algortihm;
}