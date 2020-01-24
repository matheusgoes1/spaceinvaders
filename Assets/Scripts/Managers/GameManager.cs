using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject enemyManager, boss;

    [SerializeField]
    Text recordTextUI, inGameScore;

    [SerializeField]
    Color yellowColor;

    public static int winCount;

    public static GameManager instance;

    public int playerScore, linesDestroyedCount;

    static int record;

    //Seta o valor do record salvo.
    //Define qual round vai ocorrer, se a quantidade de rounds vencidos for igual a 2, o boss aparece.
    private void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("record"))
        {
            record = PlayerPrefs.GetInt("record", record);
        }

        recordTextUI.text = record.ToString();

        if (winCount >= 2)
        {
            boss.SetActive(true);
            enemyManager.SetActive(false);
        }
   
    }

  
    void Update()
    {
        TextScoreUpdate();
        HackKeys();
    }

    //Ferramentas do desenvolvedor.
    public void HackKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            enemyManager.SetActive(false);
            boss.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Player player = FindObjectOfType<Player>();
            player.health = 999999999;
        }
    }

    //Adiciona pontos no score
    public void AddPoint(int point)
    {
        playerScore += point;
    }

    //Atualiza o texto in game. Alem disso, muda a cor caso seja um novo record.
    public void TextScoreUpdate()
    {
        inGameScore.text = playerScore.ToString();

        if (playerScore > record)
        {
            record = playerScore;
            PlayerPrefs.SetInt("record",record);
            inGameScore.color = yellowColor;
        }

    }

    //Auxilia no final da fase, o int "linesDestroyedCount" recebe do EnemyManager a confirmaçao de que
    //todas as naves de determinada linha foram destruidas.
    public void CheckEnemyLines(int value)
    {
        linesDestroyedCount += value;
        if (linesDestroyedCount >= 3)
        {
            winCount += 1;
            ScreenManager.instance.WinScreen();
        }
    }
}
