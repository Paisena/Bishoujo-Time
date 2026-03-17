using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using UnityEngine;

[Serializable]
public class DialougeNodeSavaData 
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public string CharacterName { get; set; }
    [field: SerializeField] public Sprite CharacterIcon { get; set; }
    [field: SerializeField] public List<Reward> Reward { get; set; }
    [field: SerializeField] public bool PickRandomReward { get; set; }
    [field: SerializeField] public bool PickWhichReward { get; set; }
    [field: SerializeField] public string Text { get; set; }
    [field: SerializeField] public string ID { get; set; }
    [field: SerializeField] public List<DialougeChoiceSavaData> Choices { get; set; }
    [field: SerializeField] public DialougeTypes DialougeType { get; set; }
    [field: SerializeField] public Vector2 Position { get; set; }
    [field: SerializeField] public string Requirements { get; set; }
    [field: SerializeField] public bool PickRewardBasedOnChoice { get; set; }
}
