using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListStudy : ListaPadre
{
    public ElegirEntregable elegirEntregable;


    void Start()
    {

    }

    public override void CargarLista(int id)
    {
        base.CargarLista(id);

        Subdependencie subdependencie = TI_API.GetSubdependencie(myIndex);
        DebugUnity.Log("Load Studies: [" + myIndex + "] " + subdependencie.name);

        if (subdependencie.studies != null)
        {
            for (int i = 0; i < subdependencie.studies.Length; i++)
            {
				ElementoLista el = AddBoton(subdependencie.studies[i].name + " " + subdependencie.studies[i].date + (!string.IsNullOrEmpty(subdependencie.studies[i].off_url) ? " (Scan)" : "") + (!string.IsNullOrEmpty(subdependencie.studies[i].pdf_url) ? " (PDF)" : ""), i);
                
                el.elegirEntregable = elegirEntregable;
            }
        }
    }
}
