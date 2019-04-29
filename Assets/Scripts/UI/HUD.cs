using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public RectTransform gameOverScreen;
    public CharacterController2D playerControl;
    private AnimationController playerAnimator;

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

            if (game.IsOver)
            {
                //playerAnimator = playerControl.ac;
                playerControl.enabled = false;
            }
            else playerAnimator = null;

            //if (playerAnimator)
            //    playerAnimator.playAnimation(AnimationController.AnimationType.DYING);
        }
        catch (System.Exception e)
        {

        }
    }

    protected virtual void UpdateHealthBar()
    {
        float healthPercentage = Mathf.Clamp01(game.Player.health.Value / (float)game.Player.health.Max);

        var healthBarScale = healthBar.transform.localScale;
        healthBarScale.x = healthBarStartScale.x * healthPercentage;
        healthBar.transform.localScale = healthBarScale;

        healthText.text = game.Player.health.Value.ToString("00");
    }
}
