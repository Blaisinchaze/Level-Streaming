using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class LevelManager : MonoBehaviour
{
    public enum GameMode { INGAME, INMENU, NULL}
    public GameObject player;
    public List<GameObject> loadedZones = new List<GameObject>();
    public List<GameObject> toBeDeleted = new List<GameObject>();
    private int playerPositionX;
    private int playerPositionY;
    private Vector2 playerPosition;
    public List<Vector2> loadedVectors = new List<Vector2>();
    public GameObject chunkManagerPrefab;
    public float loadDistance;
    public GameObject[] ZONESFORSAVING;
    public GameMode currentGameMode;


    public Button loadGame;
    public GameObject MenuUI;
    public GameObject InGameUI;
    public GameObject InGameMenuUI;

    private int score;
    public TextMeshProUGUI scoreText;

    Vector3 playerStartPosition;

#if (UNITY_EDITOR)
    [ContextMenu("Save Map")]
    void saveMap()
    {

        foreach (var item in ZONESFORSAVING)
        {
            GameObject TempChunkManager = GameObject.Instantiate(chunkManagerPrefab);
            List<GameObject> list = new List<GameObject>();
            foreach (Transform go in item.transform)
            {
                list.Add(go.gameObject);
            }
            TempChunkManager.GetComponent<chunkManager>().chunkReferences = list.ToArray();

            TempChunkManager.GetComponent<chunkManager>().jsonFileName = ((item.transform.position.x/300) + "," + (item.transform.position.z / 300));
            TempChunkManager.GetComponent<chunkManager>().saveMap();
            DestroyImmediate(TempChunkManager);
        }
    }
#endif

    private void Start()
    {
        playerStartPosition = player.transform.position;
        currentGameMode = GameMode.INMENU;
        var info = new DirectoryInfo(Application.dataPath + "/Resources/saveData/");
        var fileInfo = info.GetFiles();
        if (fileInfo.Length < 1)
        {
            loadGame.interactable = false;

        }
    }

    public void inGameMenu(bool active)
    {
        InGameMenuUI.SetActive(active);
    }

    public void SaveGame()
    {
        foreach (var item in loadedZones)
        {
            StartCoroutine(item.GetComponent<chunkManager>().SaveChunk());
        }
        string currentInstancePath = Application.dataPath + "/Resources/currentInstance";
        string saveDataPath = Application.dataPath + "/Resources/saveData";
        if (Directory.Exists(saveDataPath)) { Directory.Delete(saveDataPath, true); Directory.CreateDirectory(saveDataPath); }

        DirectoryInfo dir = new DirectoryInfo(currentInstancePath);
        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string tempPath = Path.Combine(saveDataPath, file.Name);
            file.CopyTo(tempPath, false);
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void ReturnToMenu()
    {
        score = 0;
        currentGameMode = GameMode.INGAME;
        InGameMenuUI.SetActive(false);
        MenuUI.SetActive(true);
        InGameUI.SetActive(false);
        var info = new DirectoryInfo(Application.dataPath + "/Resources/saveData/");
        var fileInfo = info.GetFiles();
        if (fileInfo.Length < 1)
        {
            loadGame.interactable = false;

        }
        else
        {
            loadGame.interactable = true;
        }
        foreach (var item in loadedZones)
        {

            Destroy(item);            
        }
        loadedZones.Clear();
        player.transform.position = playerStartPosition;
    }

    public void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = score + ".00";
    }

    public void NewGame(string dataFolder)
    {
        string loadDataPath = Application.dataPath + "/Resources/" + dataFolder;
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (File.Exists(loadDataPath +"/" + x + "," + y + ".json"))
                {
                    GameObject chunkManagerGO = Instantiate(chunkManagerPrefab);
                    chunkManagerGO.GetComponent<chunkManager>().jsonFileName = x + "," + y;
                    chunkManagerGO.GetComponent<chunkManager>().player = player;
                    chunkManagerGO.GetComponent<chunkManager>().loadDistance = loadDistance;
                    loadedZones.Add(chunkManagerGO);
                }

            }
        }
        string currentInstancePath = Application.dataPath + "/Resources/currentInstance";
        if (Directory.Exists(currentInstancePath)) { Directory.Delete(currentInstancePath, true); Directory.CreateDirectory(currentInstancePath); }


        DirectoryInfo dir = new DirectoryInfo(loadDataPath);
        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string tempPath = Path.Combine(currentInstancePath, file.Name);
            file.CopyTo(tempPath, false);
        }

        currentGameMode = GameMode.INGAME;
        InGameUI.SetActive(true);
        MenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        playerPositionX = (int)player.transform.position.x / 240;
        playerPositionY = (int)player.transform.position.z / 360;

        if (playerPosition.x != playerPositionX || playerPosition.y != playerPositionY)
        {
            playerPosition.x = playerPositionX;
            playerPosition.y = playerPositionY;
            foreach (var item in loadedZones)
            {
                string[] temp = item.GetComponent<chunkManager>().jsonFileName.Split(',');
                float floatx = System.Convert.ToSingle(temp[0]);
                float floaty = System.Convert.ToSingle(temp[1]);
                Vector2 Value = new Vector2(floatx, floaty);
                if ((playerPosition.x - Value.x < -1 || playerPosition.x - Value.x > 1) ||
                    (playerPosition.y - Value.y < -1 || playerPosition.y - Value.y > 1))
                {
                    loadedVectors.Remove(Value);
                    toBeDeleted.Add(item);
                }
                else
                {
                    if(!loadedVectors.Contains(Value))
                        loadedVectors.Add(Value);
                }
            }
            foreach (var item in toBeDeleted)
            {
                loadedZones.Remove(item);
                item.GetComponent<chunkManager>().DestroyArea();
            }
            toBeDeleted.Clear();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (!loadedVectors.Contains(new Vector2(playerPosition.x + x, playerPosition.y + y)) &&
                        File.Exists(Application.dataPath + "/Resources/currentInstance/" + (playerPosition.x + x) + "," + (playerPosition.y + y) + ".json"))
                    {
                        GameObject chunkManagerGO = Instantiate(chunkManagerPrefab);
                        chunkManagerGO.GetComponent<chunkManager>().jsonFileName = (playerPosition.x + x) + "," + (playerPosition.y + y);
                        chunkManagerGO.GetComponent<chunkManager>().player = player;
                        chunkManagerGO.GetComponent<chunkManager>().loadDistance = loadDistance;
                        loadedZones.Add(chunkManagerGO);
                    }
                }
            }
        }
    }
}
