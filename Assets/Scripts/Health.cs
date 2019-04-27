using UnityEngine;

public class Health : MonoBehaviour
{
    public float startValue;

    public float decreasePerSecond;

    public float StartValue { get { return startValue; } }

    public float Value { get; set; }

    protected virtual void Awake()
    {
        Restart();
    }

    void Update()
    {
        Value -= decreasePerSecond * Time.deltaTime;
    }

    public void Restart()
    {
        Value = startValue;
    }
}
