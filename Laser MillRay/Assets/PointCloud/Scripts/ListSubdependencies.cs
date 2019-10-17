using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListSubdependencies : ListaPadre
{

    public override void CargarLista(int id)
    {
        base.CargarLista(id);

        Dependencie dependencie = TI_API.GetDependencie(myIndex);
        DebugUnity.Log("Load Subdependencie: [" + myIndex + "] " + dependencie.name);

        if (dependencie.subdependencies != null)
        {
            for (int i = 0; i < dependencie.subdependencies.Length; i++)
            {
				int a = dependencie.subdependencies[i].studies != null ? dependencie.subdependencies[i].studies.Length : 0;

				AddBoton(dependencie.subdependencies[i].name, i);
            }
        }
    }
}
