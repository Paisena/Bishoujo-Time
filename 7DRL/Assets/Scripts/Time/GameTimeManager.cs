using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance { get; private set; }
    public GameTime gameTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        gameTime = GetComponent<GameTime>();
        UpdateTimeText(gameTime.TranslateToTimeUnit());
    }

    void OnEnable()
    {
        //LocationButton.onTrainingSelected += ProgressTime;
        LocationManager.onTrainingEnded += ProgressTime;
    }

    void OnDisable()
    {
        //LocationButton.onTrainingSelected -= ProgressTime;
        LocationManager.onTrainingEnded -= ProgressTime;
    }

 
    // Update is called once per frame
    void Update()
    {
        
    }
    private void ProgressTime()
    {
        gameTime.CurrentTurn++; // just update it for now, will probably have to add more stuff later
        string date = gameTime.TranslateToTimeUnit();
        UpdateTimeText(date);
    }

    public void StartTime()
    {
        gameTime.CurrentTurn = 0;
    }

    public void UpdateTimeText(string timeText)
    {
        GameObject timeTextObject = GameObject.Find("TimeText");
        if (timeTextObject != null)
        {
            timeTextObject.GetComponent<TextMeshProUGUI>().text = timeText;
        }
    }
}
