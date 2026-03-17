using UnityEngine;

[CreateAssetMenu(fileName = "ProgressReward", menuName = "Scriptable Objects/ProgressReward")]
public class ProgressReward : Reward
{
    [SerializeField] private float progressIncreaseAmount;
    public override void GiveReward(Player player)
    {
        TargetManager.Instance.ChangeProgressMeter(progressIncreaseAmount);
        Debug.Log("Progress Meter Increased by: " + progressIncreaseAmount);
    }
}
