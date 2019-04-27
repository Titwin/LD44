using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public RectTransform gameOverScreen;

    [Header("Health")]
    public Image healthBar;
    public Text healthText;

    protected Game game;
    protected Vector3 healthBarStartScale;

    protected virtual void Awake()
    {
        game = FindObjectOfType<Game>();
        healthBarStartScale = healthBar.transform.localScale;
    }

    protected virtual void LateUpdate()
    {
        UpdateHealthBar();

        gameOverScreen.gameObject.SetActive(game.IsOver);
    }

    protected virtual void UpdateHealthBar()
    {
        float healthPercentage = Mathf.Clamp01(game.Player.Health.Value / (float)game.Player.Health.StartValue);

        var healthBarScale = healthBar.transform.localScale;
        healthBarScale.x = healthBarStartScale.x * healthPercentage;
        healthBar.transform.localScale = healthBarScale;

        healthText.text = game.Player.Health.Value.ToString("00");
    }
}
