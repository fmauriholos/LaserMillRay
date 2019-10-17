using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListCorporations : ListaPadre {

    public override void CargarLista(int id)
    {
        
        base.CargarLista(id);

        DebugUnity.Log("Load Corporations.");

        for (int i = 0; i < TI_API.CorporationsCount; i++)
        {
            AddBoton(TI_API.GetCorporation(i).name, i);
        }
    }
    
}
