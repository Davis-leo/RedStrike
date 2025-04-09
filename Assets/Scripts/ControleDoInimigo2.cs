using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleDoInimigo2 : MonoBehaviour
{

    private Rigidbody2D oRigidbody2D;
    private Animator oAnimator;
    private GameObject oJogador;

    [SerializeField]private float velocidadeDoInimigo;

    private Vector2 direcaoDoMovimento;

    [SerializeField]private float tempoMaximoEntreAtaques;
    private float tempoAtualEntreAtaques;
    private bool podeAtacar;
    [SerializeField]private float distanciaParaAtacar;
    [SerializeField]private int quantidadeDeAtaquesDoInimigo;
    private int ataqueAtualDoInimigo;

    private void Start()
    {
        oRigidbody2D = GetComponent<Rigidbody2D>();
        oAnimator = GetComponent<Animator>();
        oJogador = Object.FindAnyObjectByType<ControleDoJogador>().gameObject;
    }

    private void Update()
    {
        RodarCronometroDosAtaques();
        EspelharInimigo();
        SeguirJogador();
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
        if(oJogador.transform.position.x > transform.position.x)
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
        if(Vector2.Distance(transform.position, oJogador.transform.position) > distanciaParaAtacar)
        {
            direcaoDoMovimento = (oJogador.transform.position - transform.position).normalized;
            oRigidbody2D.linearVelocity = direcaoDoMovimento * velocidadeDoInimigo; 
            oAnimator.SetTrigger("andando");
        }
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
        {
            IniciarAtaque();
        }
    }

    private void IniciarAtaque()
    {
        if(ataqueAtualDoInimigo == 0)
        {
            oAnimator.SetTrigger("socando-fraco");
        }
        else if(ataqueAtualDoInimigo ==1)
        {
            oAnimator.SetTrigger("socando-forte");
        }

        podeAtacar = false;
    }

}
