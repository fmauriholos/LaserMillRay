using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListaPadre : MonoBehaviour {

    [SerializeField]
    protected RectTransform content;
    [SerializeField]
    protected GameObject btnModelo;
    //public ListaPadre listaSiguiente;
    //public ListaPadre listaAnterior;
    [SerializeField]
    protected MenuScreen manager;
    private List<GameObject> listaBotones = new List<GameObject>(0);


    protected int myIndex;

    public void CargarLista()
    {
        CargarLista(myIndex);
    }
    public virtual void CargarLista(int id)
    {
        gameObject.SetActive(true);
        myIndex = id;
    }
    
    //public virtual void Atras()
    //{
    //    if (listaAnterior != null)
    //    {
    //        gameObject.SetActive(false);
    //        listaAnterior.CargarLista();
    //        VaciarLista();
    //    }
    //}

    protected ElementoLista AddBoton(string name, int index)
    {
        Vector3 vec = Vector3.zero;
        RectTransform myRect = gameObject.GetComponent<RectTransform>();

        GameObject lastCreated = Instantiate(btnModelo, transform) as GameObject;

        RectTransform recT = lastCreated.GetComponent<RectTransform>();
        Vector2 sizeLC = recT.sizeDelta;

        sizeLC.x = myRect.rect.width;

        //Debug.Log(sizeLC.x);

        recT.sizeDelta = sizeLC;

        vec.y = vec.y - recT.sizeDelta.y * listaBotones.Count;
        recT.localPosition = vec;


        

        lastCreated.SetActive(true);


        ElementoLista el = lastCreated.GetComponent<ElementoLista>();

        el.manager = manager;
        el.Setear(name, index);
        listaBotones.Add(lastCreated);
        Vector2 size = content.sizeDelta;

        size.y = recT.sizeDelta.y * listaBotones.Count;
        content.sizeDelta = size;
        Debug.Log("Size: " + recT.sizeDelta.y + " Y: " + vec.y + " ," + recT.gameObject.name, recT.gameObject);

        return el;
    }

    public virtual void VaciarLista()
    {
        while (listaBotones.Count > 0)
        {
            gameObject.SetActive(false);
            Destroy(listaBotones[0].gameObject);
            listaBotones.RemoveAt(0);
        }
    }
}
