using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListEnterprises : ListaPadre {

    public override void CargarLista(int id)
    {
        base.CargarLista(id);

        Corporation corporation = TI_API.GetCorporation(myIndex);
        DebugUnity.Log("Load Enterprises: [" + myIndex + "] " + corporation.name);

        if (corporation.enterprises != null)
        {
            for (int i = 0; i < corporation.enterprises.Length; i++)
            {
				AddBoton(corporation.enterprises[i].name, i);
            }
        }
           


    }


}
