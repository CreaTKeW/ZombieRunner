using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] Transform rayStart;
    public float speed = 1f;
    private Animator anim;
    private bool rightTurn = true;
    private Rigidbody rb;
    private GameManager gameManager;
    public AudioSource pointSound;
    public GameObject crystalParticleEffect;
    public GameObject EndGameUI;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameManager.gameStarted)
        {
            return;
        }
        else { anim.SetTrigger("gameStarted"); }
        rb.transform.position = transform.position + transform.forward * speed * Time.deltaTime;       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchSides();
        }

        RaycastHit hit;
        if(!Physics.Raycast(rayStart.position, -transform.up, out hit, Mathf.Infinity))
        {
            anim.SetTrigger("isDead");
        }
        else { anim.SetTrigger("isAlive"); }

        if(transform.position.y < -2f)
        {
            gameManager.EndGame();
            EndGameUI.SetActive(true);
        }
    }

    void SwitchSides()
    {
        if(!gameManager.gameStarted)
        {
            return;
        }

        rightTurn = !rightTurn;
        if(rightTurn)
        {
            rb.transform.rotation = Quaternion.Euler(0f, 45f, 0f);
        }
        else 
        {
            rb.transform.rotation = Quaternion.Euler(0f, -45f, 0f);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Crystal")
        {          
            gameManager.IncreaseScore();
            pointSound.Play();
            GameObject particle = Instantiate(crystalParticleEffect, rayStart.transform.position, Quaternion.identity);
            Destroy(particle, 2f);
            Destroy(other.gameObject);
        }
    }
}
