using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Efeitos Sonoros")]
    public AudioSource impactoChute;
    public AudioSource impactoSoco;
    public AudioSource pegarComida;
    public AudioSource inimigoLevandoDano;
    public AudioSource jogadoLevandoDano;

    [Header("Músicas")]
    [SerializeField]private AudioSource musicaDeFundo;
    [SerializeField]private AudioSource musicaDeGameOver;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        TocarMusicaDeFundo();
    }

    public void TocarMusicaDeFundo()
    {
        musicaDeGameOver.Stop();
        musicaDeFundo.Play();
    }

    public void TocarMusicaDeGameOver()
    {
        musicaDeFundo.Stop();
        musicaDeGameOver.Play();
    }
}
