using System.Collections.Generic;
using UnityEngine;

public class TrainingEventManager : MonoBehaviour
{
    TrainingEventSO[] trainingEventsStageOne;
    TrainingEventSO[] trainingEventsStageTwo;
    TrainingEventSO[] trainingEventsStageThree;
    public int eventChance = 75;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnEnable()
    {
        TargetManager.onMoodEventOver += GenerateEventLocation;   
    }

    void OnDisable()
    {
        TargetManager.onMoodEventOver -= GenerateEventLocation;
    }

    public void GenerateEventLocation()
    {
        foreach (Location location in LocationManager.Instance.CurrentLocations)
        {
            location.currentTrainingEvent = null;
        }
        // roll to see if an event will occur
        int eventRoll = Random.Range(0, 100);
        if (eventRoll >= eventChance) 
        {
            //print($"event roll is {eventRoll} and event chance is {eventChance} so no event will occur");
            return;
        }
        // check what events are possible based on the progression of events given each location and the current target mood
        print("Generating event location");

        List<TrainingEventSO> possibleEvents = new List<TrainingEventSO>();

        Location[] locations = LocationManager.Instance.CurrentLocations;
        foreach (TrainingEventSO trainingEvent in LocationManager.Instance.TrainingEvents)
        {
                if(trainingEvent.eventStage == locations[(int)trainingEvent.locationRequirement].LocationEventIndex
                 && trainingEvent.targetMoodRequirement == TargetManager.Instance.currentTarget.TargetMood 
                 && trainingEvent.progressMinRequirement <= TargetManager.Instance.currentTarget.progressValue
                 && trainingEvent.progressMaxRequirement >= TargetManager.Instance.currentTarget.progressValue)
                {
                    //print("Event " + trainingEvent.EventName + " is possible");
                    possibleEvents.Add(trainingEvent);
                }
                else
                {
                    //print("Event " + trainingEvent.EventName + " is not possible because the event stage is " + trainingEvent.eventStage + " and the location event index is " + locations[(int)trainingEvent.locationRequirement].LocationEventIndex + " or the target mood requirement is " + trainingEvent.targetMoodRequirement + " and the current target mood is " + TargetManager.Instance.currentTarget.targetMood);
                }
        }

        // randomly roll for between the possible events.
        if(possibleEvents.Count == 0)
        {
            return;
        }
        int whichEvent = Random.Range(0, possibleEvents.Count);
        TrainingEventSO selectedEvent = possibleEvents[whichEvent];

        LocationManager.Instance.GiveLocationEvent(selectedEvent);
        return;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
