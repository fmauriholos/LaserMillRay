using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour,IBeginDragHandler,IDragHandler, IEndDragHandler {

    public PointCloudManager pointCloudManager;
    public Transform bg;
    public GameObject flecha, drag, dragFlecha;
    public Camera camara;
    Vector3 posicionFinal;
    public Vector3 offSet = new Vector3(1, 1, 0);
    bool isInWorld = false;
    public Canvas canvas;
    public static bool isDragged;
    


    void Start()
    {
        RectTransform rTransform= GetComponent<RectTransform>();
        transform.position = bg.position;

        offSet = new Vector3(-rTransform.rect.size.x * canvas.scaleFactor * 0.5f, rTransform.rect.size.y * canvas.scaleFactor * 0.5f);
        

    }
    void OnEnable()
    {
        ClipToBoard();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Grab();

    }

    public void OnDrag(PointerEventData eventData)
    {
        isDragged = true;
       transform.position = (Vector3) eventData.position + offSet;
       
            drag.SetActive(false);
            flecha.SetActive(false);
            dragFlecha.SetActive(true);
            pointCloudManager.CalcularEspesor( transform.position, this);

        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragged = false;
        //transform.position = eventData.position;


        if (eventData.position.y <= Screen.height * .38)
        {
            ClipToBoard();
        }
        else
        {

            ClipToWorld(eventData.position);
        }
    }
    private void Grab()
    {
        isInWorld = false;

        drag.SetActive(false);
        flecha.SetActive(false);
        dragFlecha.SetActive(true);
    }
    private void ClipToBoard()
    {
        isInWorld = false;
        transform.position = bg.position;

        drag.SetActive(true);
        flecha.SetActive(false);
        dragFlecha.SetActive(false);
    }
    private void ClipToWorld(Vector3 pos)
    {
        isInWorld = true;

        pointCloudManager.CalcularEspesor(pos + offSet, this);

        drag.SetActive(false);
        flecha.SetActive(true);
        dragFlecha.SetActive(false);
    }

    public void SetDireccion(Vector3 posicionGlobal)
    {
        posicionFinal = posicionGlobal;
    }

    void Update()
    {
        

        if (isInWorld)
        {
            transform.position = camara.WorldToScreenPoint(posicionFinal);
            
            Vector3 screenPos = camara.WorldToScreenPoint(posicionFinal);
            //Debug.Log("Height: " + Screen.height + " screenPos" + screenPos + "\nWidth: " + Screen.width);
            if ((screenPos.y > Screen.height || screenPos.y < 0) ||
            (screenPos.x > Screen.width || screenPos.x < 0))
            {

                ClipToBoard();
            }
        }
        
    }
}
