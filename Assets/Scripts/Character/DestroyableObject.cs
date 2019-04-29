using UnityEngine;

public class DestroyableObject : Attackable
{
    [SerializeField] GameObject loot;

    internal override void OnDeath(GameObject source)
    {
        base.OnDeath(source);
        if (loot != null)
        {
            GameObject body = Instantiate(loot);
            body.transform.position = this.transform.position;
            body.SetActive(true);
        }
    }
}
