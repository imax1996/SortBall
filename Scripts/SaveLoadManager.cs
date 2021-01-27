using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveLoadManager {

    public static void SaveGame(object saveData) {
        BinaryFormatter formatter = new BinaryFormatter();

        if (!Directory.Exists(Application.persistentDataPath + "/saves")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/saves");
        }

        string path = Application.persistentDataPath + "/saves/" + "save.sad";
        FileStream file = File.Create(path);
        formatter.Serialize(file, saveData);
        file.Close();
    }

    public static object LoadGame() {
        if (!File.Exists(Application.persistentDataPath + "/saves/" + "save.sad")) {
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/saves/" + "save.sad", FileMode.Open);
        SaveData save = (SaveData)formatter.Deserialize(file);
        file.Close();

        return save;
    }
}
