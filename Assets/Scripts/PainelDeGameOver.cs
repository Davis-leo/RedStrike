using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PainelDeGameOver : MonoBehaviour
{
    [SerializeField]private string nomeDoMenu;

    private void Start()
    {
        SoundManager.instance.TocarMusicaDeGameOver();
    }
    
    private void Update()
    {
        ReceberInputs();
    }

    private void ReceberInputs()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ReiniciarPartida();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            VoltarAoMenu();
        }
    }

    private void ReiniciarPartida()
    {
        SceneManager.LoadScene("Fase01");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SoundManager.instance.TocarMusicaDeFundo();
    }

    private void VoltarAoMenu()
    {
        SceneManager.LoadScene(nomeDoMenu);
    }
}
