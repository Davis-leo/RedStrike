using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Comidas : MonoBehaviour
{
    [SerializeField]private int vidaParaDar;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<VidaDoJogador>() != null)
        {
            other.gameObject.GetComponent<VidaDoJogador>().GanharVida(vidaParaDar);
            SoundManager.instance.pegarComida.Play();
            Destroy(this.gameObject);
        }
    }
}
