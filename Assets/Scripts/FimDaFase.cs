using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FimDaFase : MonoBehaviour
{
    [SerializeField]private float tempoParaEscurecer;
    [SerializeField]private float tempoParaCarregarNovaFase;

    [SerializeField]private string nomeDaProximaFase;

    private IEnumerator EscurecerTela()
    {
        // Espera um segundo para escurecer a tela
        yield return new WaitForSeconds(tempoParaEscurecer);
        UIManager.instance.EscurecerImagemDeTransicao();

        //Espera x segundos para carregar a nova fase
        yield return new WaitForSeconds(tempoParaCarregarNovaFase);
        SceneManager.LoadScene(nomeDaProximaFase);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Se o jogador colidir, verifica se não há inimigos na fase. Então carrega a próxima fase.
        if(other.gameObject.GetComponent<ControleDoJogador>() != null)
        {
            ControleDoInimigo[] inimigosNaFase = FindObjectsOfType<ControleDoInimigo>();
            
            if(inimigosNaFase.Length == 0)
            {
                StartCoroutine(EscurecerTela());
            }
        }

    }
    
}
