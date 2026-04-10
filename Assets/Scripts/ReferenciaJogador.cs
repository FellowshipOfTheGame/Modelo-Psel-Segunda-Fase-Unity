using UnityEngine;

public class ReferenciaJogador : MonoBehaviour
{
    //Essa classe serve para ter uma referência global de quem é o jogador
    //Assim, os outros scripts que dependem dele têm uma forma rápida e simples de referênciá-lo
    
    //static -> referência global da classe, não da instância
    //ReferenciaJogador.Jogador
    public static GameObject Jogador;

    //Awake ocorre antes do Start, então todos os scripts que dependerem
    //da referência do jogador terão ele sem problemas no Start()
    void Awake()
    {
        //Se a referência já foi definida, então há múltiplos do mesmo script
        if (Jogador != null)
        {
            Debug.LogError("MÚLTIPLAS REFERÊNCIAS PARA JOGADOR!!!!");
            return;
        }

        Jogador = this.gameObject;
    }

    //Essa função é chamada quando o script for destruído
    void OnDestroy()
    {
        //Se a referência do jogador for o objeto deste script
        if (Jogador == this.gameObject)
            //Limpa a referência
            Jogador = null;

        //Então, em toda troca de cena, Jogador se torna null novamente
    }
}
