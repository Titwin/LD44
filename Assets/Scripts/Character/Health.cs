﻿using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] ParticleSystem blood;
    [SerializeField] public float invulnerabilityTime = 0.5f;
    [SerializeField] int maxValue;

    public int secondsToDecrement;

    protected float lastDecrementDuration;

    private int value;
    private float unhurtTime = 0;

    public int Max { get { return maxValue; } }

    public int Value
    {
        get { return value; }
        private set {
            this.value = Mathf.Clamp(value, 0, maxValue);
        }
    }

    public void Hurt(GameObject source, int amount)
    {
        Value -= amount;
        unhurtTime = 0;
        
        if (blood != null)
        {
            ParticleSystem.EmitParams p = new ParticleSystem.EmitParams();
            Vector3 sourcePosition = source != null ? source.transform.position : this.transform.position - new Vector3(0, 6f, 0);
            p.position = this.transform.position;
            for (int i = 0; i < 10* amount; ++i)
            {
                p.velocity = (this.transform.position - sourcePosition).normalized * Random.Range(1f, 2f)*(source==null?2:1) + new Vector3(Random.RandomRange(-0.5f,0.5f), Random.RandomRange(-0.5f, 0.5f),0);
                blood.Emit(p, (source == null ? 5: 1));
            }
        }
        else
        {
            Debug.LogWarning(this.gameObject.name + " does not have a blood emitter");
        }

    }
    public void Suck(GameObject source, int amount)
    {
        Value -= amount;
        unhurtTime = 0;
        if (blood != null)
        {
            ParticleSystem.EmitParams p = new ParticleSystem.EmitParams();
            p.position = this.transform.position;
            for (int i = 0; i < 50* amount; ++i)
            {
                p.velocity = -(this.transform.position - source.transform.position).normalized * Random.Range(1f,7f) + new Vector3(Random.RandomRange(-0.5f, 0.5f), Random.RandomRange(-0.5f, 0.5f), 0);
                blood.Emit(p, 1);
            }
        }
        else
        {
            Debug.LogWarning(this.gameObject.name + " does not have a blood emitter");
        }

    }
    public void Heal(GameObject source, int amount)
    {
        Value += amount;
        if (blood != null)
        {
            ParticleSystem.EmitParams p = new ParticleSystem.EmitParams();
            p.position = source.transform.position;
            for (int i = 0; i < 5* amount; ++i)
            {
                p.velocity = (this.transform.position - source.transform.position).normalized * Random.Range(1f, 5f) + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                blood.Emit(p, 1);
            }
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
                this.Hurt(null,1);
                lastDecrementDuration = 0;
            }
        }
    }
    private void LateUpdate()
    {
        unhurtTime+= Time.deltaTime;
    }

    public float Ratio
    {
        get
        {
            return ((float)this.value) / maxValue;
        }
    }
    public bool Invulnerable
    {
        get
        {
            return unhurtTime < invulnerabilityTime;
        }
    }

    private void OnDrawGizmos()
    {
        float r = Ratio;
        Gizmos.color = Color.gray;
        Vector2 size = new Vector2(1, 2);
        Gizmos.DrawCube(this.transform.position - new Vector3(0, size.y/2, 0), new Vector3(size.x, 0.1f, 0.1f));
        Gizmos.color = this.Invulnerable? Color.white: this.gameObject.layer == 10/*"Player"*/? Color.red:Color.magenta;
        Gizmos.DrawCube(this.transform.position - new Vector3(size.x/2 - (r) / 2, size.y/2, 0), new Vector3(r, 0.1f, 0.1f));
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(this.transform.position - new Vector3(0, size.y/2, 0), new Vector3(size.x, 0.1f, 0.1f));
    }

}
