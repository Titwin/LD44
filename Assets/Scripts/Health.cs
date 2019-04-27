using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxValue;

    public int secondsToDecrement;

    protected float lastDecrementDuration;
    private int value;

    public int Max { get { return maxValue; } }

    public int Value
    {
        get { return value; }
        set { this.value = Mathf.Clamp(value, 0, maxValue); }
    }

    protected virtual void Awake()
    {
        Value = maxValue;
    }

    void Update()
    {
        lastDecrementDuration += Time.deltaTime;
        if (lastDecrementDuration >= secondsToDecrement)
        {
            Value--;
            lastDecrementDuration = 0;
        }
    }
}
