using UnityEngine;

public class Projetil : MonoBehaviour
{
    //SerializeField -> permite definir os valores no editor, mesmo os parâmetros sendo privados

    [SerializeField] private float _velocidade;
    [SerializeField] private int _dano;
    //LayerMask -> no editor, é possível definir o layer dos gameObjects no canto superior direito do inspetor
    //LayerMask é um conjunto de layers que serão interagidos
    //Ele usa operações bitwise, então prints e comparações não são elementares (não dá pra usar ==, >, <, etc)
    [SerializeField] private LayerMask _layersColisoes;
    
    //Diferentemente do Update(), FixedUpdate() não é chamado todo frame
    //Ele é SEMPRE chamado em intervalos regulares de tempo (padrão de 50 vezes por segundo)
    //Ele é usado para os cálculos de física na Unity, então deve ser usado exclusivamente para tal
    //Neste caso, para movimentação
    void FixedUpdate()
    {
        //Quanto o projétil vai se deslocar nesta iteração
        float distanciaFrame = _velocidade * Time.fixedDeltaTime;

        //Raycast -> cria um raio com os parâmetros: origem, direção, tamanho, layers que podem colidir
        //É aqui que o LayerMask é importante. Quanto menos layers pro Raycast analisar, mais otimizado vai ser
        RaycastHit2D colisao = Physics2D.Raycast(transform.position, transform.right, distanciaFrame, _layersColisoes);

        //Se colidiu com algo
        if (colisao.collider != null)
        {
            //Move o projétil para a posição da colisão
            transform.position = colisao.point;
            //Tenta pegar o componente PlayerLife do objeto colidido
            //Se não for o jogador (não tiver o PlayerLife), só sai do if
            if (colisao.collider.gameObject.TryGetComponent<PlayerLife>(out PlayerLife vidaJogador))
            {
                //Se conseguiu pegar o componente, causa dano ao jogador
                vidaJogador.ReceberDano(_dano);
            }

            //Destrói o projétil
            //Destruição NÃO É INSTANTÂNEA, ela só vai ocorrer no final do frame, junto com todos os outros Destroy()
            Destroy(this.gameObject);
        }
        //Se não colidiu
        else
            //Move o projétil
            transform.position += transform.right * distanciaFrame;

        //Por que não usar o rigidbody?
        //Rigidbodies são muito úteis para cálculos de física, colisão, etc
        //Mas são consideravelmente mais pesados do que este script
        //Para projéteis, que tem um comportamento simples e muitos existem simultâneamente, essa otimização é essencial
        //Além disso, o Raycast é mais preciso para detectar colisões, 
        //então podem-se usar velocidades maiores para os projéteis
    }

    //Quando o objeto sair da tela
    //IMPORTANTE: tem que sair da tela tanto do jogo quanto do editor
    //Então é normal o projétil continuar existindo no editor, mesmo se sair da área da câmera
    void OnBecameInvisible()
    {
        //Destrói o projétil
        //Destruição NÃO É INSTANTÂNEA, ela só vai ocorrer no final do frame, junto com todos os outros Destroy()
        Destroy(this.gameObject);
    }
}
