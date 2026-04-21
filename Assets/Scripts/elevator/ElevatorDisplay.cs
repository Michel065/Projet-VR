using UnityEngine;
using TMPro;

public class ElevatorDisplay : MonoBehaviour
{
    public ElevatorController elevator;
    public TextMeshPro textDisplay;

    void Update()
    {
        if (elevator == null || textDisplay == null)
            return;

        if (elevator.state == ElevatorController.ElevatorState.MovingUp)
        {
            textDisplay.text = "UP";
        }
        else if (elevator.state == ElevatorController.ElevatorState.MovingDown)
        {
            textDisplay.text = "DOWN";
        }
        else
        {
            textDisplay.text = elevator.currentFloor.ToString();
        }
    }
}