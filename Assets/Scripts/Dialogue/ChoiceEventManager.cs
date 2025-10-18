using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChoiceEventManager : MonoBehaviour
{
    [System.Serializable]
    public class ScoreEvent
    {
        public int scoreToReach;
        public UnityEvent onScoreReached;
    }

    public List<ScoreEvent> scoreEvents;

    private int totalScore = 0;
    private HashSet<string> selectedChoices = new HashSet<string>();

    public void RegisterChoice(string choiceID, int points)
    {
        if (!selectedChoices.Contains(choiceID))
        {
            selectedChoices.Add(choiceID);
            totalScore += points;
            CheckScoreEvents();
        }
    }

    private void CheckScoreEvents()
    {
        foreach (var scoreEvent in scoreEvents)
        {
            if (totalScore == scoreEvent.scoreToReach)
            {
                scoreEvent.onScoreReached?.Invoke();
            }
        }
    }

    public void ResetChoices()
    {
        selectedChoices.Clear();
        totalScore = 0;
    }
}