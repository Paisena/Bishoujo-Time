using UnityEngine;

[CreateAssetMenu(fileName = "TrainingEventSO", menuName = "Scriptable Objects/TrainingEventSO")]
public class TrainingEventSO : ScriptableObject
{
    public string EventName;
    public DialougeSO dialouge;
    public int statAffected;
    public float statChangeAmount;
    public int eventStage;
    public Target.Mood targetMoodRequirement;
    public LocationManager.LocationIndex locationRequirement;
    public float eventWeight;
    public float progressMinRequirement;
    public float progressMaxRequirement;
}
