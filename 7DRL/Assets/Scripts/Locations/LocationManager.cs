using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public static LocationManager Instance;
    // These are arrays with each index being for the stage
    public LocationSO[] LocationOneInfo;
    public LocationSO[] LocationTwoInfo;
    public LocationSO[] LocationThreeInfo;
    public LocationSO[] LocationFourInfo;
    public Location[] CurrentLocations;
    public TrainingEventSO[] TrainingEvents;
    public Player player;
    public int currentStageIndex = 1;
    public GameObject EventIconPrefab;
    public GameObject currentEventIcon;
    public TrainingEventSO currentEvent;
    public Transform CanvasParent;
    // Gameobjects for the HUD elements so they can be turned on and off when needed.
    public GameObject[] StatObjects;
    public GameObject TimeObject;
    public GameObject MoodObject;
    public GameObject MoodChangeObject;
    public GameObject CharacterIconObject;

    public DialougeSO[] StageOpeningDialogue;
    public DialougeSO[] ConfessionDialogue;
    public bool isTraining = false;
    public enum LocationIndex
    {
        LocationOne,
        LocationTwo,
        LocationThree,
        LocationFour
    }

    public static event Action onTrainingEnded;
    public static event Action onTurnOver;
     void Awake()
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
    }

    void Start()
    {
        DisableTrainingHUD();
        StartCoroutine(StartStageOneDialogue());
        CurrentLocations[0].UpdateLocationInfo(LocationOneInfo[currentStageIndex]);
        CurrentLocations[1].UpdateLocationInfo(LocationTwoInfo[currentStageIndex]);
        CurrentLocations[2].UpdateLocationInfo(LocationThreeInfo[currentStageIndex]);
        CurrentLocations[3].UpdateLocationInfo(LocationFourInfo[currentStageIndex]); // :)
    }

    void OnEnable()
    {
        LocationButton.onTrainingSelected += StartTraining;
        GameTime.onStageEnd += StartConfessionScenario;
    }

    void OnDisable()
    {
        LocationButton.onTrainingSelected -= StartTraining;
        GameTime.onStageEnd -= StartConfessionScenario;
    }

    public IEnumerator StartStageOneDialogue()
    {
        DialogueTextManager.Instance.currentDialouge = StageOpeningDialogue[0];
        DialogueTextManager.Instance.StartDialouge();
        yield return new WaitUntil(() => DialogueTextManager.Instance.IsInDialouge == false);
        EnableTrainingHUD();
    }

    public void StartConfessionScenario()
    {
        StartCoroutine(ConfessionScenario());
    }

    public IEnumerator ConfessionScenario()
    {
        yield return new WaitUntil(() => DialogueTextManager.Instance.IsInDialouge == false);
        DisableTrainingHUD();
        DialogueTextManager.Instance.currentDialouge = ConfessionDialogue[currentStageIndex];
        DialogueTextManager.Instance.StartDialouge();
        yield return new WaitUntil(() => DialogueTextManager.Instance.IsInDialouge == false);
        
        if (!TargetManager.Instance.currentTarget.gameOver)
        {
            UpdateNextStageLocation();
            EnableTrainingHUD();
        }
        else
        {
            // probably just throw player into conffesion scenario
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndScene");
        }
    }
    
    private void StartTraining(Location location)
    {
        // This function will be called when the player clicks on a location button, it will start the training for that location.
        // You can add any additional functionality you want here, such as loading a new scene or displaying a new UI panel.

        if (location.currentTrainingEvent != null)
        {
            Debug.Log("Current training event: " + location.currentTrainingEvent.EventName);
            player.ChangeStat(location.currentTrainingEvent.statAffected, location.currentTrainingEvent.statChangeAmount);
            isTraining = true;
            // start dialogue 
            StartTrainingDialogue(location.currentTrainingEvent);
            return;
        }
        else
        {
            BeginBasicTraining(location);
            return;
        }
        // update stats based on the location's base stat increase and the player's current stats
        
    }
    public void endTraining()
    {
        StartCoroutine(TrainEnd());
    }
    private IEnumerator TrainEnd()
    {
        EnableTrainingHUD();
        Destroy(currentEventIcon);
        onTrainingEnded?.Invoke();
        yield return new WaitUntil(() => TargetManager.Instance.moodEventCheckFailed == true);
        print("Turn is over, invoking onTurnOver event...");
        TargetManager.Instance.moodEventCheckFailed = false;
        onTurnOver?.Invoke();
    }

    public void EndTraining()
    {
        CurrentLocations[(int)currentEvent.locationRequirement].currentTrainingEvent = null;
        CurrentLocations[(int)currentEvent.locationRequirement].LocationEventIndex++;
        currentEvent = null;
        endTraining();
    }

    public IEnumerator TrainingDialogue(TrainingEventSO trainingEvent)
    {
        // this function will be called to start the training dialogue for the current event, it will check if there is an event for the current location and if there is it will start the dialogue for that event.
        if (trainingEvent != null)
        {
            DialogueTextManager.Instance.currentDialouge = trainingEvent.dialouge;
            DialogueTextManager.Instance.StartDialouge();
        }
        DisableTrainingHUD();
        yield return new WaitUntil(() => DialogueTextManager.Instance.IsInDialouge == false);
        print("training dialogue is over");
        EndTraining();
    }

    public void StartTrainingDialogue(TrainingEventSO trainingEvent)
    {
        StartCoroutine(TrainingDialogue(trainingEvent));
    }

    public void GiveLocationEvent(TrainingEventSO trainingEvent)
    {
        // this function will be called by the TrainingEventManager when a training event is triggered, it will give the player the event for the current location.
        currentEventIcon = Instantiate(EventIconPrefab);
        currentEventIcon.transform.SetParent(GameObject.Find($"Location{(int)trainingEvent.locationRequirement + 1}").transform, false);

        currentEventIcon.transform.localRotation = Quaternion.identity;
        RectTransform rectTransform = currentEventIcon.GetComponent<RectTransform>();

        rectTransform.anchoredPosition = CurrentLocations[(int)trainingEvent.locationRequirement].GetComponentInChildren<RectTransform>().anchoredPosition + rectTransform.rect.height * Vector2.up;
        
        CurrentLocations[(int)trainingEvent.locationRequirement].currentTrainingEvent = trainingEvent;
        currentEvent = trainingEvent;
        return;
    }

    #region HUD Management
    public void DisableTrainingHUD()
    {
        DisableTrainingButtons();
        DisableTrainingStats();
        DisableTrainingYear();
        DisableTrainingMood();
        //DisableCharacterIcon();
        //DisableMoodChange();
    }

    public void DisableCharacterIcon()
    {
        if (CharacterIconObject != null)
        {
            CharacterIconObject.SetActive(false);
        }
    }

    public void DisableMoodChange()
    {
        print("Disabling Mood Change");
        // needs to be enabled for it to function so i will jsut throw it off the map
        if (MoodChangeObject != null)
        {
            MoodChangeObject.transform.position = new Vector2(MoodChangeObject.transform.position.x - 1000, MoodChangeObject.transform.position.y - 1000);
        }
    }

    public void DisableTrainingMood()
    {
        if (MoodObject != null)
        {
            MoodObject.SetActive(false);
        }
    }

    public void DisableTrainingYear()
    {
        if (TimeObject != null)
        {
            TimeObject.SetActive(false);
        }
    }

    public void DisableTrainingStats()
    {
        foreach (GameObject statObject in StatObjects)
        {
            statObject.SetActive(false);
        }
    }

    public void DisableTrainingButtons()
    {
        foreach (Location location in CurrentLocations)
        {
            location.gameObject.SetActive(false);
        }
    }
    
    public void EnableTrainingHUD()
    {
        EnableTrainingButtons();
        EnableTrainingStats();
        EnableTrainingYear();
        EnableTrainingMood();
        //EnableCharacterIcon();
        //EnableMoodChange();
    }

    public void EnableCharacterIcon()
    {
        if (CharacterIconObject != null)
        {
            CharacterIconObject.SetActive(true);
        }   
    }
    
    public void EnableMoodChange()
    {
        if (MoodChangeObject != null)
        {
            MoodChangeObject.transform.position = MoodChangeObject.GetComponent<MoodChangeButton>().OriginalPosition;
        }
    }

    public void EnableTrainingMood()
    {
        if (MoodObject != null)
        {
            MoodObject.SetActive(true);
        }
    }

    public void EnableTrainingButtons()
    {
        foreach (Location location in CurrentLocations)
        {
            location.gameObject.SetActive(true);
        }
    }

    public void EnableTrainingStats()
    {
        foreach (GameObject statObject in StatObjects)
        {
            statObject.SetActive(true);
        }
    }

    public void EnableTrainingYear()
    {
        TimeObject.SetActive(true);
    }
    #endregion

    public void UpdateNextStageLocation()
    {
        currentStageIndex++;

        
        // get locations in scene and then update them to have the info for the next stage.
        // can jsut make this location info a 2D array but im lazy so we wil do it later if needed
        CurrentLocations[0].UpdateLocationInfo(LocationOneInfo[currentStageIndex]);
        CurrentLocations[1].UpdateLocationInfo(LocationTwoInfo[currentStageIndex]);
        CurrentLocations[2].UpdateLocationInfo(LocationThreeInfo[currentStageIndex]);
        CurrentLocations[3].UpdateLocationInfo(LocationFourInfo[currentStageIndex]);

        return;
    }

    public  IEnumerator StartBasicTraining(Location location)
    {
        DisableTrainingHUD();
        // figure out which location is being trained
        string text = $"Trained {location.baseStatIncrease} {Enum.GetName(typeof(Player.StatIndex), location.statIndex)}";
        // start dialogue which tells the player what stat they trained 
        DialougeSO dialouge = DialogueTextManager.Instance.GenerateDialogue("", text, "", null, DialougeTypes.SingleChoice, true, null, false, false, false);
        DialogueTextManager.Instance.StartDialouge(dialouge);

        yield return new WaitUntil(() => DialogueTextManager.Instance.IsInDialouge == false);
        //update stats on screen 
        player.Stats[location.statIndex] += location.baseStatIncrease;
        
        player.UpdateStatText();
        endTraining();
    }

    public void BeginBasicTraining(Location location)
    {
        StartCoroutine(StartBasicTraining(location));
    }
}
