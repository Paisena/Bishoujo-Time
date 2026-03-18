using System;
using TMPro;
using Unity.VectorGraphics;
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
    [SerializeField] private Mood targetMood;
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
            print($"Progress value changed by {delta}, new value: {progressValue}");

            onProgressValueChanged?.Invoke(progressValue);

            if (progressValue >= 1)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("WinScene");
            }

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
