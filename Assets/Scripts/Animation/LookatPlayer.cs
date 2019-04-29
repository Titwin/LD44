using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatPlayer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Player.thePlayer)
        {
            this.transform.localEulerAngles = new Vector3(0, Player.thePlayer.transform.position.x > this.transform.position.x ? 0 : 180, 0);
        }
    }
}
