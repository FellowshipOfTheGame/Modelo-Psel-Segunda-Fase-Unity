using UnityEngine;

//RequireComponent -> obriga o gameObject a ter, pelo menos, 1 do componente especificado
//Se o jogo for compilado e o objeto não tiver o componente, o Unity vai criar automaticamente
//Também bloqueia o componente de ser removido até que o script também seja
[RequireComponent(typeof(AudioSource))]
public class Disparador : MonoBehaviour
{
    //SerializeField -> permite definir os valores no editor, mesmo os parâmetros sendo privados

    [SerializeField] private GameObject _prefabProjetil, _spawner;
    [SerializeField] private float _intervaloDisparos, _distanciaMinimaJogadorHorizontal;

    private float _ultimoDisparo;
    private AudioSource _fonteAudio;

    //Para a máquina de estados finitos
    private enum Estados
    {
        procurando,
        disparando
    };
    private Estados _estadoAtual;

    //Calcula se o jogador está na área de ataque
    bool JogadorNaArea()
    {
        //Mathf.Abs -> pega o módulo do valor
        return Mathf.Abs(ReferenciaJogador.Jogador.transform.position.x - transform.position.x) <= _distanciaMinimaJogadorHorizontal;
    }

    //Para disparar os projéteis
    void Disparar()
    {
        //Instantiate -> cria o gameobject
        //É uma função pesada! Se for chamado muitas vezes por frame, terá impacto significante
        Instantiate(_prefabProjetil, _spawner.transform);

        //Toca o clipe que estiver no AudioSource
        _fonteAudio.Play();
    }

    //Quando iniciar o script. Ocorre depois de todos os Awake()
    void Start()
    {
        _ultimoDisparo = 0f;
        //Pega o componente especificado do gameObject
        _fonteAudio = gameObject.GetComponent<AudioSource>();

        _estadoAtual = Estados.procurando;
    }

    //Todo frame
    void Update()
    {
        //Máquina de estados finitos
        switch (_estadoAtual)
        {
            case Estados.procurando:
            {
                if (JogadorNaArea())
                    _estadoAtual = Estados.disparando;
                break;
            }

            case Estados.disparando:
            {
                if (Time.time >= _ultimoDisparo + _intervaloDisparos)
                {
                    _ultimoDisparo = Time.time;
                    Disparar();
                }
                if (!JogadorNaArea())
                    _estadoAtual = Estados.procurando;
                break;
            }
        }
    }
}
