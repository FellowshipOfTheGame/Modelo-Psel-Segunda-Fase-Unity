using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    //Script para editar valores globais do Unity
    //Este script está no EventSystem do MainMenu

    private int _fpsDesejado = 144;

    //Awake ocorre antes do Start
    void Awake()
    {
        //Desativa o Vsync
        QualitySettings.vSyncCount = 0;
        //Define a taxa de fps desejada
        Application.targetFrameRate = _fpsDesejado;
    }
}
