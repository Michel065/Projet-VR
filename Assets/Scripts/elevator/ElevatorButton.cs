using UnityEngine;
using TMPro;

public class ElevatorButton : MonoBehaviour
{
    [Header("References")]
    public ElevatorController elevator;
    public Renderer buttonRenderer;

    [Header("Config")]
    public int targetFloor;

    [Header("Text")]
    public TextMeshPro text;

    [Header("Colors")]
    public Color idleColor = Color.white;
    public Color activeColor = Color.green;
    public Color blockedColor = Color.red;

    [Header("State")]
    public bool isCalled = false;
    bool lastState = false;

    void Start()
    {
        if (text != null)
        {
            text.text = targetFloor.ToString();
        }
        UpdateVisual();
    }

    void Update()
    {
        UpdateVisual();

        if (isCalled && !lastState)
        {
            CallElevator();
        }

        lastState = isCalled;

        if (isCalled && elevator != null)
        {
            if (!elevator.IsMoving && elevator.currentFloor == targetFloor)
            {
                isCalled = false;
            }
        }
    }

    public void CallElevator()
    {
        if (elevator == null)
            return;

        if (GameManager.Instance == null)
            return;

        if (!GameManager.Instance.IsFloorAccessible(targetFloor))
            return;

        elevator.GoToFloor(targetFloor);
        UpdateVisual();
        isCalled = true;
    }

    void UpdateVisual()
    {
        if (buttonRenderer == null)
            return;

        if (GameManager.Instance == null)
        {
            buttonRenderer.material.color = idleColor;
            return;
        }

        if (!GameManager.Instance.IsFloorAccessible(targetFloor))
        {
            buttonRenderer.material.color = blockedColor;
            return;
        }

        if (elevator != null && elevator.targetFloor == targetFloor && elevator.IsMoving)
        {
            buttonRenderer.material.color = activeColor;
            return;
        }

        buttonRenderer.material.color = idleColor;
        UpdateTextColor();
    }

    void UpdateTextColor()
    {
        if (text == null)
            return;
        text.color = Color.black;
        text.outlineColor = Color.white;
        text.outlineWidth = 0.2f;
    }

    void OnMouseDown()
    {
        CallElevator();
    }
}