using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDeLuta : MonoBehaviour
{
    [Header("Verificações")]
    private bool podeVerificarJogador;
    private bool podeSpawnar;

    [Header("Cronômetro do Spawn")]
    [SerializeField]private float tempoMaximoEntreSpawns;
    private float tempoAtualEntreSpawns;

    [Header("Controle do Spawn")]
    [SerializeField]private Transform[] pontosDeSpawn;
    [SerializeField]private GameObject[] InimigosParaSpawnar;
    private int inimigosSpawnados;
    private int inimigoAtual;

    private int inimigosDerrotados;

    private void Start()
    {
        podeVerificarJogador = true;
        podeSpawnar = false;

        inimigosSpawnados = 0;
        inimigoAtual = 0;
        inimigosDerrotados = 0;
    }

    private void Update()
    {
        if (podeSpawnar && inimigosSpawnados < InimigosParaSpawnar.Length)
        {
            RodarCronometroDoSpawn();
        }
    }

    private void SpawnarInimigo()
    {
        //Escolhe um ponto aleatório para spawnar o inimigo
        Transform pontoAleatorio = pontosDeSpawn[Random.Range(0, pontosDeSpawn.Length)];
        GameObject novoInimigo = InimigosParaSpawnar[inimigoAtual];

        //Spawna um novo inimigo no local escolhido
        GameObject inimigoInstanciado = Instantiate(novoInimigo, pontoAleatorio.position, pontoAleatorio.rotation);
        inimigoInstanciado.GetComponent<VidaDoInimigo>().ReceberMinhaAreaDeLuta(this);
        inimigoAtual++;
        inimigosSpawnados++;
    }

    public void IncrementarContagemDeInimigosDerrotados()
    {
        inimigosDerrotados++;
        VerificarSetodosInimigosForamDerrotados();
    }

    private void VerificarSetodosInimigosForamDerrotados()
    {
        if(inimigosDerrotados == InimigosParaSpawnar.Length)
        {
            CameraQueSegue.instance.DestravarCamera();
            UIManager.instance.AtivarPainelDaAreaDeLuta();
            SoundManager.instance.setaDeProssiga.Play();
        }
    }

    private void RodarCronometroDoSpawn()
    {
        //Controla a quantidade de tempo entre os spawns
        tempoAtualEntreSpawns -= Time.deltaTime;
        if(tempoAtualEntreSpawns <= 0)
        {
            SpawnarInimigo();
            tempoAtualEntreSpawns = tempoMaximoEntreSpawns;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(podeVerificarJogador)
        {
            if(other.gameObject.GetComponent<ControleDoJogador>() != null)
            {
                podeSpawnar = true;
                podeVerificarJogador = false;
                CameraQueSegue.instance.TravarCamera();
            }
        }
    }
}
