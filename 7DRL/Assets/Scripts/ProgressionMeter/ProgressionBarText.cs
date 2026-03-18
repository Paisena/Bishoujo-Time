using System.Collections;
using TMPro;
using UnityEngine;

public class ProgressionBarText : MonoBehaviour
{
    public TextMeshProUGUI progressionText;
    public Target target;
    public float textUpdateDelay = 0.5f;

    void OnEnable()
    {
        target.onProgressValueChanged += StartUpdate;     
    }

    void OnDisable()
    {
        target.onProgressValueChanged -= StartUpdate;     
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartUpdate(float newValue)
    {
        StartCoroutine(UpdateProgressTextWithDelay(newValue));
    }

    public void UpdateProgressText(float newValue)
    {
        progressionText.text = $"Progress: {Mathf.RoundToInt(newValue * 100)}%";
    }
    public IEnumerator UpdateProgressTextWithDelay(float newValue)
    {   
        UpdateProgressText(newValue);
        yield return new WaitForSeconds(textUpdateDelay);
        CheckIfGameOver();
    }

    public void CheckIfGameOver()
    {
        if (target.progressValue >= 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndScene");
        }
    }
}
