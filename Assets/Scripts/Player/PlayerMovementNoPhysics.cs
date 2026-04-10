using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementNoPhysics : MonoBehaviour
{
    //Script de movimento do jogador sem física (sem rigidbody)

    //SerializeField -> permite definir os valores no editor, mesmo os parâmetros sendo privados

    [SerializeField] private float _velocidade;
    //InputSystem_Actions é uma classe criada pelo Unity.
    //Ele é quem recebe os inputs do jogador e traduz para tipos de movimento
    private InputSystem_Actions _actions;

    //Quando iniciar o script. Ocorre antes do Start() e do OnEnable()
    private void Awake()
    {
        _actions = new InputSystem_Actions();
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

    //Todo frame
    void Update()
    {
        //Pega a direção de movimento do input e multiplica pela velocidade
        //A direção do input já é normalizada (módulo = 1), então a velocidade é sempre constante
        Vector2 movimento = _actions.Player.Move.ReadValue<Vector2>() * _velocidade;
        //Move o jogador
        //Time.deltaTime -> tempo entre frames
        //Sem o Time.deltaTime, o jogador se movimentaria mais rápido quanto mais frames o jogo tivesse
        this.transform.position += new Vector3(movimento.x, movimento.y, 0) * Time.deltaTime;
    }
}
