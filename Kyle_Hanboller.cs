using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kyle_Hanboller : MonoBehaviour
{
    GameObject battery;
    GameObject player;
    public Animator anim;
    public GameObject killEffect;
    public GameObject killEffect_2;
    public GameObject hitEffect;
    public GameObject smoke;
    public GameObject Ball_run;
    public GameObject Ball_shoot;
    private int health;
    public int lifes;
    public float ShootingDistance = 100f;
    public float bulletspeed;
    // public GameObject robotic_ball;
    public float robotspeed = 0.1f;
    Rigidbody rb;
    Rigidbody rb_ball;
    bool dead;
    bool ishooted;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        anim = gameObject.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        // Ball_shoot = GameObject.FindWithTag("Kyleball"); 
        battery = GameObject.FindWithTag("battery");
        transform.LookAt(battery.transform.position);
        // killEffect.SetActive(false);
        smoke.SetActive(false);
        health = lifes;
    }

    //Update is called once per frame
    void Update()
    {
        //print("I`m deadmen health" + health);
        Vector3 targetPostition = new Vector3(battery.transform.position.x,
                                          this.transform.position.y,
                                          battery.transform.position.z);

        float dist1 = Vector3.Distance(transform.position, battery.transform.position);

        if (dist1 > ShootingDistance && !dead)
        {
            transform.position = Vector3.Slerp(transform.position, battery.transform.position, robotspeed * Time.deltaTime);
        }
        else if (dist1 <= ShootingDistance && !ishooted)
        {
            anim.SetTrigger("StartShooting");
            Invoke("ShootBall", 3f);
            ishooted = true;

        }

        transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);

        if (health == 0 && anim != null)
        {

            Die();

        }
    }


    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "Bullet")
        {
            if (health > 0)
            {
                health = health - 1;
                robotspeed *= 0.8f;
                anim.SetTrigger("Injured");
                smoke.SetActive(true);
                GameObject go = Instantiate(hitEffect, transform.position, transform.rotation) as GameObject;
                Destroy(go, 1f);
            }
        }
    }

    void ShootBall()
    {
        Ball_run.SetActive(false);
        Ball_shoot.SetActive(true);
        rb_ball = Ball_shoot.GetComponent<Rigidbody>();
        rb_ball.velocity = new Vector3(0f, 0f, 0f);
        rb_ball.transform.LookAt(battery.transform.position + new Vector3 (Random.Range (-5,6), Random.Range (1,5), Random.Range(-10,11)));

        rb_ball.AddForce(transform.forward * bulletspeed, ForceMode.Impulse);
        Die();
    }

    public void Die()
    {
        rb.velocity = new Vector3(0, 0, 0);
        smoke.SetActive(false);
        health = -1;
        anim.SetTrigger("Die");
        // GameObject gop1 = Instantiate(killEffect_2, transform.position, transform.rotation) as GameObject;
        // gop1.SetActive(true);
        // Destroy(gop1, 5f);
        Invoke("Bang", 1.5f);
    }

    public void Bang()
    {
        // GameObject gop = Instantiate(killEffect, transform.position, transform.rotation) as GameObject;
        // gop.SetActive(true);
        // Destroy(gop, 3.5f);
        Destroy(gameObject, 1f);


    }


}
