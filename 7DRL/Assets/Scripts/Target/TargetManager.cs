using System.Collections;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }
    public Target currentTarget;
    public MoodEvent[] moodEvents;
    public int MoodEventChance = 30; 
    public MoodEventText moodEventText;
    public bool moodEventCheckFailed = false;
    public static event System.Action onMoodEventOver;
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
        moodEventText.SetMoodEventText(currentTarget.TargetMood.ToString());
    }

    void OnEnable()
    {
        LocationManager.onTrainingEnded += MoodEventCheck;
    }

    void OnDisable()
    {
        LocationManager.onTrainingEnded -= MoodEventCheck;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void MoodEventCheck()
    {
        StartCoroutine(DecideTargetMood());
    }

    public IEnumerator DecideTargetMood()
    {
        // Roll to see if a mood event happens
        int roll = Random.Range(0, 100);
        if (roll < MoodEventChance) // 30% chance for a mood event to occur
        {
            print("mood event triggered");
            int moodEventIndex = Random.Range(0, moodEvents.Length);
            currentTarget.TargetMood = moodEvents[moodEventIndex].MoodChangeTo;

            // Trigger the dialogue for the mood event
            DialogueTextManager.Instance.StartDialouge(moodEvents[moodEventIndex].dialouge);
            moodEventText.SetMoodEventText(currentTarget.TargetMood.ToString());
            yield return new WaitUntil(() => DialogueTextManager.Instance.IsInDialouge == false);
            LocationManager.Instance.EnableTrainingHUD();
        }
        print("mood event check over, setting moodEventCheckFailed to false and invoking onMoodEventOver event...");
        moodEventCheckFailed = true;
        onMoodEventOver?.Invoke();
    }

    public void ChangeProgressMeter(float amount)
    {
        currentTarget.progressValue += amount;
    }
}
