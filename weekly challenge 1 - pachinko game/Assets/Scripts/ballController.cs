using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ballController : MonoBehaviour
{

    private Rigidbody2D ball;
    public float force;

    private Vector3 startPosition;
    private Vector3 relativePos;


    private int damage = 0;
    private int enemyDamage = 10;
    private int enemyHealth = 100;
    private int playerHealth = 100;

    private bool canFire = true;

    public TMP_Text playerHealth_txt;
    public TMP_Text enemyHealth_txt;
    public TMP_Text endText_txt;

    // Start is called before the first frame update
    void Start()
    {
        ball = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        enemyHealth_txt.text = 100 + "/100";
        playerHealth_txt.text = 100 + "/100";
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth > 0 && enemyHealth > 0)
        {
            if (canFire == true)
            {
                if (transform.position == startPosition)
                {
                    Vector3 worldposMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    relativePos = worldposMouse - transform.position;

                    float rot_z = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    ball.constraints = RigidbodyConstraints2D.None;
                    ball.AddForce(relativePos.normalized * force);
                }
            }
            else if (canFire == false)
            {

                enemyHealth -= damage;
                damage = 0;

                enemyHealth_txt.text = enemyHealth + "/100";
                //Debug.Log("enemy health: " + enemyHealth + "/100");


                playerHealth -= enemyDamage;

                playerHealth_txt.text = playerHealth + "/100";
                //Debug.Log("player health: " + playerHealth + "/100");


                canFire = true;
            }
        }
        else if (enemyHealth <= 0)
        {
            endText_txt.text = "VICTORY!";
        }
        else
        {
            endText_txt.text = "DEFEATED!";
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Ground")
        {
            transform.position = startPosition;
            ball.constraints = RigidbodyConstraints2D.FreezePosition;
            ball.velocity = Vector3.zero;
            ball.angularVelocity = 0;
            canFire = false;
        }

        if(other.tag == "Peg")
        {
            damage++;
        }
    }
}
