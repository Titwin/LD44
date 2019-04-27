using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] ParticleSystem blood;
    public int maxValue;

    public int secondsToDecrement;

    protected float lastDecrementDuration;
    private int value;

    public int Max { get { return maxValue; } }

    public int Value
    {
        get { return value; }
        set {
            if (value < this.value)
            {
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
        Gizmos.DrawCube(this.transform.position - new Vector3(0, 0.5f, 0), new Vector3(1, 0.1f, 0.1f));
        Gizmos.color = this.gameObject.layer == 10/*"Player"*/? Color.red:Color.magenta;
        Gizmos.DrawCube(this.transform.position - new Vector3(-0.5f + (r) / 2, 0.5f, 0), new Vector3(r, 0.1f, 0.1f));
        //Gizmos.DrawCube(this.transform.position + new Vector3(-0.5f +(1- r) / 2, 1, 0), new Vector3(1 - r, 0.1f, 0.1f));
        //Gizmos.DrawCube(this.transform.position + new Vector3(-Ratio / 2, 1, 0), new Vector3(Ratio, 0.1f, 0.1f));
        //Gizmos.color = Color.red;
        //Gizmos.DrawCube(this.transform.position + new Vector3(-0.5f + r / 2, 1, 0), new Vector3(r, 0.1f, 0.1f));
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(this.transform.position - new Vector3(0, 0.5f, 0), new Vector3(1, 0.1f, 0.1f));
    }

}
