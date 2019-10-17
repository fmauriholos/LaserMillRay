using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementoLista : MonoBehaviour {
    public Text txtBtn;
    string path;
    int id;
	ViewerScreen stateManager;
    public MenuScreen manager;
    public ElegirEntregable elegirEntregable;




    public string Setear(string nombre, string ruta,ViewerScreen _stateManager)
    {

        txtBtn.text = nombre;
        path = ruta;
		stateManager = _stateManager;
        Debug.Log(ruta);
        return ruta;
    }

    public void Setear (string nombre, int _id)
    {
        txtBtn.text = nombre;
        id = _id;
        
    }
    
    public void ElegirLista()
    {
        manager.Siguiente(id);
    }

    public void MostrarEntregables()
    {
        manager.Siguiente(id);
        elegirEntregable.CheckScale();
    }


}
