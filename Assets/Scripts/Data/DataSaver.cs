#region

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

#endregion

public static class DataSaver
{
    //Save Data
    public static void SaveData<T>(T dataToSave, string dataFileName)
    {
        var tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Convert To Json then to bytes
        var jsonData = JsonConvert.SerializeObject(dataToSave);
        jsonData = JsonPrettify(jsonData);
        var jsonByte = Encoding.UTF8.GetBytes(jsonData);

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
        //Debug.Log(path);

        try
        {
            File.WriteAllBytes(tempPath, jsonByte);
            Debug.Log("Saved Data to: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To PlayerInfo Data to: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }

    //Load Data
    public static T LoadData<T>(string dataFileName)
    {
        var tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Debug.LogWarning("Directory does not exist");
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "data"));
            File.Create(tempPath);
            return default;
        }

        if (!File.Exists(tempPath))
        {
            Debug.Log("File does not exist");
            File.Create(tempPath);
            return default;
        }

        //Load saved Json
        byte[] jsonByte = null;
        try
        {
            jsonByte = File.ReadAllBytes(tempPath);
            Debug.Log("Loaded Data from: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        //Convert to json string
        var jsonData = Encoding.UTF8.GetString(jsonByte);

        //Convert to Object
        object resultValue = JsonConvert.DeserializeObject<T>(jsonData);
        var returnVar = (T) Convert.ChangeType(resultValue, typeof(T));
        return returnVar;
    }

    public static bool DeleteData(string dataFileName)
    {
        var success = false;

        //Load Data
        var tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Debug.LogWarning("Directory does not exist");
            return false;
        }

        if (!File.Exists(tempPath))
        {
            Debug.Log("File does not exist");
            return false;
        }

        try
        {
            File.Delete(tempPath);
            Debug.Log("Data deleted from: " + tempPath.Replace("/", "\\"));
            success = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Delete Data: " + e.Message);
        }

        return success;
    }

    public static string JsonPrettify(string json)
    {
        using (var stringReader = new StringReader(json))
        using (var stringWriter = new StringWriter())
        {
            var jsonReader = new JsonTextReader(stringReader);
            var jsonWriter = new JsonTextWriter(stringWriter) {Formatting = Formatting.Indented};
            jsonWriter.WriteToken(jsonReader);
            return stringWriter.ToString();
        }
    }
}