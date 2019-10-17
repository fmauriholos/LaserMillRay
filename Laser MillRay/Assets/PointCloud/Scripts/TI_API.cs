using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Text;
using System;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Runtime.InteropServices;
//using IndieYP;

public enum httpVerb
{
    GET,
    POST,
    PUT,
    DELETE
}



public struct LoginInfo
{
    public string user;
    public string id;
    public string token;
    public string tokenExpires;
    public bool isLoggedIn;

    public LoginInfo(string _user)
    {
        user = _user;
        id = "";
        token = "";
        tokenExpires = "";
        isLoggedIn = false;
    }
}

public delegate void ReturnFile(byte[] file);

public class TI_API
{
	public const string URL = "https://millray.teicser.com";

    [DllImport("__Internal")]
    private static extern void OpenPDF(string url);

	//Current data
	public static LoginInfo currentLogin = new LoginInfo("");
	private static ManagedCorporations managedCorporations;

	//SCOPE IDs
	public static int corporationIndex;
	public static int enterpriseIndex;
	public static int dependencieIndex;
	public static int subdependencieIndex;
	public static int studyIndex;

    public static int CorporationsCount
    {
        get
        {
            if (managedCorporations.corporations != null)
                return managedCorporations.corporations.Length;
            else return 0;
        }
    }
	#region Properties
	public static bool IsCorporationValid
	{
		get
		{
			return (corporationIndex >= 0 && corporationIndex < managedCorporations.corporations.Length);
		}
	}

	public static bool IsEnterpriseValid
	{
		get
		{
			if (IsCorporationValid)
				return (enterpriseIndex >= 0 && enterpriseIndex < managedCorporations.corporations[corporationIndex].enterprises.Length);
			else
				return false;
		}
	}
	public static bool IsDependencieValid
	{
		get
		{
			if (IsEnterpriseValid)
				return (dependencieIndex >= 0 && dependencieIndex < managedCorporations.corporations[corporationIndex].enterprises[enterpriseIndex].dependencies.Length);
			else
				return false;
		}
	}
	public static bool IsSubdependencieValid
	{
		get
		{
			if (IsDependencieValid)
				return (subdependencieIndex >= 0 && subdependencieIndex < managedCorporations.corporations[corporationIndex].enterprises[enterpriseIndex].dependencies[dependencieIndex].subdependencies.Length);
			else
				return false;
		}
	}
	public static bool IsStudyValid
	{
		get
		{
			if (IsSubdependencieValid)
				return (studyIndex >= 0 && studyIndex < managedCorporations.corporations[corporationIndex].enterprises[enterpriseIndex].dependencies[dependencieIndex].subdependencies[subdependencieIndex].studies.Length);
			else
				return false;
		}
	}
	#endregion

    public static void CerrarSesion()
    {
        currentLogin = new LoginInfo("none");
    }

    public static IEnumerator LoginRequest(string _usuario, string _password, Action callbackSuccess, Action<string> callbackError)
    {
        LoginInfo newLogin = new LoginInfo(_usuario);

        if (currentLogin.user != newLogin.user || (currentLogin.user == newLogin.user && !currentLogin.isLoggedIn))
        {
            DebugUnity.Log("Attempting Login for user: " + newLogin.user + "...");


            WWWForm form = new WWWForm();
            form.AddField("email", _usuario);
            form.AddField("password", _password);

            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            UnityWebRequest requestLogin = UnityWebRequest.Post(URL + "/login", form);

            //requestLogin.SetRequestHeader("Access-Control-Allow-Credentials","true");

            yield return requestLogin.SendWebRequest();

            if (requestLogin.isNetworkError || requestLogin.isHttpError)
            {
                string msg = "";
                Debug.LogError("Login error: " + requestLogin.error);

                if (requestLogin.isNetworkError)
                    msg = "Error de conexión con el servidor";
                else
                    msg = "Usuario o contraseña incorrecto";

                if (callbackError != null)
                    callbackError(msg);
            }
            else
            {
				DebugUnity.Log("Login Successful!");
				DebugUnity.Log("Login response: " + requestLogin.downloadHandler.text);
				currentLogin = JsonUtility.FromJson<LoginInfo>(requestLogin.downloadHandler.text);
                currentLogin.isLoggedIn = true;

                if (callbackSuccess != null)
                    callbackSuccess();
            }


        }
    }

    public static IEnumerator RequestManagedCorporations(Action callback)
	{
		string strResponseValue = string.Empty;


		DebugUnity.Log("Requesting managed corporations...");
		ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

		UnityWebRequest corporationRequest = UnityWebRequest.Get(URL + "/search");
		corporationRequest.SetRequestHeader("token", currentLogin.token);
		corporationRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        //corporationRequest.SetRequestHeader("Access-Control-Allow-Credentials","true");

		DebugUnity.Log("Corporations test...");

        yield return corporationRequest.SendWebRequest();

		if (corporationRequest.isNetworkError || corporationRequest.isHttpError)
		{
			Debug.LogError("Managed Corporations error: " + corporationRequest.error);
		}
		else
		{
			DebugUnity.Log("Managed Corporations Received!");

			strResponseValue = corporationRequest.downloadHandler.text;
			DebugUnity.Log("Corporations response:\n" + strResponseValue);


			managedCorporations = JsonUtility.FromJson<ManagedCorporations>(strResponseValue);

			if (DebugUnity.IsEditor)
			{
				string debugData = "Managed Corporations: " + managedCorporations.corporations.Length;

				foreach (Corporation corporation in managedCorporations.corporations)
				{
					debugData += "\n\t- " + corporation.name;
				}
				DebugUnity.Log(debugData);
			}

			if (managedCorporations.corporations.Length > 0)
				corporationIndex = 0;
			else
				corporationIndex = -1;
		}
        callback();
    }


	#region Getters
	public static Corporation GetCorporation()
	{
		if (IsCorporationValid)
			return managedCorporations.corporations[corporationIndex];
		else
			return new Corporation();
	}
	public static Corporation GetCorporation(int index)
	{
		corporationIndex = index;
		enterpriseIndex = 0;
		dependencieIndex = 0;
		subdependencieIndex = 0;
		studyIndex = 0;

		return GetCorporation();
	}
	public static Enterprise GetEnterprise()
	{
		if (IsEnterpriseValid)
			return managedCorporations.corporations[corporationIndex].enterprises[enterpriseIndex];
		else
			return new Enterprise();
	}
	public static Enterprise GetEnterprise(int index)
	{
		enterpriseIndex = index;
		dependencieIndex = 0;
		subdependencieIndex = 0;
		studyIndex = 0;

		return GetEnterprise();
	}

	public static Dependencie GetDependencie()
	{
		if (IsDependencieValid)
			return managedCorporations.corporations[corporationIndex].enterprises[enterpriseIndex].dependencies[dependencieIndex];
		else
			return new Dependencie();
	}
    public static Dependencie GetDependencie(int index)
	{
		dependencieIndex = index;
		subdependencieIndex = 0;
		studyIndex = 0;

		return GetDependencie();
	}
	public static Subdependencie GetSubdependencie()
	{
		if (IsSubdependencieValid)
			return managedCorporations.corporations[corporationIndex].enterprises[enterpriseIndex].dependencies[dependencieIndex].subdependencies[subdependencieIndex];
		else
			return new Subdependencie();
	}
    public static Subdependencie GetSubdependencie(int index)
	{

		subdependencieIndex = index;
		studyIndex = 0;

		return GetSubdependencie();

	}

	public static Study GetStudy()
	{
		if (IsStudyValid)
			return managedCorporations.corporations[corporationIndex].enterprises[enterpriseIndex].dependencies[dependencieIndex].subdependencies[subdependencieIndex].studies[studyIndex];
		else
			return new Study();
	}
    public static Study GetStudy(int index)
    {
		studyIndex = index;

		return GetStudy();
	}
	#endregion




    public static IEnumerator DownloadPDF(Action callback, Action<string> callbackError)
    {
        DebugUnity.Log("Download PDF: " + GetStudy().pdf_url);
        Application.OpenURL(GetStudy().pdf_url);
        callback();
        yield return null;
    }

    public static IEnumerator DownloadOFF(Action callbackSuccess, Action<string> callbackError)
    {

		if (GetStudy().off_url == null)
        {
			Debug.LogError("OFF download url not valid: " + GetStudy().off_url);

			if (callbackError!=null)
				callbackError("OFF download url not valid");
		}
        else
        {
			string downloadUrl = GetStudy().off_url;

			DebugUnity.Log("Start downloading OFF: " + downloadUrl);

			UnityWebRequest requestOff = UnityWebRequest.Get(GetStudy().off_url);

			yield return requestOff.SendWebRequest();

			if (requestOff.isNetworkError || requestOff.isHttpError)
			{
				Debug.LogError("Downloading OFF error: " + requestOff.error);
			}
			else
			{
				DebugUnity.Log("OFF downloaded - " + (requestOff.downloadedBytes / 1024f / 1024f).ToString("0.0") + "MB");
                string fileString = System.Text.Encoding.UTF8.GetString(requestOff.downloadHandler.data);

                PointCloudManager.Instance.file = fileString;
				callbackSuccess();
			}
        }
    }


    public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }
}
