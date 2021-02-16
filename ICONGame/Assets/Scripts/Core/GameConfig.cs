
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using System.Linq;

public class GameConfig
{
    public static GameConfig _instance = null;
    public static GameConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameConfig();
            }
            return _instance;
        }
    }
    Dictionary<string, string> properties = new Dictionary<string, string>();
    bool initialized = false;
    GameConfig()
    {
        properties.Clear();
        properties = new Dictionary<string, string>();
        string path = Path.Combine(Application.streamingAssetsPath, "Config");
        string fileName = "Config";
        string fullPath = Path.Combine(path, fileName);
        if (File.Exists(fullPath))
        {
            using (StreamReader reader = new StreamReader(fullPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] pair = line.Split('=');
                    if (pair.Length == 2)
                    {
                        if (!properties.ContainsKey(pair[0]))
                            properties.Add(pair[0], pair[1]);
                    }
                }
            }
        }
        initialized = true;
    }

    public static string GetProperty(string propertyName)
    {
        return Instance.properties[propertyName];
    }

    public static List<KeyValuePair<string, string>> GetAllProperties()
    {
        return Instance.properties.ToList();
    }
}



