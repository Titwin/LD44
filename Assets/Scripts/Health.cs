using UnityEngine;

public class Health : MonoBehaviour
{
    public int startValue;

    public int secondsToDecrement;

    public int StartValue { get { return startValue; } }

    public int Value { get; set; }

    protected float lastDecrementDuration;

    protected virtual void Awake()
    {
        Value = startValue;
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
