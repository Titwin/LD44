using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Player")]
    public Player player;

    [Header("Health")]
    public Image healthBar;
    public Text healthText;

    [Header("You Died Screen")]
    public RectTransform youDiedScreen;
    public CanvasRenderer youDiedScreenBacgkround;
    public float youDiedScreenFadeDuration;

    [Header("You Died Screen Text")]
    public Image youDiedText;
    public List<Sprite> youDiedTexts;
    public float youDiedScreenTextFadeDuration;

    public GameObject youWonScreen;

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

        if (game.PlayerDied && !wasOver)
        {
            player.GetComponent<CharacterController2D>().enabled = false;
            youDiedScreen.gameObject.SetActive(true);
            StartCoroutine(ShowYouDiedScreen());
        }
        else if (!game.PlayerDied && wasOver)
        {
            youDiedScreen.gameObject.SetActive(false);
            player.GetComponent<CharacterController2D>().enabled = true;
        }

        wasOver = game.PlayerDied;
    }

    public void end()
    {
        StartCoroutine(ShowYouWonScreen());
    }

    protected virtual void UpdateHealthBar()
    {
        float healthPercentage = Mathf.Clamp01(player.health.Value / (float)player.health.Max);

        var healthBarScale = healthBar.transform.localScale;
        healthBarScale.x = healthBarStartScale.x * healthPercentage;
        healthBar.transform.localScale = healthBarScale;

        healthText.text = player.health.Value.ToString("00")+"/"+ player.health.Max;
    }

    protected virtual IEnumerator ShowYouDiedScreen()
    {
        float duration = 0;

        int textIndex = 0;
        float currentTextDuration = 0;
        float textDuration = youDiedScreenTextFadeDuration / youDiedTexts.Count;

        do
        {
            float durationPercentage = Mathf.Clamp01(duration / youDiedScreenFadeDuration);
            youDiedScreenBacgkround.SetAlpha(durationPercentage);

            if (textIndex < youDiedTexts.Count - 1)
            {
                if (currentTextDuration > textDuration)
                {
                    currentTextDuration = 0;
                    textIndex++;
                }
                youDiedText.sprite = youDiedTexts[textIndex];
            }

            yield return null;

            duration += Time.deltaTime;
            currentTextDuration += Time.deltaTime;
        }
        while (duration <= youDiedScreenFadeDuration) ;
    }

    protected virtual IEnumerator ShowYouWonScreen()
    {
        youWonScreen.SetActive(true);
        yield return new WaitForSeconds(10000);
    }
}
