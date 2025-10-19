using UnityEngine;
using UnityEngine.Events;

public class PressurePuzzle : MonoBehaviour
{
    public PressureButton[] buttons;
    public UnityEvent onPuzzleSolved;
    public UnityEvent onPuzzleUnsolved;

    private bool _solved = false;
    public bool keepButtonsPressed = false;

    void Update()
    {
        if (!_solved && AreAllButtonsPressed())
        {
            _solved = true;
            Debug.Log("[Puzzle2D] Puzzle resolvido!");
            onPuzzleSolved.Invoke();
        }
        else if (_solved && !AreAllButtonsPressed() && !keepButtonsPressed)
        {
            _solved = false;
            Debug.Log("[Puzzle2D] Puzzle desfeito.");
            onPuzzleUnsolved.Invoke();
        }
    }

    private bool AreAllButtonsPressed()
    {
        foreach (var btn in buttons)
        {
            if (!btn.IsPressed)
                return false;
        }
        return true;
    }
}
