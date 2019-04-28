using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] ParticleSystem blood;
    public int maxValue;

    public int secondsToDecrement;

    protected float lastDecrementDuration;

    private int value;
    private int hurt = 0;

    public int Max { get { return maxValue; } }

    public int Value
    {
        get { return value; }
        set {
            if (value < this.value)
            {
                hurt = 0;
                if (blood != null)
                {
                    blood.Emit(10);
                }
                else
                {
                    Debug.LogWarning(this.gameObject.name + " does not have a blood emitter");
                }
            }
            this.value = Mathf.Clamp(value, 0, maxValue);
        }
    }

    protected virtual void Awake()
    {
        Value = maxValue;
    }

    void Update()
    {
        if (secondsToDecrement > 0)
        {
            lastDecrementDuration += Time.deltaTime;
            if (lastDecrementDuration >= secondsToDecrement)
            {
                Value--;
                lastDecrementDuration = 0;
            }
        }
    }
    private void LateUpdate()
    {
        ++hurt;
    }

    public float Ratio
    {
        get
        {
            return ((float)this.value) / maxValue;
        }
    }

    private void OnDrawGizmos()
    {
        float r = Ratio;
        Gizmos.color = Color.gray;
        Vector2 size = new Vector2(1, 2);
        Gizmos.DrawCube(this.transform.position - new Vector3(0, size.y/2, 0), new Vector3(size.x, 0.1f, 0.1f));
        Gizmos.color = hurt<10? Color.white: this.gameObject.layer == 10/*"Player"*/? Color.red:Color.magenta;
        Gizmos.DrawCube(this.transform.position - new Vector3(size.x/2 - (r) / 2, size.y/2, 0), new Vector3(r, 0.1f, 0.1f));
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(this.transform.position - new Vector3(0, size.y/2, 0), new Vector3(size.x, 0.1f, 0.1f));
    }

}
