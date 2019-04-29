using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public float gameOverDuration;
    public GameObject respawn;
    public Player player;
    public CameraController camera;
    public GameObject resetables;
    public HUD hud;

    private GameObject instanciatedScene;
    private GameObject instanciatedPlayer;
    //public Player Player { get; private set; }

    public bool IsOver { get; private set; }

    protected virtual void Start()
    {
        //Player = FindObjectOfType<Player>();
        resetables.SetActive(false);
        player.gameObject.SetActive(false);
        initialize();
    }

    protected virtual void LateUpdate()
    {
        if (!IsOver)
        {
            if (instanciatedPlayer.GetComponent<Player>().health.Value <= 0)
            {
                StartCoroutine(Over());
            }
        }
    }

    private void initialize()
    {
        instanciatedScene = Instantiate(resetables);
        instanciatedScene.SetActive(true);
        instanciatedPlayer = Instantiate(player.gameObject);
        instanciatedPlayer.SetActive(true);

        camera.target = instanciatedPlayer.transform;
        hud.player = instanciatedPlayer.GetComponent<Player>();
        hud.playerControl = instanciatedPlayer.GetComponent<CharacterController2D>();
    }

    protected IEnumerator Over()
    {
        IsOver = true;
        yield return new WaitForSeconds(gameOverDuration);
        
        DestroyImmediate(instanciatedScene);
        DestroyImmediate(instanciatedPlayer);
        initialize();
        instanciatedPlayer.transform.position = respawn.transform.position;
        IsOver = false;
    }
}
