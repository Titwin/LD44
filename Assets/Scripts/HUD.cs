using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text healthBar;

    protected virtual void LateUpdate()
    {
        healthBar.text = Game.Instance.Health.Value.ToString("000");
    }
}
