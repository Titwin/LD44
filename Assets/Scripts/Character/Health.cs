﻿using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] ParticleSystem blood;
    [SerializeField] float invulnerabilityTime = 0.5f;
    [SerializeField] int maxValue;

    public int secondsToDecrement;

    protected float lastDecrementDuration;

    private int value;
    private float unhurtTime = 0;

    public int Max { get { return maxValue; } }

    public int Value
    {
        get { return value; }
        set {
            if (value < this.value)
            {
                unhurtTime = 0;
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