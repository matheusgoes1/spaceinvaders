using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ScreenManager : MonoBehaviour
{
    //Conjunto de telas do jogo, alem da referencia para o enemyManager e boss (usados nas ferramentas
    //de desenvolvedor no metodo Update).
    [SerializeField]
    GameObject outGameScreen, initialMenu, instructionMenu, storeMenu, hackMenu, creditMenu;

    [SerializeField]
    GameObject inGameScreen, pauseScreen, winScreen, loseScreen, game;

    static int scoreSave;

    static bool restart = false, roundWon;
    
    public static ScreenManager instance;


    //É setado o singleton e coordena quando é apenas resetar o jogo ou a apariçao do menu principal.
    //Tambem define o define o score do jogador caso ele vença o round, para que o ele continue com a pontuaçao
    //do round anterior.
    private void Awake()
    {
        
        if (restart)
        {
            instance = this;
            Time.timeScale = 1f;

            if (roundWon)
            {
              GameManager.instance.playerScore  = scoreSave;
            }

            PlayButton();
        }
        else
        {
            instance = this;
            Time.timeScale = 1f;
            game.SetActive(false);
            inGameScreen.SetActive(false);
            AudioManager.instance.PlaySound("MenuTheme");
        }
    }

    

    //Coloca o esc como uma opçao para a tela de pausa in game.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenuButton();
        }
      
    }

    //A maioria dos metodos abaixo sao usados por botoes em varias 
    //instancias e neles sao definidos as transiçoes de cenas.
    public void PlayButton()
    {
        AudioManager.instance.StopSound("MenuTheme");
        game.SetActive(true);
        outGameScreen.SetActive(false);
        inGameScreen.SetActive(true);
    }

    public void InstructionsButton()
    {
        instructionMenu.SetActive(true);
    }

    public void StoreButton()
    {
        storeMenu.SetActive(true);
    }

    public void HackButton()
    {
        hackMenu.SetActive(true);
    }

    public void CreditsButton()
    {
        creditMenu.SetActive(true);
    }

    //auxGameObject recebe a referencia para o botao que esta usando esse metodo.
    //Em seguida, é acessado o pai desse botao, para poder desativar a tela atual.
    public void BackButton()
    {
        GameObject auxGameObject = EventSystem.current.currentSelectedGameObject;
        auxGameObject.gameObject.transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    //Volta para o menu principal, atraves do loadScene.
    public void BackToMenuButton()
    {
        restart = false;
        Time.timeScale = 1f;
        GameManager.winCount = 0;
        SceneManager.LoadScene("SampleScene");
    }

    public void PauseMenuButton()
    {
        if (game.activeSelf)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    //É usado tanto para resetar o progresso do jogador em caso dele perder a fase
    //como para avançar seu progresso no jogo.
    //O bool roundWon serve de auxilio para o scoreSave salvar o valor do score atual do jogador para que ele continue
    //no proximo round. Isso so acontece se o jogador vencer o round, caso perca, o score volta a ser 0.
    public void RestartButton()
    {
        if (loseScreen.activeSelf)
        {
            Time.timeScale = 1f;
            restart = true;
            roundWon = false;
            GameManager.winCount = 0;
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            scoreSave = GameManager.instance.playerScore;
            roundWon = true;
            Time.timeScale = 1f;
            restart = true;
            SceneManager.LoadScene("SampleScene");
        }
    }
    public void ResumeButton()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LoseScreen()
    {
        AudioManager.instance.PlaySound("PlayerDeath");
        Time.timeScale = 0f;
        loseScreen.SetActive(true);
    }

   
    public void WinScreen()
    {
        Time.timeScale = 0f;
        winScreen.SetActive(true);
    }

    public void MouseClickSound()
    {
        AudioManager.instance.PlaySound("MouseClick");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
