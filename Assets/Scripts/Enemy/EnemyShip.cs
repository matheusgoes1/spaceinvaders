using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    [SerializeField]
    float velocity, raySize;

    static float xPosition = -7.2f, yPosition =0;

    float savedPosition;
    [SerializeField]
    public bool moveRight=true;
    Rigidbody2D rigidbody2d;
    Collider2D collider;

    
    private void Awake()
    {
        ShipPosition();
    }
    void Start()
    {
        this.rigidbody2d = GetComponent<Rigidbody2D>();
        this.collider = GetComponent<Collider2D>();
    }

   
    void Update()
    {
        Movement();
    }

    //Organiza a posiçao das naves, assim quando elas forem geradas pelo EnemyManger ficaram organizadas.
    void ShipPosition()
    {
        Vector3 startPosition = this.transform.position;
        startPosition.x += xPosition;
        startPosition.y += yPosition;
        this.transform.position = startPosition;
        xPosition = xPosition + 2;
        if (xPosition >= 7.5f)
        {
            xPosition = -7.2f;
            yPosition += 2;
        }
        if (yPosition >= 5)
        {
            yPosition = 0;
        }
    }

    //Movimentaçao base da EnemyShip, que é controlada pelo EnemyManager, alterando o valor de moveRight.
    void Movement()
    {
        if (moveRight)
        {
            this.rigidbody2d.velocity = new Vector2(velocity, this.rigidbody2d.velocity.y);
        }

        else
        {
            this.rigidbody2d.velocity = new Vector2(-velocity, this.rigidbody2d.velocity.y);
        }
    }

    //Checa se ha algum obstaculo, sendo usado pela primeira e ultima nave para definir o movimento da fileira.
    public int CheckMovement()
    {
        RaycastHit2D leftRay = Physics2D.Raycast(this.transform.position, Vector2.left, this.raySize);
        RaycastHit2D rightRay = Physics2D.Raycast(this.transform.position, Vector2.right, this.raySize);

        if (rightRay.transform != null)
        {
            return 1;
        }

        else if (leftRay.transform != null)
        {
            return 2;
        }

        else
        {
            return 3;
        }
    }

    //Instancia um prefab.
    public void Shoot(GameObject missile)
    {
        Vector3 missileSpawnPosition = this.transform.position;
        missileSpawnPosition.y -= 0.7f;
        GameObject basicMissile = GameObject.Instantiate(missile, missileSpawnPosition, Quaternion.Euler(new Vector3(0,0,180)));
    }

    //Checa se o missel do Player atingiu a nave.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Missile"))
        {
            AudioManager.instance.PlaySound("EnemyDeath");
            Destroy(collision.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    //Metodo usado pelo EnemyManager para destruir o lixo de memoria.
    public void DestroyShip()
    {
        GameManager.instance.AddPoint(1);
        Destroy(this.gameObject);
    }

    //Auxilio no tamanho do raycast.
    private void OnDrawGizmos()
    {
        Vector2 drawVector = this.transform.position;
        drawVector.x += raySize;
        Gizmos.DrawLine(drawVector, this.transform.position);
    }
}
