using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public float gameOverDuration;

    public Player Player { get; private set; }

    public bool IsOver { get; private set; }

    protected virtual void Start()
    {
        Player = FindObjectOfType<Player>();
    }

    protected virtual void LateUpdate()
    {
        if (!IsOver)
        {
            if (Player.Health.Value <= 0)
            {
                StartCoroutine(Over());
            }
        }
    }

    protected IEnumerator Over()
    {
        IsOver = true;

        float startTime = Time.time;
        while (Time.time - startTime < gameOverDuration)
        {
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
