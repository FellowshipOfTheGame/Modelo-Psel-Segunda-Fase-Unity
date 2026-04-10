using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    //Script de dano do jogador
    public void ReceberDano(int dano)
    {
        //Como não há um sistema de vida implementado, apenas recarrega a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
