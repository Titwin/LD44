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

    
    void OnTrigerEnter2D(Collision2D col)
    {
        Debug.Log("TOTO");
        if (!alreadyLaunched && col.gameObject.tag == "Player")
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
