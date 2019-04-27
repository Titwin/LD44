using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public float gameOverDuration;

    public Health Health { get; private set; }

    public CharacterController2D Character { get; private set; }

    public bool IsOver { get; private set; }

    protected virtual void Start()
    {
        Health = FindObjectOfType<Health>();
        Character = FindObjectOfType<CharacterController2D>();
    }

    protected virtual void LateUpdate()
    {
        if (!IsOver)
        {
            if (Health.Value <= 0)
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
