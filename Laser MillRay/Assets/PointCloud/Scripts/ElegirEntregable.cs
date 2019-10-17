
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ElegirEntregable : MonoBehaviour {

    [Header("Screens")]
    [SerializeField]
    private MenuScreen menuScreen;
    [SerializeField]
    private ViewerScreen stateManager;

    [Header("Elementos")]
    [SerializeField]
    private Text txtTitle;
    public GameObject txtNoScale;

    [Space(5)]
    [SerializeField]
    private GameObject descargandoContainer;
    [SerializeField]
    private GameObject buttonContainer;

    

    [Header("Botones")]
    [SerializeField]
    private Button btn3D;
    [SerializeField]
    private Button btnVR;
    [SerializeField]
    private Button btnPDF;



    public void Show(int id)
    {
        txtTitle.text = TI_API.GetStudy(id).name + " - " + TI_API.GetStudy(id).date;
        btn3D.interactable = TI_API.GetStudy().off_url != null;
        btnVR.interactable = TI_API.GetStudy().off_url != null;
        btnPDF.interactable = TI_API.GetStudy().pdf_url != null;
        descargandoContainer.SetActive(false);
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);

        buttonContainer.SetActive(true);
        descargandoContainer.SetActive(false);
    }

    private void StartDownload()
    {
        descargandoContainer.SetActive(true);
        buttonContainer.SetActive(false);
    }
    private void StopDownload()
    {
        descargandoContainer.SetActive(false);
        buttonContainer.SetActive(true);
    }


    public void Btn3D()
    {
        StartDownload();
        StartCoroutine(TI_API.DownloadOFF(OffSuccess3D, ErrorCallback));
    }
    public void BtnVR()
    {
        StartDownload();
        StartCoroutine(TI_API.DownloadOFF(OffSuccessVR, ErrorCallback));
    }
    public void BtnPDF()
    {
        StartDownload();
        
        StartCoroutine(TI_API.DownloadPDF(PdfSuccess, ErrorCallback));
    }

    //CALLBACKS
    private void PdfSuccess()
    {
        //PDFReader.OpenDocRemote(TI_API.GetStudy().pdf_url);
        //PDFReader.OpenDocRemote(TI_API.GetStudy().pdf_url, false);
        StopDownload();
        
    }
    private void OffSuccess3D()
    {
        menuScreen.Hide();
        StopDownload();
        stateManager.Show(Estado.noVR);
        PointCloudManager.Instance.LoadFile();
    }
    private void OffSuccessVR()
    {
        menuScreen.Hide();
        StopDownload();
        stateManager.Show(Estado.VR);
        PointCloudManager.Instance.LoadFile();
    }

    private void ErrorCallback(string msg)
    {
        StopDownload();
    }

    public void CheckScale()
    {
        string url = null;
        if (TI_API.GetSubdependencie().images != null)
        {
            url = TI_API.GetSubdependencie().images[0].imageUrl;
        }

        if (url == null)
        {
            txtNoScale.SetActive(true);
        }
        else
        {
            txtNoScale.SetActive(false);
        }
    }
}
