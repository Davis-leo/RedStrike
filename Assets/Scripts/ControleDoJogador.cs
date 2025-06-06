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

    [Header("Limites da Movimentação")]
    [SerializeField]private float xMinimoOriginal;
    [SerializeField]private float xMaximoOriginal;

    private float xMinimoAtual;
    private float xMaximoAtual;
    [SerializeField]private float yMinimo;
    [SerializeField]private float yMaximo;

    [Header("Controle do Ataque")]
    [SerializeField]private float tempoMaximoEntreAtaques; // tempo de espera entre ataques
    private float tempoAtualEntreAtaques; // tempo passado do ultimo ataque
    private bool podeAtacar; // pode ou não atacar
    private bool estaAtacando;

    [Header("Controle do Dano")]
    [SerializeField]private float tempoMaximoDoDano;
    private float tempoAtualDoDano;
    private bool levouDano;
    
    private void Start()
    {
        oRigidbody2D = GetComponent<Rigidbody2D>();
        oAnimator = GetComponent<Animator>();

        podeAtacar = true;
        estaAtacando = false;
        tempoAtualDoDano = tempoMaximoDoDano;

        xMinimoAtual = xMinimoOriginal;
        xMaximoAtual = xMaximoOriginal;
    }

    private void Update()
    {
        if(GetComponent<VidaDoJogador>().jogadorVivo){

            if(estaAtacando)
            {
                RodarCronometroDosAtaques();
            }

            if(!levouDano)
            {
                ReceberInputs();
                RodarAnimacoesEAtaques();
                EspelharJogador();
                MovimentarJogador();
            }
            else
            {
                RodarCronometroDoDano();
            }

        }
        else
        {
            RodarAnimacaoDeDerrota();
        }
    }
    
    //Controla o tempo entre os Ataques
    private void RodarCronometroDosAtaques()
    {
        tempoAtualEntreAtaques -= Time.deltaTime;
        if(tempoAtualEntreAtaques <= 0)
        {
            podeAtacar = true;
            estaAtacando = false;
            tempoAtualEntreAtaques = tempoMaximoEntreAtaques;
        }
    }

    //Controla o congelar do Jogador ao levar dano
    private void RodarCronometroDoDano()
    {
        tempoAtualDoDano -= Time.deltaTime;
        RodarAnimacaoDeDano();

        if(tempoAtualDoDano <= 0)
        {
            levouDano = false;
            tempoAtualDoDano = tempoMaximoDoDano;
        }
    }

    private void ReceberInputs()
    {
        //Armazena a direção que o Jogador quer seguir
        inputDeMovimento = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //Testar dano do Jogador
       /*if(Input.GetKeyDown(KeyCode.L))
        {
            RodarAnimacaoDeDano();
        }*/
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
            estaAtacando = true;
            SoundManager.instance.impactoSoco.Play();
        }
        
        if(Input.GetKeyDown(KeyCode.K) && podeAtacar)
        {
            oAnimator.SetTrigger("chutando");
            podeAtacar = false;
            estaAtacando = true;
            SoundManager.instance.impactoChute.Play();
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

    public void AtualizarLimiteXQuandoCameraTravada()
    {
        xMinimoAtual = transform.position.x - 8.0f;
        xMaximoAtual = transform.position.x + 8.0f;
    }

    public void AtualizarLimiteXQuandoCameraDestravada()
    {
        //Atualiza os limites de movimentação do Jogador
        xMinimoAtual = xMinimoOriginal;
        xMaximoAtual = xMaximoOriginal;
    }

    private void MovimentarJogador()
    {
        if(!estaAtacando)
        {
            //Movimenta o Jogador com base na sua direção
            direcaoDoMovimento = inputDeMovimento.normalized;
            oRigidbody2D.linearVelocity = direcaoDoMovimento * velocidadeDoJogador;

            oRigidbody2D.position = new Vector2(Mathf.Clamp(oRigidbody2D.position.x, xMinimoAtual, xMaximoAtual), oRigidbody2D.position.y);
            oRigidbody2D.position = new Vector2(oRigidbody2D.position.x, Mathf.Clamp(oRigidbody2D.position.y, yMinimo, yMaximo));
        }
        else
        {
            oRigidbody2D.linearVelocity = Vector2.zero;
        }    
    }

    public void RodarAnimacaoDeDano()
    {
        //Roda a animação de dano e zerar a velocidade do Jogador
        oAnimator.SetTrigger("levando-dano");
        levouDano = true;

        oRigidbody2D.linearVelocity = Vector2.zero;
    }

    public void RodarAnimacaoDeDerrota()
    {
        oAnimator.Play("jogador-derrotado");
    }
}
