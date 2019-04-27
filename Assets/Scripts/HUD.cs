using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Game game;

    public Image healthBar;

    public RectTransform gameOverScreen;

    protected Vector3 healthBarStartScale;

    protected virtual void Awake()
    {
        healthBarStartScale = healthBar.transform.localScale;
    }

    protected virtual void LateUpdate()
    {
        float healthPercentage = game.Health.Value / game.Health.StartValue;

        var healthBarScale = healthBar.transform.localScale;
        healthBarScale.x = healthBarStartScale.x * healthPercentage;
        healthBar.transform.localScale = healthBarScale;
    }
}
