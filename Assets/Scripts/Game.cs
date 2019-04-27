using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public Health Health { get; private set; }

    public CharacterController2D Character { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            throw new Exception("More than two Game instance.");
        }

        Instance = this;
    }

    protected virtual void Start()
    {
        Health = FindObjectOfType<Health>();
        Character = FindObjectOfType<CharacterController2D>();
    }

    protected virtual void LateUpdate()
    {
        if (Health.Value <= 0)
        {
            Restart();
        }
    }

    protected virtual void Restart()
    {
        Health.Restart();
    }
}
