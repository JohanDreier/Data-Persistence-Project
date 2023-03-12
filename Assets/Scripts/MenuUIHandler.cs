using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] Text playerScore;
    [SerializeField] InputField playerName;

    public string name = "";
    public string bestPlayer;
    public int bestScore;

    public static MenuUIHandler Instance;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        LoadGameData();
    }

    public void Start()
    {
        if(bestPlayer != "")
        {
            playerScore.text = "Best Score: " + bestPlayer + " : " + bestScore;
        }
    }

    public void Update()
    {
    }

    public void AddBestScore(int score)
    {
        if(score > bestScore)
        {
            bestScore = score;
            bestPlayer = name;
            SaveGameData();
            MainManager.Instance.BestScoreText.text = "Best Score: " + bestPlayer + " : " + bestScore;
        }
    }    

    public void StartNew()
    {
        if (playerName.text != "")
        {
            name = playerName.text;
            SceneManager.LoadScene("main");
        }
        else
            Debug.LogWarning("Please enter a name!");
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit;
        #endif
    }

    [System.Serializable]
    class SaveData
    {
        public string name;
        public int score;
    }

    public void SaveGameData()
    {
        SaveData data = new SaveData();
        data.name = bestPlayer;
        data.score = bestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadGameData()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.name;
            bestScore = data.score;
        }
    }
}