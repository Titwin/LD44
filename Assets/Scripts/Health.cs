using UnityEngine;

public class Health : MonoBehaviour
{
    public float startValue;

    public float decreasePerSecond;

    public float StartValue { get { return startValue; } }

    public float Value { get; set; }

    protected virtual void Awake()
    {
        Value = startValue;
    }

    void Update()
    {
        Value -= decreasePerSecond * Time.deltaTime;
    }
}
