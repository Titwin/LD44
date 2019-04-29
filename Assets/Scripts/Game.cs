using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    public float gameOverDuration;
    public GameObject respawn;
    public Player player;
    public new CameraController camera;
    public GameObject resetables;
    public HUD hud;

    private GameObject instanciatedScene;
    private GameObject instanciatedPlayer;

    public bool PlayerDied { get; private set; }

    protected virtual void Start()
    {
        PlayerDied = false;
        resetables.SetActive(false);
        player.gameObject.SetActive(false);
        Initialize();
    }

    protected virtual void LateUpdate()
    {
        if (!PlayerDied)
        {
            if (instanciatedPlayer.GetComponent<Player>().health.Value <= 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    private void Initialize()
    {
        instanciatedScene = Instantiate(resetables);
        instanciatedScene.SetActive(true);
        instanciatedPlayer = Instantiate(player.gameObject);
        instanciatedPlayer.SetActive(true);

        camera.target = instanciatedPlayer.transform;
        hud.player = instanciatedPlayer.GetComponent<Player>();
    }

    protected IEnumerator Die()
    {
        PlayerDied = true;
        yield return new WaitForSeconds(gameOverDuration);
        
        DestroyImmediate(instanciatedScene);
        DestroyImmediate(instanciatedPlayer);
        Initialize();
        instanciatedPlayer.transform.position = respawn.transform.position;
        PlayerDied = false;
    }
}
