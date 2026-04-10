using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//RequireComponent -> obriga o gameObject a ter, pelo menos, 1 do componente especificado
//Se o jogo for compilado e o objeto não tiver o componente, o Unity vai criar automaticamente
//Também bloqueia o componente de ser removido até que o script também seja
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
//CapsuleCollider2D -> colisor 2D em formato de cápsula
//Colisores de caixa têm dificuldades em lidar com desníveis, travando o personagem ou forçando interações indesejáveis
//O colisor de cápsula resolve isso, pois tem bordas redondas que se adaptam aos pequenos desníveis
//Na fase do jogo está um pequeno desnível no chão para mostrar isso
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerMovementPhysics : MonoBehaviour
{
    //Script de movimento do jogador com física (com rigidbody)

    //SerializeField -> permite definir os valores no editor, mesmo os parâmetros sendo privados

    [SerializeField] private float _velocidade, _forcaPulo, _distanciaChao;
    //LayerMask -> no editor, é possível definir o layer dos gameObjects no canto superior direito do inspetor
    //LayerMask é um conjunto de layers que serão interagidos
    //Ele usa operações bitwise, então prints e comparações não são elementares (não dá pra usar ==, >, <, etc)
    [SerializeField] private LayerMask _layersChao;

    //Rigidbody2D -> rigidbody para jogos 2D
    //O rigidbody é muito útil, pois calcula física automaticamente e gerencia colisões
    private Rigidbody2D _corpo;
    //InputSystem_Actions é uma classe criada pelo Unity.
    //Ele é quem recebe os inputs do jogador e traduz para tipos de movimento
    private InputSystem_Actions _actions;
    private AudioSource _fonteAudio;

    //Quando iniciar o script. Ocorre antes do Start() e do OnEnable()
    private void Awake()
    {
        _actions = new InputSystem_Actions();

        _corpo = GetComponent<Rigidbody2D>();

        _fonteAudio = GetComponent<AudioSource>();
    }

    //Quando o script for ativado
    //Se o Awake() acima fosse Start(), iria dar erro pois _actions seria nulo
    void OnEnable()
    {
        //Se não chamar essa função, não receberá os inputs
        _actions.Enable();
    }

    //Quando o script for desativado
    void OnDisable()
    {
        _actions.Disable();
    }

    //Quando o script for destruído
    void OnDestroy()
    {
        //Destrói o gerenciador de inputs
        _actions.Dispose();
    }

    //OnTriggerEnter2D -> se colidiu com um objeto com um colisor trigger
    //Todo collider pode ser definido como trigger no inspetor
    //Colliders de tipo trigger não interagem com a física
    //No modelo do jogo, a bandeira de finalização da fase é trigger
    void OnTriggerEnter2D(Collider2D collider)
    {
        //Se o objeto colidido tiver a tag "Bandeira"
        //tag != layer, ambos são definidos no inspetor, mas em locais diferenets
        //Canto superior esquerdo e canto superior direito, respectivamente
        if (collider.CompareTag("Bandeira"))
        {
            //Carrega a cena 0 (no modelo, o mainMenu)
            SceneManager.LoadScene(0);
        }
    }

    //Diferentemente do Update(), FixedUpdate() não é chamado todo frame
    //Ele é SEMPRE chamado em intervalos regulares de tempo (padrão de 50 vezes por segundo)
    //Ele é usado para os cálculos de física na Unity, então deve ser usado exclusivamente para tal
    //Neste caso, para a movimentação e o pulo
    void FixedUpdate()
    {

        //Pega a direção de movimento do input e multiplica pela velocidade
        //A direção do input já é normalizada (módulo = 1), então a velocidade é sempre constante
        Vector2 movimento = _actions.Player.Move.ReadValue<Vector2>() * _velocidade;
        //Define a velocidade do jogador
        //IMPORTANTE: se comparar com o PlayerMovementNoPhysics, aqui não está sendo usado o Time.deltaTime
        //Isso é porque o cálculo da física já incorpora isso automaticamente, então usá-lo aqui teria o efeito contrário
        //Quanto mais fps, mais lento ele se moveria
        _corpo.linearVelocity = new Vector2(movimento.x, _corpo.linearVelocity.y);

        //Calcula a posição do pé do jogador (centro da face inferior) 
        Vector2 peJogador = (Vector2)transform.position + Vector2.down * (transform.localScale.y / 2f);
        //Define o tamanho da caixa que será analisada
        Vector2 tamanhoCaixa = new Vector2(transform.localScale.x, _distanciaChao);
        //OverlapBox -> cria uma caixa com os parâmetros: origem, tamanho, ângulo, layers que podem colidir
        //É aqui que o LayerMask é importante. Quanto menos layers pro Raycast analisar, mais otimizado vai ser
        //Qual o objetivo? Detectar se o jogador está no chão, ou seja, se ele pode pular
        Collider2D colisao = Physics2D.OverlapBox(peJogador, tamanhoCaixa, 0, _layersChao);
        //Esta linha está comentada para fins de performance.
        //Ativá-la vai mostrar, no editor, a distância de detecção da caixa
        //Debug.DrawRay(peJogador, Vector2.down * _distanciaChao, Color.red);

        //Se a caixa colidiu com algo (se o jogador está no chão)
        if(colisao)
        {
            //Verifica se o jogador está dando o input para pular
            bool pulo = _actions.Player.Jump.IsPressed();
            //Se sim
            if (pulo)
            {
                //Empurra o rigidBody para cima
                //ForceMode2D.Impulse -> avisa o sistema de física que é uma força instantânea, não contínua
                _corpo.AddForce(new Vector2(0, _forcaPulo), ForceMode2D.Impulse);
                
                //Toca o clipe que estiver no AudioSource
                _fonteAudio.Play();
            }
        }
    }
}
