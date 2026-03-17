using UnityEngine;

[CreateAssetMenu(fileName = "MoodReward", menuName = "Scriptable Objects/MoodReward")]
public class MoodReward : Reward
{
    public Target.Mood moodToChangeTo;
    public override void GiveReward(Player player)
    {
        TargetManager.Instance.currentTarget.TargetMood = moodToChangeTo;
    }
}
