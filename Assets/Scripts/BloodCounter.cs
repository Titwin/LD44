using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCounter : Interactable
{
    [SerializeField] GameObject particleTemplate;
    [SerializeField] Transform particleContainer;
    [SerializeField] int max = 10;
    [SerializeField] int current = 0;

    [SerializeField] List<GameObject> particles = new List<GameObject>();
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
            particles.Add(go);
            go.SetActive(true);

            ++current;
        }
    }
    void Clear()
    {
        
    }
    bool interacting = false;
    IEnumerator DoDrip(int quantity)
    {
        if (!interacting)
        {
            interacting = true;
            for (int i = 0; i < quantity; ++i)
            {
                Drip();
                yield return new WaitForEndOfFrame();
            }
            interacting = false;
        }
        
    }
    IEnumerator DoClear()
    {
        if (!interacting)
        {
            interacting = true;
            foreach (GameObject particle in particles)
            {
                GameObject.Destroy(particle);
                yield return new WaitForEndOfFrame();
            }
            particles.Clear();
            current = 0;
            interacting = false;
        }
        
    }
    public override bool CanInteract(Character character)
    {
        return character.gameObject.tag == "Player";
    }

    public override bool DoInteract(Character character)
    {
        Debug.Log(character.name + " interacting with " + this.name);
        if (CanInteract(character))
        {
            if (current == 0)
            {
                StartCoroutine(DoDrip(50));
                character.DoDamage(null, 1);
                return true;
            }
            else
            {
                StartCoroutine(DoClear());
                character.health.Value += 1;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
