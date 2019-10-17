using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListDependencies : ListaPadre {
    public override void CargarLista(int id)
    {
        base.CargarLista(id);

        Enterprise enterprise = TI_API.GetEnterprise(myIndex);
        DebugUnity.Log("Load dependencie: [" + myIndex + "] " + enterprise.name);

        if (enterprise.dependencies != null)
        {
            for (int i = 0; i < enterprise.dependencies.Length; i++)
            {
				AddBoton(enterprise.dependencies[i].name, i);
            }
        }
    }
}
