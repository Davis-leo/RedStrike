using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleDoInimigo : MonoBehaviour
{
    [Header("Referencias Gerais")]
    private Rigidbody2D oRigidbody2D;
    private Animator oAnimator;
    private GameObject oJogador;

    [Header("Movimento do Inimigo")]
    [SerializeField] private float velocidadeDoInimigo;
    [SerializeField] private float distanciaMinima; // Adicione esta linha - distância mínima para não colar no jogador
    private Vector2 direcaoDoMovimento;

    [Header("Controle do Ataque")]
    [SerializeField] private float tempoMaximoEntreAtaques;
    private float tempoAtualEntreAtaques;
    private bool podeAtacar;
    [SerializeField] private float distanciaParaAtacar;
    [SerializeField] private int quantidadeDeAtaquesDoInimigo;
    private int ataqueAtualDoInimigo;

    private void Start()
    {
        oRigidbody2D = GetComponent<Rigidbody2D>();
        oAnimator = GetComponent<Animator>();
        oJogador = Object.FindAnyObjectByType<ControleDoJogador>().gameObject;
        
        // Inicializa a distância mínima se não foi configurada
        if(distanciaMinima <= 0) distanciaMinima = 0.5f;
    }

    private void Update()
    {
        if(GetComponent<VidaDoInimigo>().inimigoVivo)
        {
            RodarCronometroDosAtaques();
            EspelharInimigo();
            SeguirJogador();
        }
        else
        {
            RodarAnimacaoDeDerrota();
        }
    }

    private void RodarCronometroDosAtaques()
    {
        tempoAtualEntreAtaques -= Time.deltaTime;
        if(tempoAtualEntreAtaques <= 0)
        {
            podeAtacar = true;
            tempoAtualEntreAtaques = tempoMaximoEntreAtaques;
        }
    }

    private void EspelharInimigo()
    {
        if (oJogador.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(oJogador.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }   

    private void SeguirJogador()
    {
        float distanciaAtual = Vector2.Distance(transform.position, oJogador.transform.position);
        
        // Se estiver longe, segue normalmente
        if(distanciaAtual > distanciaParaAtacar)
        {
            direcaoDoMovimento = (oJogador.transform.position - transform.position).normalized;
            oRigidbody2D.linearVelocity = direcaoDoMovimento * velocidadeDoInimigo;
            oAnimator.SetTrigger("andando");
        }
        // Se estiver muito perto, afasta um pouco mantendo a distância mínima
        else if(distanciaAtual < distanciaMinima)
        {
            direcaoDoMovimento = (transform.position - oJogador.transform.position).normalized;
            oRigidbody2D.linearVelocity = direcaoDoMovimento * velocidadeDoInimigo;
            oAnimator.SetTrigger("andando");
        }
        // Se estiver na distância correta para atacar, para e ataca
        else
        {
            oRigidbody2D.linearVelocity = Vector2.zero;
            oAnimator.SetTrigger("parado");
            SortearAtaque();
        }
    }

    private void SortearAtaque()
    {
        ataqueAtualDoInimigo = Random.Range(0, quantidadeDeAtaquesDoInimigo);

        if(podeAtacar)
            IniciarAtaque();
    }

    private void IniciarAtaque()
    {
        if(ataqueAtualDoInimigo == 0)
        {
            oAnimator.SetTrigger("socando-fraco");
        }
        else if(ataqueAtualDoInimigo == 1)
        {
            oAnimator.SetTrigger("socando-forte");
        }

        podeAtacar = false;
    }

    public void RodarAnimacaoDeDano()
    {
        oAnimator.SetTrigger("levando-dano");
    }

    public void RodarAnimacaoDeDerrota()
    {
        oAnimator.Play("inimigo-derrotado");
        oRigidbody2D.linearVelocity = Vector2.zero;
    }
}