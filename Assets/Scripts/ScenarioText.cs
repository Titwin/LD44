using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioText : MonoBehaviour
{
    public GameObject textGO;
    public Text text;
    public float timer;
    public string[] messages;

    private IEnumerator messagesCoroutine;
    private bool alreadyLaunched = false;

    
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject)
        {
            alreadyLaunched = true;
            messagesCoroutine = messagesDraw();
            StartCoroutine(messagesCoroutine);
            messagesDraw();
        }
    }

    private IEnumerator messagesDraw()
    {
        textGO.SetActive(true);
        foreach (string msg in messages)
        {
            text.text = msg;
            yield return new WaitForSeconds(timer);
        }
        textGO.SetActive(false);
    }
}
