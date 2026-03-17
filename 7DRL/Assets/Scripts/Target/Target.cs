using System;
using TMPro;
using UnityEngine;

public class Target : MonoBehaviour
{
    public event Action<float> onProgressValueChanged;
    public string TargetName;
    public string TargetDescription;
    public TextMeshProUGUI targetMoodText;
    public enum Mood
    {
        Happy,
        Sad,
        Angry
        
    }    
    private Mood targetMood;
    public Mood TargetMood
    {
        get => targetMood;
        set
        {
            targetMood = value;
            if (targetMoodText != null)
            {
                targetMoodText.text = targetMood.ToString();
            }
        }
    }
    [SerializeField] private float _progressValue;
    public float progressValue
    {
        get => _progressValue;
        set
        {
            float oldValue = _progressValue;
            _progressValue = value;

            float delta = progressValue - oldValue;

            onProgressValueChanged?.Invoke(delta);

        }
    }
    public bool gameOver = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
