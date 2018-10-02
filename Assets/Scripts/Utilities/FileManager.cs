using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public static class FileManager
{
    public static string SaveSettings(string fileName)
    {
        string tmpName = fileName;
        if (fileName == "")
            fileName = "PathfinderSettings";

        string gamePath = Application.dataPath;
        fileName = gamePath + "/" + fileName;

        int numOfFiles = 1;
        string uniqueFileName = fileName + ".txt";
        while (File.Exists(uniqueFileName))
        {
            numOfFiles++;
            uniqueFileName = fileName + "(" + numOfFiles + ").txt";
        }

        if (numOfFiles > 1)
            tmpName = tmpName + "(" + numOfFiles + ")";

        LevelSettings settings = UISettings.settings;
        var jsonConvert = JsonConvert.SerializeObject(settings);

        try
        {
            File.WriteAllText(uniqueFileName, jsonConvert);
            return "Saving file " + tmpName + " successful!";
        }
        catch (Exception e)
        {
            return "Saving file " + tmpName + " failed";
        }
    }

    public static string LoadSettings(string fileName)
    {
        string tmpname = fileName;
        string gamePath = Application.dataPath;
        fileName = gamePath + "/" + fileName + ".txt";
        string fileText;

        if (File.Exists(fileName))
        {
            try
            {
                fileText = File.ReadAllText(fileName);
            }
            catch (Exception e)
            {
                return "Loading file " + tmpname + " failed";
            }
        }
        else
            return "File " + tmpname + " does not exist";

        LevelSettings loadedSettings =  JsonConvert.DeserializeObject<LevelSettings>(fileText);

        UISettings.settings = loadedSettings;
        return "Loading file " + tmpname + " successful!";
    }
}