using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCounter : Interactable
{
    [SerializeField] GameObject particleTemplate;
    [SerializeField] Transform particleContainer;
    [SerializeField] int max = 10;
    [SerializeField] int current = 0;
    float t = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*t += Time.deltaTime;
        if (t > 1)
        {
            t = 0;
            Drip();
        }*/
    }

    void Drip()
    {
        if (current < max)
        {
            GameObject go = GameObject.Instantiate(particleTemplate);
            go.transform.parent = particleContainer;
            go.transform.position = particleTemplate.transform.position+ new Vector3(Random.Range(-0.05f,0.05f), Random.Range(-0.01f, 0.01f), 0);
            
            go.SetActive(true);

            ++current;
        }
    }
    IEnumerator DoDrip(int quantity)
    {
        for (int i = 0; i < quantity; ++i)
        {
            Drip();
            yield return new WaitForEndOfFrame();
        }
    }
    public override bool CanInteract(Character character)
    {
        return character.gameObject.tag == "Player" && current < max;
    }

    public override bool DoInteract(Character character)
    {
        Debug.Log(character.name + " interacting with " + this.name);
        if (CanInteract(character))
        {
            StartCoroutine(DoDrip(10));
            character.DoDamage(null, 1);
            return true;
        }
        else
        {
            return false;
        }
    }
}
