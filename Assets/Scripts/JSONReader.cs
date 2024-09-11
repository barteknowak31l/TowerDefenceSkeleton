using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    public static JSONReader Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;

    }

    public T Read<T>(string filename) where T : IJsonReadable
    {
        string path = Path.Combine(Application.streamingAssetsPath, filename + ".json");
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            T data = JsonUtility.FromJson<T>(jsonString);

            if (data == null)
            {
                Debug.LogError("Failed to deserialize JSON data.");
            }
            else
            {
                Debug.Log("Successfully loaded and parsed JSON data.");
            }

            return data;
        }
        else
        {
            Debug.LogError("File not found: " + path);
            return default;
        }
    }
}

