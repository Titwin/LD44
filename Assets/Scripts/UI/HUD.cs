using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public RectTransform gameOverScreen;
    public CharacterController2D playerControl;
    public Player player;

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
        try
        {
            UpdateHealthBar();

            gameOverScreen.gameObject.SetActive(game.IsOver);
            playerControl.enabled = !game.IsOver;
        }
        catch (System.Exception e)
        {

        }
    }

    protected virtual void UpdateHealthBar()
    {
        float healthPercentage = Mathf.Clamp01(player.health.Value / (float)player.health.Max);

        var healthBarScale = healthBar.transform.localScale;
        healthBarScale.x = healthBarStartScale.x * healthPercentage;
        healthBar.transform.localScale = healthBarScale;

        healthText.text = player.health.Value.ToString("00")+"/"+ player.health.Max;
    }
}
