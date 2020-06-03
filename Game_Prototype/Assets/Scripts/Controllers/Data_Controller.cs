using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Data_Controller : MonoBehaviour
{
    //Karakter prefabok és aktív prefab id
	public List<Character_Controller> player;
	public int playerID = 0;

    //Aktív pálya id
    public int levelId = 0;

    //Karakter pozíciója, defaultban [0,0,0]
    public float[] position = new  float[3] {0f,0f,0f};

    //A lehetséges ellenfelek asset listája
	public List<Character> enemyList;

    //A lehetséges pályák listája
	public List<GameObject> levels;

    //A lehetséges elemek listája és aktuális ellenség
	public List<Elements> elements;
    Character enemy;

    //Interaction range-ben lévő karakter helye
    public NPC_Controller interactableCharacter;

    public Player_Data data;

    

    void Start()
    {
        //Az adatvezérlő fixen végigköveti az alkalmazás életciklusát
        DontDestroyOnLoad (transform.gameObject);

        //Képernyőarányok fixálása
        Screen.SetResolution(1920,1080,true);
    }

    //Megváltoztatja az aktív karakter id-t és az aktív karakter nevét és típusát
    public void SetCharacter(int id, string name, int elementID)
    {
        ChangePlayerId(id);
    	this.player[this.playerID].ChangeCharacter(name, elements[elementID]);
    }

    //Az aktív karakter ID-ja a karakter prefab tömbben.
    public int getPlayerID()
    {
    	return this.playerID;
    }

    public void ChangePlayerId(int newId)
    {
        this.playerID = newId;
    }

    //Az aktív pálya ID-ja a páyla tömbben
    public int GetLevelId()
    {
        return this.levelId;
    }

    public void ChangeLevelId(int newId)
    {
        this.levelId = newId;
    }

    //A karakter tárolt pozíciója az aktív pályán
    public void ChangePosition(float[] data)
    {
        this.position[0] = data[0];
        this.position[1] = data[1];
        this.position[2] = data[2];
    }

    public float[] getPositions()
    {
        return this.position;
    }

    //Betölti a játék mentett adatait és eltárolja az adatvezérlőbe
    public void LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "savegame.fun");

        //Ellenőrzi, hogy van-e már mentésünk
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            this.data = formatter.Deserialize(stream) as Player_Data;

            stream.Close();

            //Megnézzük, hogy a betöltött adatok érvényesek-e
            if(this.data != null)
            {
                ChangePosition(this.data.position);
                ChangePlayerId(this.data.playerId);
                ChangeLevelId(this.data.levelId);
            }

            else
            {
                Debug.Log("Loading the game was unsuccessful!");
            }

        } 

        //Ha nincs még mentés file
        else
        {
            Debug.Log("Save file not found!");
        }
    }

    public Player_Data GetData()
    {
        return this.data;
    }

    public void DeleteData()
    {
        this.data = null;
    }


    //Karakter és játék adatok elmentése
    public void SaveGame(Character_Controller player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "savegame.fun");
        FileStream stream = new FileStream(path, FileMode.Create);

        Player_Data data = new Player_Data(player, this.playerID, this.levelId);

        formatter.Serialize(stream,data);
        stream.Close();
        Debug.Log("Save Finished!");
    }
}

