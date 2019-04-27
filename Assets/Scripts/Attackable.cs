using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable: MonoBehaviour
{
    public Health health;

    public void DoDamage(int amount)
    {
        health.Value -= amount;
        Debug.Log(this.name + " got attacked for " + amount + "dmg");
    }
}
