using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    //Missel do inimigo, no start é setado a posiçao e o collider destroi o objeto se colidir com uma parede
    //invisivel, nos cantos da tela.
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        this.rigidbody2d.velocity = new Vector2(this.rigidbody2d.velocity.x, -5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
        
    }
}
