using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileManager : MonoBehaviour {

    public ListaPadre lista;
    public GameObject btn;

    
    // Use this for initialization
    void Start() {
        
    // BuscarArchivos();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BuscarArchivos()
    {

#if UNITY_ANDROID
        List<string> modelosL = new List<string>(0);
        modelosL.Add(Application.streamingAssetsPath + "/Test.off");
        modelosL.Add(Application.streamingAssetsPath + "/TestW.off");
        string[] modelos = modelosL.ToArray();
#else
        string[] modelos = Directory.GetFiles(Application.streamingAssetsPath, "*.off");
        foreach (string m in modelos)
        {
            Debug.Log(m);

        }
#endif
        Debug.Log(modelos.Length);

 //       lista.CargarLista();
    }





}
