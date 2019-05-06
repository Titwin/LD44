using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class IntroductionUI : MonoBehaviour
{

    [SerializeField] Text pressStart;
    [SerializeField] Image cover;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = pressStart.transform.position;
    }
    float t = 0;
    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t / 3 <= 1) {
            pressStart.color = new Color(1, 1, 1, t / 3f);
        }
        pressStart.transform.position = startPos + new Vector3(0, Mathf.Sin(Time.time*5)*10, 0);
        pressStart.transform.localEulerAngles = new Vector3(0,0, Mathf.Sin(Time.time * 2) * 3);
        cover.transform.localScale = Vector3.one * (1 + Mathf.Sin(Time.time * 3)*0.05f);
    }
}
