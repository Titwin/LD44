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

    // Start is called before the first frame update
    void Start()
    {
        messagesCoroutine = messagesDraw();
        StartCoroutine(messagesCoroutine);
        messagesDraw();
    }

    // Update is called once per frame
    void Update()
    {
        
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
