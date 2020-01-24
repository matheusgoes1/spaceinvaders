using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSounds[] sounds;

    public static AudioManager instance;

    //Com o auxilio da classe AudioSounds que nao é derivada do MonoBehaviour, é feio uma lista de sons em que
    //é serialized para o inspector, facilitando o ajuste de volume, pitch e setar o loop de cada source.
    //Atraves do singleton os metodos para tocar e parar a musica podem ser chamados sem necessariamente cada
    //gameObject da hierarquia precisar possuir o componente AudioSource.
    void Awake()
    {
        instance = this;
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].source = gameObject.AddComponent<AudioSource>();
            sounds[i].source.clip = sounds[i].clip;
            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.pitch = sounds[i].pitch;
            sounds[i].source.loop = sounds[i].loop;
        }
    }


   
    //Procura o nome do som em um lista que possui todos os sons e executa ele.
    public void PlaySound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if(sounds[i].name == name)
            {
                sounds[i].source.Play();
            }
        }
    }

    //Procura o nome do som em um lista que possui todos os sons e para a execuçao dele.
    public void StopSound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].source.Stop();
            }
        }
    }
}
