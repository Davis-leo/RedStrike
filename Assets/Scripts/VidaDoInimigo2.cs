using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaDoInimigo2 : MonoBehaviour
{
    private bool inimigoVivo;
    [SerializeField]private int vidaMaxima;
    private int vidaAtual;
    
    private void Start()
    {
        inimigoVivo = true;
    }

    public void LevarDano(int danoParaReceber)
    {
        if(inimigoVivo)
        {
            vidaAtual -= danoParaReceber;
            if(vidaAtual <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
