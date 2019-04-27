using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    void DoDamage(int amount);

    int GetLayer();
}
