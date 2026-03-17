using System.Collections.Generic;
using UnityEngine;

public class GrettingGenerator : MonoBehaviour
{
    public GreetingSO[] greetings;
    public string currentGreeting;

    void OnEnable()
    {
        LocationManager.onTurnOver += UpdateCurrentGreeting;
    }
    void OnDisable()
    {
        LocationManager.onTurnOver -= UpdateCurrentGreeting;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateGreeting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GenerateGreeting()
    {
        string greeting;

        // get current parameters of the target

        // based on the parameters, put possible greetings into list
        List<string> possibleGreetings = new List<string>();
        for (int i = 0; i < greetings.Length; i++)
        {
            if (CanSayGreeting(greetings[i]))
            {
                possibleGreetings.Add(greetings[i].greetingText);
            }
        }

        // select a random greeting from the list of possible greetings
        if (possibleGreetings.Count > 0)
        {
            greeting = possibleGreetings[Random.Range(0, possibleGreetings.Count)];
        }
        else
        {
            print("No possible greetings found for current target parameters.");
            greeting = "Hello."; // default greeting if no other greetings are possible
        }

        return greeting;
    }

    public string GetGreeting()
    {
        if (string.IsNullOrEmpty(currentGreeting))
        {
            currentGreeting = GenerateGreeting();
        }
        return currentGreeting;
        
    }

    public bool CanSayGreeting(GreetingSO greeting)
    {
        Target target = TargetManager.Instance.currentTarget;
        if (greeting.targetMood == target.TargetMood && greeting.minProgressMeter < target.progressValue && greeting.maxProgressMeter >= target.progressValue)
        {
            //print("CAN say greeting " + greeting.greetingText + " because the target mood requirement is " + greeting.targetMood + " and the current target mood is " + target.targetMood + " and the min progress meter requirement is " + greeting.minProgressMeter + " and the current progress meter is " + target.progressValue + " and the max progress meter requirement is " + greeting.maxProgressMeter + " and the current progress meter is " + target.progressValue);
            return true;
        }
        else
        {
            //print("CANNOT say greeting " + greeting.greetingText + " because the target mood requirement is " + greeting.targetMood + " and the current target mood is " + target.targetMood + " or the min progress meter requirement is " + greeting.minProgressMeter + " and the current progress meter is " + target.progressValue + " or the max progress meter requirement is " + greeting.maxProgressMeter + " and the current progress meter is " + target.progressValue);
        }
        return false;
    }
    
    public void UpdateCurrentGreeting()
    {
        print("Updating current greeting...");
        currentGreeting = GenerateGreeting();
    }
}
