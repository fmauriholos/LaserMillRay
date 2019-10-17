using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour {

	[SerializeField]
	private Text infoText;
	[SerializeField]
	private Slider sliderBar;

	private bool isShowing = false;
    public bool IsShowing
    {
        get{
            return isShowing;
        }
    }


    private void Start()
	{
		isShowing = true;
		Hide ();
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
	public void Show(float completion)
	{
		completion = Mathf.Clamp01 (completion);
		sliderBar.value = completion;
		Show ();
	}
	public void Show(string info, float completion)
	{
		Show (completion);
		Show (info);
	}
}
