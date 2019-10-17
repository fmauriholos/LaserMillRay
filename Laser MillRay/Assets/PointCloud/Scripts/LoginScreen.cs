using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using UnityEngine.Events;



public class LoginScreen : MonoBehaviour
{

    public InputField usuario;
    public InputField password;
    public Text error;

    public MenuScreen menuScreen;
	void Start()
	{
		usuario.text = PlayerPrefs.GetString("Email");
	}
	void Update ()
	{
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (usuario.isFocused)
            {
                password.Select();
            }
            if (password.isFocused)
            {
                usuario.Select();
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (usuario.text != "" && password.text != "")
            {
                Ingresar();
            }
        }


	}
    public void Ingresar()
    {
        error.gameObject.SetActive(false);
        try
        {
            usuario.interactable = false;
            password.interactable = false;

            StartCoroutine(TI_API.LoginRequest(usuario.text, password.text, LoginSuccess, LoginError));
        }
        catch (ArgumentException)
        {
            error.text = "Error de conexión con el servidor";
            error.gameObject.SetActive(true);

            throw;
        }
        catch (Exception)
        {
            error.text = "Error de conexión con el servidor";
            error.gameObject.SetActive(true);

            throw;
        }
    }

    private void LoginSuccess()
    {
        usuario.interactable = true;
        password.interactable = true;

		PlayerPrefs.SetString("Email", usuario.text);
		Debug.Log(usuario.text + " " + PlayerPrefs.GetString("Email"));

        StartCoroutine(TI_API.RequestManagedCorporations(CallbackCorporation));
    }

    private void LoginError(string msg)
    {
        usuario.interactable = true;
        password.interactable = true;

        if (usuario.text == "" || password.text == "")
        {
            error.text = "Ingrese usuario y contraseña";
            error.gameObject.SetActive(true);
        }
        else
        {
            error.text = msg;
            error.gameObject.SetActive(true);
        }

    }

    public void CallbackCorporation()
    {
        Hide();
        menuScreen.Show();
    }
    

    public void Hide()
    {
        gameObject.SetActive(false);
        error.gameObject.SetActive(false);

    }
    public void Show()
    {
        gameObject.SetActive(true);

		usuario.text = PlayerPrefs.GetString("Email");
		Debug.Log(PlayerPrefs.GetString("Email"));

        password.text = "";
    }
}
