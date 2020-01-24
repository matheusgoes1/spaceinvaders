using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField]
    float velocity,raySize,elapsedTime,rayForce,modeController,shootTime;
    Player player;
    [SerializeField]
    GameObject gravityRay,missile, shield;

    [SerializeField]
    Collider2D rayCollider, shipCollider;
    Rigidbody2D rigidbody2d;

    [SerializeField]
    Slider healthBar;
    bool moveRight, checkRay=true, attackMode;

    //Seta o raycast usado no defense mode.
    private void Awake()
    {
        this.rayCollider.enabled = false;
        LTDescr tween;
        tween = LeanTween.scale(this.gravityRay, Vector3.zero, 0);
    }
    void Start()
    {
        player = FindObjectOfType<Player>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {
        BossModeController();
    }

    //É o controlador do Boss, alterna os modos de ataque e defesa, e checa se o Boss morreu;
    void BossModeController()
    {
        if (healthBar.value <= 0)
        {
            GameManager.instance.AddPoint(5);
            GameManager.winCount = 0;
            ScreenManager.instance.WinScreen();
            Destroy(this.gameObject);
        }
        velocity = 5;
        DefenseMode();
        modeController += Time.deltaTime;
        if (modeController > 10)
        {
            velocity = 2;
            AttackMode();
            if (modeController > 20)
            {
                modeController = 0;
                DefenseTools();
                checkRay = true;
                attackMode = false;
            }
        }

    }
    

    //Movimentaçao base do boss no modo defensivo.Foram definidos valores para que o boss fique na area util do jogo.
    void DefenseMovement()
    {
        if(moveRight && this.transform.position.x <= 6)
        {
            this.rigidbody2d.velocity = new Vector2(velocity, this.rigidbody2d.velocity.y);
        }

        else if (!moveRight && this.transform.position.x >= -6)
        {
            this.rigidbody2d.velocity = new Vector2(-velocity, this.rigidbody2d.velocity.y);
        }
        else
        {
            this.rigidbody2d.velocity = new Vector2(0, 0);
        }
    }

    //Mini IA que ao jogador chegar perto do Boss, ele se afasta para a direçao oposta.
    //Alem disso, ele recebe menos dano do Player, AtkSpeed é menor e velocidade maior;
    //O elapsedTime na formaçao do raycast server para impedir que ele seja ativado freneticamente,
    //o que causaria um travamento no boss (ele travaria em cima do player indo pra esquerda e direita).
    void DefenseMode()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime>= 1)
        {
            RaycastHit2D ray = Physics2D.Raycast(this.transform.position, Vector2.down, raySize);
            if (ray.transform != null)
            {
                moveRight = !moveRight;
                elapsedTime = 0;
            }
        }
        shootTime += Time.deltaTime;
        if (shootTime > 2)
        {
            Shoot();
            shootTime = 0;
        }
        DefenseMovement(); 
        
    }

    //Movimentaçao base do boss no modo ofensivo.Ele persegue o jogador.
    void AttackMovement()
    {
        if(this.transform.position.x<= player.transform.position.x+0.2f && 
            this.transform.position.x >= player.transform.position.x - 0.2f)
        {
            this.rigidbody2d.velocity = new Vector2(0, 0);
        }

        else if(player.transform.position.x> this.transform.position.x)
        {
            this.rigidbody2d.velocity = new Vector2(velocity, this.rigidbody2d.velocity.y);
        }

        else
        {
            this.rigidbody2d.velocity = new Vector2(-velocity, this.rigidbody2d.velocity.y);
        }
    }

    //Mini IA em que o boss persegue o jogador.
    //Alem disso, ele recebe mais dano do Player, AtkSpeed é maior e velocidade menor;
    void AttackMode()
    {
        AttackMovement();
        shootTime += Time.deltaTime;
        if (shootTime > 1.7)
        {
            Shoot();
            shootTime = 0;
        }
        if (checkRay)
        {
            AttackTools();
            checkRay = false;
            attackMode = true;
        }
    }

    //Ativa a ferramenta de ataque, o raio anti gravidade.
    void AttackTools()
    {
        this.rayCollider.enabled = true;
        this.gravityRay.gameObject.SetActive(true);
        this.gravityRay.gameObject.transform.localScale = Vector3.zero;
        LTDescr tween;
        tween = LeanTween.scale(this.gravityRay, new Vector3(3.4f, 5.3f, 1), 0.4f);
        tween.setFrom(Vector3.zero);
        tween = LeanTween.scale(this.shield, Vector3.zero, 0.2f);
    }

    //Ativa a ferramenta de defesa, o escudo.
    void DefenseTools()
    {
        this.rayCollider.enabled = false;
        LTDescr tween;
        tween = LeanTween.scale(this.gravityRay, Vector3.zero, 0.2f);
        tween = LeanTween.scale(this.shield, Vector3.one, 0.5f);
        tween.setFrom(0);
        tween.setEase(LeanTweenType.easeOutQuad);
    }

    //O numero Random serve para parte mais estetica, para escolher em qual canhao vai sair o missel.
    //O missel é instanciado a partir de um prefab.
    public void Shoot()
    {
        int r = Random.Range(1, 3);
        if (r == 1)
        {
            Vector3 missileSpawnPosition = this.transform.position;
            missileSpawnPosition.y -= 1.7f;
            missileSpawnPosition.x -= 0.7f;
            GameObject basicMissile = GameObject.Instantiate(missile, missileSpawnPosition, Quaternion.Euler(new Vector3(0, 0, 180)));
        }
        else
        {
            Vector3 missileSpawnPosition = this.transform.position;
            missileSpawnPosition.y -= 1.7f;
            missileSpawnPosition.x += 0.7f;
            GameObject basicMissile = GameObject.Instantiate(missile, missileSpawnPosition, Quaternion.Euler(new Vector3(0, 0, 180)));
        }
        
    }

   
    //Seta o dano de 5 no modo defensivo e o dano de 10 no modo ataque.
    //O boss tem 100 de vida.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Missile"))
        {
            Destroy(collision.gameObject);
            if (attackMode)
            {
                AudioManager.instance.PlaySound("DamageBossAttack");
                healthBar.value -= 10;
            }
            else
            {
                AudioManager.instance.PlaySound("DamageBossDefense");
                healthBar.value -= 5;
            }
        }
    }

    //Auxilio para o tamanho do raycast.
    private void OnDrawGizmos()
    {
        Vector2 drawVector = this.transform.position;
        drawVector.y -= raySize;
        Gizmos.DrawLine(this.transform.position, drawVector);
    }
}
