using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxValue;

    public int secondsToDecrement;

    public int Max { get { return maxValue; } }

    public int Value { get; set; }

    protected float lastDecrementDuration;

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
