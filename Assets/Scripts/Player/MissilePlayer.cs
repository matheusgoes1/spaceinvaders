using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePlayer : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    //Seta a velocidade do missel e o destroy quando colide em algo.
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        this.rigidbody2d.velocity = new Vector2(this.rigidbody2d.velocity.x, 7);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
