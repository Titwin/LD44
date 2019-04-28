using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : Attackable
{
    [SerializeField] GameObject loot;

    protected override void OnDeath(GameObject source)
    {
        base.OnDeath(source);
        if (loot!=null)
        {
            GameObject body = GameObject.Instantiate<GameObject>(loot);
            body.transform.position = this.transform.position;
            body.SetActive(true);
        }
    }
}
