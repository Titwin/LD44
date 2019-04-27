using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Player : Character
{
    public CharacterController2D Controller { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Controller = GetComponent<CharacterController2D>();
    }
}
