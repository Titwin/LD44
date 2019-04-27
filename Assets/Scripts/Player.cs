using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Player : Character
{
    public CharacterController2D Controller { get; private set; }
    
    // comodity property for AI, can be replaced with line-of-sight player detection
    public static Player thePlayer;

    protected override void Awake()
    {
        base.Awake();
        Controller = GetComponent<CharacterController2D>();
        thePlayer = this;
    }

    private void Update()
    {
        // set the movement actions
        float movementX = Input.GetAxis("Horizontal");
        bool jump = Input.GetButtonDown("Jump");
        bool duck = Input.GetButton("Duck");
        bool attack = Input.GetButtonDown("Fire1");
        Controller.Move(movementX, jump, attack, duck);
    }

    internal bool OnPickObject(PickableObject pickableObject)
    {
        // TODO: implement picking behaviour
        // - check pickableObject.pickableType
        // - if object picked:
        // --  do the action associated to picking
        // - return true in case the object was picked
        Debug.Log("Pick " + pickableObject.name + "(" + pickableObject.objectType+")");
        return true;
    }
}
