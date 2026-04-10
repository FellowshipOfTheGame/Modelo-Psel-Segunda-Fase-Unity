using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Os botões da UI da Unity chamam funções de um script que estiver em um gameObject
    //Este está no EventSystem da cena MainMenu

    public void Jogar()
    {
        //No modelo do jogo, a cena da fase está na posição 1
        SceneManager.LoadScene(1);
    }

    public void Sair()
    {
        //Fecha o jogo (não funciona no editor, apenas no jogo compilado)
        Application.Quit();
    }
}
