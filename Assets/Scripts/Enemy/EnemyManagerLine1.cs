using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerLine1 : MonoBehaviour
{
    [SerializeField]
    EnemyShip enemyShip;

    [SerializeField]
    GameObject missile;

    [SerializeField]
    public float elapsedTime = 0, shootTime;

    public bool moveRight = true, hasEnemy = true;
    [SerializeField]
    List<EnemyShip> shipsList = new List<EnemyShip>();

    //EnemyManagerLine2 e EnemyManagerLine3 herdam todo o codigo de EnemyManager.
    //Foi optado por ter um manager para cada linha para evitar repetiçao de codigo e porque as naves
    //tem pequenas diferenças no comportamento.
    //No Start, a linha de x tipo de nave é gerada e adicionada em uma lista.
    //A lista facilita o controle uniforme da linha de naves.
    void Start()
    {
        if (GameManager.winCount >= 2)
        {
            hasEnemy = false;
        }
        else
        {
            EnemyGenerator();
        }  
    }


    void Update()
    {
        if (hasEnemy)
        {
            CheckDestroyedShips();
            EnemyController();    
            EnemyShoot();
        }     
    }

    //Gera uma fileira de naves a partir de determinado prefab.
    public void EnemyGenerator()
    {
        for (int i = 0; i < 8; i++)
        {
            EnemyShip enemy = EnemyShip.Instantiate(this.enemyShip, this.transform.position, Quaternion.identity);
            enemy.transform.parent = transform;
            shipsList.Add(enemy);
        }
    }

    //EnemyController: define o comportamento da fileira de naves.
    //moveRight tem o mesmo valor que em enemyShip, em que determina para que lado a nave vai andar.
    //checkRaycast, recebe um valor da primeira nave  -quando o movimento da linha é para a esquerda (moveRight =false)
    //e da ultima nave - quando o movimento da linha é para a direita (moveRight = true).
    //Alem disso, checkRaycast determina o movimento da fileira de naves baseado no valor de retorno do metodo CheckMoviment.
    //CheckMoviment retorna se algum raycast encostou em algo. Por exemplo, se encostar na direita, todas a naves andam para
    //esquerda, vice versa.
    void EnemyController()
    {
        int checkRaycast;
        if (moveRight)
        {
            checkRaycast = shipsList[shipsList.Count - 1].CheckMovement();

            if (checkRaycast == 1)
            {
                for (int i = 0; i < shipsList.Count; i++)
                {
                    shipsList[i].moveRight = false;
                }
                moveRight = false;
            }
     
        }

        else
        {
            checkRaycast = shipsList[0].CheckMovement();
            if (checkRaycast == 2)
            {
                for (int i = 0; i < shipsList.Count; i++)
                {
                    shipsList[i].moveRight = true;
                }
                moveRight = true;
            }

        }

    }


    //Escolhe aleatoriamente uma nave da fileira para lançar um missel, num tempo pre definido.
    //A velocidade de lançamento dos misseis sao diferentes para cada linha.
    void EnemyShoot()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > shootTime)
        {
            int r = Random.Range(0, shipsList.Count);
            shipsList[r].Shoot(missile);
            elapsedTime = 0;
        }
    }

    //Primeiramente as naves abatidas tem apenas seu gameObject desativado.
    //A fim de excluir o lixo de memoria, esse metodo checa quais naves estao desativadas
    //e entao, de fato, exclui a nave da lista e a destroi.
    //Esse metodo foi necessario para a correçao de um erro em que se a nave da extremidade (a que o raycast estava
    //sendo testado) fosse destruida, exibia um erro de NullReference e bugava a movimentaçao de toda fileira.
    void CheckDestroyedShips()
    {
        if (shipsList.Count > 0)
        {
            for (int i = 0; i < shipsList.Count; i++)
            {
                if (!shipsList[i].gameObject.activeSelf)
                {
                    shipsList[i].DestroyShip();
                    shipsList.Remove(shipsList[i]);
                }
            }
        }
        else
        {
            Debug.Log("hasnoenemy");
            hasEnemy = false;
            GameManager.instance.CheckEnemyLines(1);
        }
        
    }

}
