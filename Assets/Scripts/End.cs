using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public bool activated = false;
    public LayerMask playerMask;

    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer) == playerMask)
        {
            activated = true;
        }
    }
}
