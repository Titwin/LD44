using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCounter : Interactable
{
    [SerializeField] ActivatedObject activatedObject;

    [SerializeField] GameObject particleTemplate;
    [SerializeField] Transform particleContainer;
    [SerializeField] int max = 10;
    [SerializeField] int current = 0;
    [SerializeField] int damage = 3;

    [SerializeField] List<GameObject> particles = new List<GameObject>();

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
        return !interacting && character.gameObject.tag == "Player";
    }

    public override bool DoInteract(Character character)
    {
        Debug.Log(character.name + " interacting with " + this.name);
        if (CanInteract(character))
        {
            if (current == 0)
            {
                StartCoroutine(DoDrip(max));
                character.health.Suck(this.gameObject, damage);
                // hack to poke the death system
                if (character.health.Value<= 0)
                {
                    character.DoDamage(this.gameObject, 0);
                }
                if (activatedObject != null) activatedObject.SetActive(true);
            }
            else
            {
                StartCoroutine(DoClear());
                character.health.Heal(this.gameObject,damage);
                if (activatedObject != null) activatedObject.SetActive(false);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
