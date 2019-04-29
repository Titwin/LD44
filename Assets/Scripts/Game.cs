using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject introduction;

    public float gameOverDuration;
    public GameObject respawn;
    public Player player;
    public new CameraController camera;
    public GameObject resetables;
    public HUD hud;
    public End end;

    private GameObject instanciatedScene;
    private GameObject instanciatedPlayer;

    public bool PlayerDied { get; private set; }

    protected virtual void Start()
    {
        introduction.SetActive(true);
        hud.gameObject.SetActive(false);

        PlayerDied = false;
        resetables.SetActive(false);
        player.gameObject.SetActive(false);
        
    }

    protected virtual void LateUpdate()
    {
        if (introduction.activeSelf)
        {
            if(Input.anyKey)
            {
                introduction.SetActive(false);

                hud.gameObject.SetActive(true);
                Initialize();
            }
        }
        if (!introduction.activeSelf)
        {
            if (!PlayerDied)
            {
                if (instanciatedPlayer.GetComponent<Player>().health.Value <= 0)
                {
                    StartCoroutine(Die());
                }
            }

            if (end.activated)
            {
                hud.end();
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

        var player = instanciatedPlayer.GetComponent<Player>();
        if (!player.GoesThroughOnDeath)
        {
            player.OnDeath(null);
        }

        yield return new WaitForSeconds(gameOverDuration);
        
        DestroyImmediate(instanciatedScene);
        DestroyImmediate(instanciatedPlayer);
        Initialize();
        instanciatedPlayer.transform.position = respawn.transform.position;
        PlayerDied = false;
    }
}
