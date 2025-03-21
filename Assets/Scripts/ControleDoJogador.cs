using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ControleDoJogador : MonoBehaviour
{
    [Header("Referências Gerais")]
    private Rigidbody2D oRigidbody2D;
    private Animator oAnimator;

    [Header("Movimento do Jogador")]
    [SerializeField] private float velocidadeDoJogador;
    private Vector2 inputDeMovimento;
    private Vector2 direcaoDoMovimento;

    [Header("Controle do Ataque")]
    [SerializeField]private float tempoMaximoEntreAtaques; // tempo de espera entre ataques
    private float tempoAtualEntreAtaques; // tempo passado do ultimo ataque
    private bool podeAtacar; // pode ou não atacar

    private void Start()
    {
        oRigidbody2D = GetComponent<Rigidbody2D>();
        oAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        RodarCronometroDosAtaques();
        ReceberInputs();
        RodarAnimacoesEAtaques();
        EspelharJogador();
        MovimentarJogador();
    }
    
    //Controla o tempo entre os Ataques
    private void RodarCronometroDosAtaques()
    {
        tempoAtualEntreAtaques -= Time.deltaTime;
        if(tempoAtualEntreAtaques <= 0)
        {
            podeAtacar = true;
            tempoAtualEntreAtaques = tempoMaximoEntreAtaques;
        }
    }

    private void ReceberInputs()
    {
        //Armazena a direção que o Jogador quer seguir
        inputDeMovimento = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //Testar dano do Jogador
        if(Input.GetKeyDown(KeyCode.L))
        {
            GetComponent<VidaDoJogador>().LevarDano(1);
        }
    }

    private void RodarAnimacoesEAtaques()
    {   //Rodam a Movimentação do Jogador
        if(inputDeMovimento.magnitude == 0)
        {
            oAnimator.SetTrigger("parado");
        }
        else if(inputDeMovimento.magnitude != 0)
        {
            oAnimator.SetTrigger("andando");
        }
        //Rodam os Ataques do Jogador
        if(Input.GetKeyDown(KeyCode.J) && podeAtacar)
        {
            oAnimator.SetTrigger("socando");
            podeAtacar = false;
        }
        
        if(Input.GetKeyDown(KeyCode.K) && podeAtacar)
        {
            oAnimator.SetTrigger("chutando");
            podeAtacar = false;
        }
    }

    private void EspelharJogador()
    {
        //Faz o jogador olhar para a direção que está andando(direita / esquerda)
        if(inputDeMovimento.x == 1)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if(inputDeMovimento.x == -1)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void MovimentarJogador()
    {
        //Movimenta o Jogador com base na sua direção
        direcaoDoMovimento = inputDeMovimento.normalized;
        oRigidbody2D.linearVelocity = direcaoDoMovimento * velocidadeDoJogador;
    }
}
