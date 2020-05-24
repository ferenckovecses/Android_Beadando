using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Save_Controller
{
    public static void SaveGame(Character_Controller player, int playerId)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "savegame.fun");
        FileStream stream = new FileStream(path, FileMode.Create);

        Player_Data data = new Player_Data(player, playerId);

        formatter.Serialize(stream,data);
        stream.Close();

    }

    public static Player_Data LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "savegame.fun");

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Player_Data data = formatter.Deserialize(stream) as Player_Data;

            stream.Close();

            return data;

        }

        else
        {
            Debug.Log("Save File not found!");
            return null;
        }
    }
}

