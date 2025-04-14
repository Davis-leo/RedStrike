using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraQueSegue : MonoBehaviour
{
    public static CameraQueSegue instance;

    [Header("Referências ao Jogador")]
    private GameObject oJogador;
    private Vector3 posicaoDoJogador;

    [Header("Limites da Movimentação da Câmera")]
    [SerializeField]private float xMinimo;
    [SerializeField]private float xMaximo;

    private bool podeSeguirJogador;
    private bool voltandoParaJogador;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        oJogador = FindObjectOfType<ControleDoJogador>().gameObject;

        podeSeguirJogador = true;
        voltandoParaJogador = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            TravarCamera();
        }else if(Input.GetKeyDown(KeyCode.V))
        {
            DestravarCamera();
        }

        GetPosicaoDoJogador();

        if(podeSeguirJogador)
        {
            SeguirJogador();
        }

        if(voltandoParaJogador)
        {
            VoltarParaJogador();
        }
    }

    private void GetPosicaoDoJogador()
    {
        posicaoDoJogador = oJogador.transform.position;
    }

    [Header("Quando na Área de Luta")]

    [SerializeField]private float velocidadeParaVoltarAoJogador;

    private void SeguirJogador()
    {
        //Deixa sua posição x igual a do jogador, e mantém as posições y e z da câmera
        transform.position = new Vector3(posicaoDoJogador.x, transform.position.y, transform.position.z);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMinimo, xMaximo), transform.position.y, transform.position.z);
    }

    public void TravarCamera()
    {
        podeSeguirJogador = false;
        FindObjectOfType<ControleDoJogador>().AtualizarLimiteXQuandoCameraTravada();
    }

    public void DestravarCamera()
    {
        voltandoParaJogador = true;
        StartCoroutine(VoltarASeguirJogadorCoroutine());
    }

    public void VoltarParaJogador()
    {
        Vector3 posicaoCentralizada = new Vector3(posicaoDoJogador.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, new Vector3(posicaoDoJogador.x, posicaoCentralizada.y, transform.position.z), velocidadeParaVoltarAoJogador * Time.deltaTime);
    }

    private IEnumerator VoltarASeguirJogadorCoroutine()
    {
        FindObjectOfType<ControleDoJogador>().AtualizarLimiteXQuandoCameraDestravada();
        yield return new WaitForSeconds(1f);
        podeSeguirJogador = true;
        voltandoParaJogador = false;
    }
}
