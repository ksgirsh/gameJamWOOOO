using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    private SelectControl control;

    [SerializeField] GameObject rocketHoverEffect;

    private GameObject currentRocketObj;

    [SerializeField] LayerMask surfaceLayers;

    public RocketControl rocket;

    [SerializeField] GameObject[] earthPieces;
    private FadeIn fade;
    [SerializeField] GameObject gameOver;

    [Header("Sound")]
    [SerializeField] AudioClip[] sfx;
    [SerializeField] AudioSource music;
    [SerializeField] AudioClip gameOverTheme;

    bool dead;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        rocket = player.GetComponent<RocketControl>();
        fade = gameObject.GetComponent<FadeIn>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        if ((mainCamera.ScreenToWorldPoint(Input.mousePosition)).magnitude > 2 && !dead)
        {
            currentRocketObj = GameObject.Instantiate(rocketHoverEffect, MouseToSurfaceRay(this.transform), Quaternion.identity, this.transform);
            rocket.planet = gameObject;
        }

    }

    void OnMouseOver()
    {

        if (currentRocketObj != null && !dead)
        {
            Vector2 rayPoint = MouseToSurfaceRay(this.transform);
            currentRocketObj.transform.position = new Vector3(rayPoint.x, rayPoint.y, 0f);
            if ((mainCamera.ScreenToWorldPoint(Input.mousePosition)).magnitude < 10.04f)
            {

                OnMouseExit();
            } else if (currentRocketObj == null)
            {
                OnMouseEnter();
            }
        }
        

    }
    void OnMouseExit()
    {
        Destroy(currentRocketObj);
        rocket.planet = null;
    }

    public Vector2 MouseToSurfaceRay(Transform planetPos)
    {
        //cast ray from mouse position directed towards planet

        Vector2 distVector = (mainCamera.ScreenToWorldPoint(Input.mousePosition)) - planetPos.position;
       // Debug.Log(-(distVector.normalized));
        RaycastHit2D hit = Physics2D.Raycast((mainCamera.ScreenToWorldPoint(Input.mousePosition)), -(distVector.normalized), Mathf.Infinity, surfaceLayers);

        if (hit != null)
        {
            return hit.point;
        } else
        {
            return Vector2.zero;
        }

    }

    public Vector2 MouseToSurfaceNormal(Transform planetPos)
    {
        //cast ray from mouse position directed towards planet

        Vector2 distVector = (mainCamera.ScreenToWorldPoint(Input.mousePosition)) - planetPos.position;
       // Debug.Log(-(distVector.normalized));
        RaycastHit2D hit = Physics2D.Raycast((mainCamera.ScreenToWorldPoint(Input.mousePosition)), -(distVector.normalized), Mathf.Infinity, surfaceLayers);

        if (hit != null)
        {
            return hit.normal;
        }
        else
        {
            return Vector2.zero;
        }

    }

    public IEnumerator EarthDestroy()
    {
        music.Stop();
        SoundFXManager.instance.PlaySoundEffectClip(sfx[0], transform.position, 1f);
        yield return new WaitForSeconds(2f);

        Explode();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        dead = true;

        Destroy(currentRocketObj);
        rocket.planet = null;

        yield return new WaitForSeconds(2f);
        gameOver.SetActive(true);
        fade.FadeInObj(gameOver);

        yield return new WaitForSeconds(1f);
        music.gameObject.transform.SetParent(null);
        music.clip = gameOverTheme;
        music.Play();

        //StartCoroutine(GameOverUI(10f));
        
    }

    IEnumerator GameOverUI(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameOver.SetActive(true);
        fade.FadeInObj(gameOver);
        gameObject.SetActive(false);

    }

    void Explode()
    {
        Debug.Log("!!!!!");
        GameObject earthParent = (earthPieces[0].transform.parent).gameObject;

        earthParent.SetActive(true);
        earthParent.transform.SetParent(null);
        earthParent.transform.position = Vector2.zero;




        Time.timeScale = 0.6f;
        UISingleton.instance.playUI.SetActive(false);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.SetActive(false);


        foreach (GameObject piece in earthPieces)
        {
            Vector2 randomDir = (Random.insideUnitCircle).normalized;
            Rigidbody2D pieceRB = piece.GetComponent<Rigidbody2D>();

            int forceStrength = Random.Range(5, 8);

            pieceRB.AddForce(randomDir * forceStrength, ForceMode2D.Impulse);
            float lifeTime = Random.Range(5f, 9f);

            Destroy(piece, (lifeTime * 0.6f));
        }

    }
}
