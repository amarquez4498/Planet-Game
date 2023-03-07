using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    Rigidbody2D playerRigidbody;
    float inputX;

    public LayerMask wallLayer;
    public float rayLenght;
    bool CanJump;
    public float jumpheight;

    bool Hurt;
    public float maxHealth;
    [SerializeField]
    float health;
    public float timeBetweenHurt;
    float iframe;

    Animator anim;
    SpriteRenderer rend;


    int coins = 0;

    public Image healthImage;
    public Text coinsText;

    public GameObject gameoverUI;
    bool gameover;




    // Start is called before the first frame update
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        CanJump = true;
        health = maxHealth;
        Hurt = false;
        iframe = timeBetweenHurt;
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        gameover = false;

    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");

        if (inputX != 0)
            playerRigidbody.AddForce(Vector2.right * inputX * speed * Time.deltaTime);

        rend.flipX = (inputX < 0);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLenght, wallLayer);

        if (hit.collider != null)
        {
            CanJump = true;
        }

        if (CanJump && Input.GetKeyDown(KeyCode.Space))
        {
            playerRigidbody.AddForce(Vector2.up * jumpheight);
            CanJump = false;
        }


        Debug.DrawRay(transform.position, Vector2.down * rayLenght);

        if (iframe > 0) iframe -= Time.deltaTime;

        //Test Damage
        if (!Hurt && Input.GetKeyDown(KeyCode.LeftControl))
            Damage(2);

        healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, health / maxHealth, Time.deltaTime * 10f);
        coinsText.text = "X " + coins.ToString();

        anim.SetBool("moving", inputX != 0);
        anim.SetBool("canjump", CanJump);
        anim.SetBool("hurt", Hurt);

        if (gameover && Input.anyKeyDown)
        {
            SceneManager.LoadScene("PlanetSelection");
            Time.timeScale = 5f;
        }


    }

    public void Damage(float amt)
    {
        if (iframe < 0)
        {
            health -= amt;
            Hurt = true;
            Invoke("ResetHurt", 0.2f);

            if (health <= 0)
            {
                GameOver();
            }

            iframe = timeBetweenHurt;
        }

    }

    void GameOver()
    {
        gameover = true;
        gameoverUI.SetActive(true);
        Time.timeScale = 0f;
    }


    void ResetHurt()
    {
        Hurt = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            coins++;
            collision.gameObject.SetActive(false);
        }

       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && playerRigidbody.velocity.y < 0)
        {
            float boundsY = collision.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
            if (transform.position.y > collision.gameObject.transform.position.y + boundsY)
            {
                playerRigidbody.AddForceAtPosition(-playerRigidbody.velocity.normalized * jumpheight / 2, playerRigidbody.position);
                collision.gameObject.GetComponent<EnemyController>().Damage(5f);
            }
        }
    }

}
