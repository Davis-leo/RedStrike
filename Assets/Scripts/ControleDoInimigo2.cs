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

    [SerializeField]private float distanciaParaAtacar;

    private void Start()
    {
        oRigidbody2D = GetComponent<Rigidbody2D>();
        oAnimator = GetComponent<Animator>();
        oJogador = Object.FindAnyObjectByType<ControleDoJogador>().gameObject;
    }

    private void Update()
    {
        EspelharInimigo();
        SeguirJogador();
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
        }
    }

}
