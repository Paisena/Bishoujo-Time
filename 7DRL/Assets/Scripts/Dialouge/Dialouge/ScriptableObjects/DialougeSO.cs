using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialougeSO", menuName = "Scriptable Objects/DialougeSO")]
public class DialougeSO : ScriptableObject
{
    [field: SerializeField] public string DialougeName { get; set; }
    [field: SerializeField] public string CharacterName { get; set; }
    [field: SerializeField] public Sprite CharacterIcon { get; set; }
    [field: SerializeField] public string Text { get; set; }
    [field: SerializeField] public List<DialougeChoiceData> Choices { get; set; }
    [field: SerializeField] public DialougeTypes DialougeTypes { get; set; }
    [field: SerializeField] public bool IsStartingDialouge { get; set; }
    [field: SerializeField] public List<Reward> reward;
    [field: SerializeField] public bool PickRewardRandom { get; set; }
    [field: SerializeField] public bool PickWhichReward { get; set; }
    [field: SerializeField] public bool PickRewardBasedOnChoice { get; set; }

    public void Initialize(string dialougeName, string text, string characterName, Sprite characterIcon, List<DialougeChoiceData> choices, DialougeTypes dialougeTypes, List<Reward> reward, bool isStartingDialouge,
     bool pickRewardRandom, bool pickWhichReward, bool pickRewardBasedOnChoice)
    {
        DialougeName = dialougeName;
        CharacterName = characterName;
        CharacterIcon = characterIcon;
        Text = text;
        Choices = choices;
        DialougeTypes = dialougeTypes;
        IsStartingDialouge = isStartingDialouge;
        this.reward = reward;
        PickRewardRandom = pickRewardRandom;
        PickWhichReward = pickWhichReward;
        PickRewardBasedOnChoice = pickRewardBasedOnChoice;
    }
}
