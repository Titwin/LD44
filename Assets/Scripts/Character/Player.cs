using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController2D))]
public class Player : Character
{
    public CharacterController2D Controller { get; private set; }
    [SerializeField] Weapon[] weapons;
    private Dictionary<AnimationController.Weapon, AnimationController> animationControllers = new Dictionary<AnimationController.Weapon, AnimationController>();
    //private Dictionary<Weapon.Type, Weapon> weaponDictionary = new Dictionary<Weapon.Type, Weapon>();

    // comodity property for AI, can be replaced with line-of-sight player detection
    public static Player thePlayer;

    public bool GoesThroughOnDeath { get; protected set; }

    public Interactable Interactable { get; set; }
    private bool dying = false;
    public bool IsInteracting { get; protected set; }

    public AudioClip pickUpClip;
    public AudioClip pickUpWeaponClip;

    protected void Awake()
    {
        Controller = GetComponent<CharacterController2D>();
        thePlayer = this;

        AnimationController[] aclist = GetComponents<AnimationController>();
        foreach(AnimationController ac in aclist)
        {
            animationControllers[ac.weapon] = ac;
        }
        /*foreach(Weapon w in weapons)
        {
            weaponDictionary[w.type] = w;
        }*/
        SetWeapon(this.weapon);
    }

    protected void Update()
    {
        // set the movement actions
        float movementX = Input.GetAxis("Horizontal");
        bool jump = Input.GetButtonDown("Jump");
        bool duck = Input.GetButton("Duck");
        bool attack = Input.GetButtonDown("Fire1");
        Controller.Move(movementX, jump, attack, duck);

        if (Controller.canAttack && attack)
        {
            Attack();
        }

        if (duck)
        {
            if (Interactable != null && Interactable.CanInteract(this))
            {
                StartCoroutine(Interact());
            }
        }
    }

    public bool TryPick(MonoBehaviour item)
    {
        // TODO: implement picking behaviour
        // - check pickableObject.pickableType
        // - if object picked:
        // --  do the action associated to picking
        // - return true in case the object was picked

        var weapon = item as Weapon;
        if (weapon != null)
        {
            SetWeapon(weapon);
            audioSource.PlayOneShot(pickUpWeaponClip);
        }

        var consumable = item as ConsumableItem;
        if (consumable != null)
        {
            health.Heal(item.gameObject, consumable.healthModifier);
            audioSource.PlayOneShot(pickUpClip);
        }

        Debug.Log(name + " has picked " + item.name + "(" + item.GetType() + ")");
        return true;
    }

    protected virtual void SetWeapon(Weapon weapon)
    {
        this.weapon.damages = weapon.damages;
        //int delta = weapon.healthModifier - this.weapon.healthModifier;
        int delta = Mathf.Min(health.Value - 1, 2);
        //if (delta>0) health.Heal(weapon.gameObject,delta);
        //else if (delta < 0)
            health.Hurt(weapon.gameObject, delta);

        foreach(Weapon w in weapons)
        {
            w.gameObject.SetActive(w.type == weapon.type);
            if (w.type == weapon.type) this.weapon = w;
        }
        AnimationController.Weapon t;
        switch (weapon.type)
        {
            case Weapon.Type.SWORD: t = AnimationController.Weapon.SWORD; break;
            case Weapon.Type.MAGIC: t = AnimationController.Weapon.MAGIC; break;

            default: t = AnimationController.Weapon.SWORD; break;
        }
        foreach(KeyValuePair<AnimationController.Weapon, AnimationController> ac in animationControllers)
        {
            if(ac.Key == t)
            {
                ac.Value.enabled = true;
            }
            else
            {
                ac.Value.enabled = false;
            }
        }
        Controller.ac = animationControllers[t];
    }

    internal override void OnDeath(GameObject source)
    {
        if (dying) return;

        audioSource.PlayOneShot(deathClip);

        GoesThroughOnDeath = true;

        thePlayer = null;
        StartCoroutine(DeathAnimation());
        dying = true;
    }
    internal override void OnHurt(GameObject source)
    {
        if (dying) return;
        base.OnHurt(source);
    }
    protected virtual IEnumerator DeathAnimation()
    {
        foreach (Sprite s in Controller.ac.animationDying)
        {
            Controller.ac.sr.sprite = s;
            yield return new WaitForSeconds(Controller.ac.timeDying);
        }
    }
    protected virtual IEnumerator Interact()
    {
        if (!IsInteracting)
        {
            IsInteracting = true;

            Interactable.DoInteract(this);
            audioSource.PlayOneShot(pickUpClip);
            yield return new WaitForSeconds(Interactable.interactionDuration);

            IsInteracting = false;
        }
    }
}
