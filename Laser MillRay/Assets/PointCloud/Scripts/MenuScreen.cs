using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
    [SerializeField]
    private LoginScreen loginScreen;
    [SerializeField]
    private GameObject atras;
    [SerializeField]
    private GameObject corporationExplorer;
    [SerializeField]
    private List<ListaPadre> listas = new List<ListaPadre>(0);

    

    [SerializeField]
    private ElegirEntregable entregables;
    private int listaIndex = 0;

    private bool isEntregable = false;
    public void Show()
    {
        listaIndex = 0;
        isEntregable = false;

        foreach(ListaPadre lista in listas)
        {
            lista.VaciarLista();
        }
        listas[listaIndex].CargarLista(0);
        gameObject.SetActive(true);
        corporationExplorer.SetActive(true);
        entregables.Hide();
        atras.SetActive(false);
    }

    public void Hide()
    {
        foreach (ListaPadre lista in listas)
        {
            lista.VaciarLista();
        }
        gameObject.SetActive(false);
    }

    public void Siguiente(int index)
    {
         if (listaIndex == listas.Count-1 && !isEntregable)
        {
            isEntregable = true;
            listas[listaIndex].VaciarLista();

            entregables.Show(index);

            corporationExplorer.SetActive(false);
        }
        else if(!isEntregable)
        {
            listas[listaIndex].VaciarLista();
            listaIndex = Mathf.Clamp(listaIndex + 1, 0, listas.Count);
            listas[listaIndex].CargarLista(index);
            if (listaIndex > 0)
                atras.SetActive(true);
        }
    }

    public void Anterior()
    {
        if (isEntregable)
        {
            isEntregable = false;
            entregables.Hide();
            corporationExplorer.SetActive(true);

            listas[listaIndex].CargarLista();
        }
        else
        {
            listas[listaIndex].VaciarLista();
            listaIndex = Mathf.Clamp(listaIndex - 1, 0, listas.Count);
            listas[listaIndex].CargarLista();
            if (listaIndex == 0)
                atras.SetActive(false);
        }

    }
    public void CerrarSesion()
    {
        TI_API.CerrarSesion();

        Hide();
        loginScreen.Show();
    }
}
