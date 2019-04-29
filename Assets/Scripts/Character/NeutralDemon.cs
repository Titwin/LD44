using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralDemon : MonoBehaviour
{
    Vector3 initPoisition;
    // Start is called before the first frame update
    void Start()
    {
        initPoisition = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = initPoisition + new Vector3(0, Mathf.Sin(Time.time * 5)*0.1f,0f);

        if (Player.thePlayer) {
            this.transform.localEulerAngles = new Vector3(0, Player.thePlayer.transform.position.x > this.transform.position.x ? 180 : 0,0) ;
        }
    }
}
