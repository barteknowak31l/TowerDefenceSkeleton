using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomer : MonoBehaviour
{
    public Camera camera; // Przypisz kamer� w edytorze
    private Coroutine changeCoroutine;

    public float zoomLvl1;
    public float zoomLvl2;
    public float zoomLvl3;
    public float duration;


    // Funkcja uruchamiaj�ca zmian� rozmiaru kamery
    public void ChangeCameraSize(int zoomLvl)
    {
        // Je�li ju� trwa zmiana, zatrzymaj j�
        if (changeCoroutine != null)
        {
            StopCoroutine(changeCoroutine);
        }

        // Rozpocznij now� zmian�
        changeCoroutine = StartCoroutine(ChangeCameraSizeCoroutine(zoomLvl));
    }

    // Korutyna do p�ynnej zmiany rozmiaru kamery
    private IEnumerator ChangeCameraSizeCoroutine(int lvl)
    {

        float startSize = zoomLvl1;
        float endSize = zoomLvl2;

        if (lvl == 1)
        {
            startSize = zoomLvl1;
            endSize = zoomLvl2;
        }
        else if (lvl == 2)
        {
            startSize = zoomLvl2;
            endSize = zoomLvl3;
        }


        float timeElapsed = 0f;
        camera.orthographicSize = startSize;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration);
            camera.orthographicSize = Mathf.Lerp(startSize, endSize, t);
            yield return null;
        }

        // Ustaw ko�cowy rozmiar na pewno na koniec
        camera.orthographicSize = endSize;
    }
}
