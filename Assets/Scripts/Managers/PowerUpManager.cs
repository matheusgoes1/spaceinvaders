using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;
    public static string powerUpName;

    [SerializeField]
    GameObject shield;

    Player player;

    [SerializeField]
    Image powerUpIcon;

    [SerializeField]
    Sprite uiVelocity, uiNull, uiShield;
    private void Awake()
    {
        instance = this;
        player = FindObjectOfType<Player>();
        WhichIsActive();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ActivePowerUp();
        }
    }

    //Atraves da variavel estatica, checa qual power up esta ativo.
    void WhichIsActive()
    {
        if (powerUpName == null)
        {
            powerUpIcon.sprite = uiNull;
        }

        else if (powerUpName == "Velocity")
        {
            powerUpIcon.sprite = uiVelocity;
        }

        else if (powerUpName == "Shield")
        {

            powerUpIcon.sprite = uiShield;
        }
    }

    //Ativa o power up.
    void ActivePowerUp()
    {
        if(powerUpName == null)
        {

        }

        else if(powerUpName == "Velocity")
        {
            player.velocity = 7;
        }

        else if(powerUpName == "Shield")
        {
            GameObject shield = GameObject.Instantiate(this.shield, player.transform.position, Quaternion.identity);
            shield.transform.parent = player.transform;
            LTDescr tween;
            tween = LeanTween.scale(shield, Vector3.one, 0.5f);
            tween.setFrom(0);
            tween.setEase(LeanTweenType.easeOutQuad);
        }

        CleanPowerUp();
    }

    //Limpa o espaço do power up, apos o uso.
    private void CleanPowerUp()
    {
        powerUpName = null;
        powerUpIcon.sprite = uiNull;
    }


    //Metodos dos botoes para a escolha do power up.
    public void ShieldButton()
    {
        powerUpName = "Shield";
        powerUpIcon.sprite = uiShield;
        GameObject auxGameObject = EventSystem.current.currentSelectedGameObject;
        auxGameObject.gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void SpeedUpButton()
    {
        powerUpName = "Velocity";
        powerUpIcon.sprite = uiVelocity;
        GameObject auxGameObject = EventSystem.current.currentSelectedGameObject;
        auxGameObject.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
