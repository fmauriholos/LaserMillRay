using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using UnityEngine.Networking;

public enum Estado {noVR, VR}


public class ViewerScreen : MonoBehaviour {
    [HideInInspector]
	public Estado estado = Estado.noVR;
    [Header("Player Camera")]
    public Camera cam;

    [Header("Screens")]
    public MenuScreen menuScreen;

    [Header("Point cloud")]
	public PointCloudManager pointCloudManager;
    public TooltipBox tooltipBox;

    [Header("VR Switch")]

    [Space(5)]
    public Button btnSwitch;
	public Image btnSwitchVR;
	public Color activeColor = Color.green;
	public Color inactiveColor = Color.white;
    public GameObject txtSalirVR;

	[Space(5)]
	public CameraControl cameraControl;
	public GameObject novrParent;
	public Image scaleView;

    public void Show(Estado _estado)
    {
        gameObject.SetActive(true);
        CambiarEstado(_estado);
		StartCoroutine(LoadImage());

    }
	private IEnumerator LoadImage()
	{
        string url = null;
        if (TI_API.GetSubdependencie().images != null)
        {
            url = TI_API.GetSubdependencie().images[0].imageUrl;
        }

        if (url != null)
        {
            //WWWForm form = new WWWForm();
            //form.AddField("email", _usuario);
            //form.AddField("password", _password);
            UnityWebRequest img = UnityWebRequestTexture.GetTexture(url.ToString());
            
            //WWW img = new WWW(url);
            Debug.Log("Downloading Image");
            Debug.Log("URL: " + url.ToString());
            yield return img.Send();

            if (img.isNetworkError || img.isHttpError)
            {
                string msg = "";
                Debug.LogError("Download error: " + img.error);

                if (img.isNetworkError)
                    msg = "Error de conexión con el servidor";
                
            }
            else
            {
                DebugUnity.Log("Scale Download Successful!");
                DebugUnity.Log("Scale response: " + img.downloadHandler.text);
                
            }


            Debug.Log(img.downloadedBytes);
            Debug.Log(img.downloadedBytes.ToString());
            //Texture a = new Texture();

            DownloadHandlerTexture handler = (DownloadHandlerTexture)img.downloadHandler;
            Sprite sprt = Sprite.Create(handler.texture, new Rect(0, 0, handler.texture.width, handler.texture.height), new Vector2(0.5f, 0.5f));
            scaleView.sprite = sprt;
            scaleView.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No image attached");
            scaleView.gameObject.SetActive(false);
            yield return null;
        }

    }
    public void Hide()
    {
        gameObject.SetActive(false);
        pointCloudManager.DestroyPointCloud();
        tooltipBox.Hide();
    }

    public void CambiarEstado(Estado _estado)
	{
		estado = (Estado) _estado;
        Debug.Log("Cambiar Estado: " + estado);
		switch (estado)
		{
			case Estado.noVR:
                //vrParent.SetActive(false);
				novrParent.SetActive(true);
                cameraControl.enabled = true;

                btnSwitchVR.gameObject.SetActive(true);
                btnSwitch.gameObject.SetActive(false);
                txtSalirVR.SetActive(false);

                PointCloudManager.Instance.gazeTooltip = false;

				btnSwitchVR.color = inactiveColor;
                
                StartCoroutine(SwitchOutOfVr());
                
                break;
			case Estado.VR:
                //vrParent.SetActive(true);
                btnSwitchVR.gameObject.SetActive(false);
                btnSwitch.gameObject.SetActive(true);
                txtSalirVR.SetActive(true);
                StartCoroutine(InstructionOff());

				novrParent.SetActive(false);
                cameraControl.enabled = false;

                PointCloudManager.Instance.gazeTooltip = true;

				btnSwitchVR.color = activeColor;

                StartCoroutine(SwitchToVR());
                break;
			default:
				break;
		}
	}
	public void SwitchVR()
	{
		switch (estado)
		{
			case Estado.noVR:
				CambiarEstado(Estado.VR);
				break;
			case Estado.VR:
				CambiarEstado(Estado.noVR); 
				break;
			default:
				break;
		}
	}

    public void Atras()
    {
        menuScreen.Show();
        UnityEngine.XR.XRSettings.LoadDeviceByName("");
        Hide();
    }


    IEnumerator SwitchToVR()
    {
        UnityEngine.XR.XRSettings.LoadDeviceByName("cardboard"); // Or "cardboard" (both lowercase).

        // Wait one frame!
        yield return null;

        // Now it's ok to enable VR mode.
        UnityEngine.XR.XRSettings.enabled = true;
    }

    IEnumerator SwitchOutOfVr()
    {
        UnityEngine.XR.XRSettings.LoadDeviceByName(""); // Empty string loads the "None" device.

        // Wait one frame!
        yield return null;

        UnityEngine.XR.XRSettings.enabled = false;
        cam.fieldOfView = 60f;
        cam.rect = new Rect(0f, 0f, 1f, 1f);
    }

    IEnumerator InstructionOff()
    {
        yield return new WaitForSeconds(4f);
        txtSalirVR.SetActive(false);
    }


}
