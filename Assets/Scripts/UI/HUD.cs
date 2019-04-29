using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Player")]
    public CharacterController2D playerControl;
    public Player player;

    [Header("Health")]
    public Image healthBar;
    public Text healthText;

    [Header("Game Over Screen")]
    public CanvasGroup gameOverScreen;
    public float gameOverScreenFadeDuration;

    protected Game game;
    protected Vector3 healthBarStartScale;
    protected bool wasOver;

    protected virtual void Awake()
    {
        game = FindObjectOfType<Game>();
        healthBarStartScale = healthBar.transform.localScale;
    }

    protected virtual void LateUpdate()
    {
        UpdateHealthBar();

        if (game.IsOver && !wasOver)
        {
            player.GetComponent<CharacterController2D>().enabled = false;
            gameOverScreen.gameObject.SetActive(true);
            StartCoroutine(ShowGameOverScreen());
        }
        else if (!game.IsOver && wasOver)
        {
            gameOverScreen.gameObject.SetActive(false);
            player.GetComponent<CharacterController2D>().enabled = true;
        }

        wasOver = game.IsOver;
    }

    protected virtual void UpdateHealthBar()
    {
        float healthPercentage = Mathf.Clamp01(player.health.Value / (float)player.health.Max);

        var healthBarScale = healthBar.transform.localScale;
        healthBarScale.x = healthBarStartScale.x * healthPercentage;
        healthBar.transform.localScale = healthBarScale;

        healthText.text = player.health.Value.ToString("00")+"/"+ player.health.Max;
    }

    protected virtual IEnumerator ShowGameOverScreen()
    {
        float duration = 0;

        do
        {
            float durationPercentage = duration / gameOverScreenFadeDuration;
            gameOverScreen.alpha = durationPercentage;
            yield return null;

            duration += Time.deltaTime;
        }
        while (duration <= gameOverScreenFadeDuration) ;
    }
}
