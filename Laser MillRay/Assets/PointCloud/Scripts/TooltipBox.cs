using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipBox : MonoBehaviour {
	[SerializeField]
	private Text infoText;
	[SerializeField]
	private Image bgImg;
	[SerializeField]
	private Camera targetCamera;
	[SerializeField]
	private bool startShowing = false;

	private Vector3 lastPosTarget = Vector3.zero;
	private Vector3 lastPosMe = Vector3.zero;
	private bool isShowing = false;

   [Header("Escalar")]
    public float objectScale = 1.0f;
    private Vector3 initialScale;

    private void Start()
	{
        initialScale = transform.localScale;

        UpdateFacing();

		isShowing = !startShowing;
		if (startShowing)
			Show ();
		else
			Hide ();

        // record initial scale, use this as a basis

        // if no specific camera, grab the default camera
        if (targetCamera == null)
            targetCamera = Camera.main;
    }
	private void Update()
	{
		if (isShowing)
		{
            

            if (targetCamera.transform.position != lastPosTarget || transform.position != lastPosMe)
			{
				UpdateFacing ();
			}
            transform.LookAt(transform.position - targetCamera.transform.rotation * Vector3.forward,
            targetCamera.transform.rotation * Vector3.up);
        }
        
    }

	private void UpdateFacing()
	{
		transform.LookAt (targetCamera.transform);
		lastPosTarget = targetCamera.transform.position;
		lastPosMe = transform.position;


        Plane plane = new Plane(targetCamera.transform.forward, targetCamera.transform.position);
        float dist = plane.GetDistanceToPoint(transform.position);
        transform.localScale = initialScale * dist * objectScale;
        
    }
	
	public void Hide ()
	{
		if (isShowing)
		{
			gameObject.SetActive (false);
			isShowing = false;
		}

	}
	
	// Update is called once per frame
	public void Show ()
	{
		if (!isShowing)
		{
			gameObject.SetActive (true);
			isShowing = true;
		}
	}
	public void Show(string info)
	{
		if (infoText != null)
		{
			infoText.text = info;
		}
		Show ();
	}
	public void Show(string info, Vector3 pos)
	{
		transform.position = pos;
		Show (info);
	}
	public void Show(string info, Vector3 pos,Color col)
	{
		bgImg.color = col;
		Show (info,pos);
	}
}
