using System;


[Serializable]
public struct ManagedCorporations
{
	public Corporation[] corporations;

	public ManagedCorporations(Corporation[] _corporations)
	{
		corporations = _corporations;
	}
}


[Serializable]
public struct Corporation
{
	public string _id;
	public string name;
    //public string[] adminsEmails;
	public Enterprise[] enterprises;

	public Corporation(string id, string _name)
	{
		_id = id;
		name = _name;
        //adminsEmails = _adminsEmails;
		enterprises = null; // To be filled by parser
	}
}
[Serializable]
public struct Enterprise
{
	public string _id;
	public string name;
    //public string[] adminsEmails;
	public string corporationId;
	public Dependencie[] dependencies;

	public Enterprise(string id, string _name, string _corporationId)
	{
		_id = id;
		name = _name;
        //adminsEmails = _email;
		corporationId = _corporationId;
		dependencies = null; // To be filled by parser
	}
}
[Serializable]
public struct Dependencie
{
	public string _id;
	public string name;
    //public string[] adminsEmails;
	public string enterpriseId;
	public Subdependencie[] subdependencies;

	public Dependencie(string id, string _name, string _enterpriseId)
	{
		_id = id;
		name = _name;
		subdependencies = null; // To be filled by parser
        //adminsEmails = _email;
		enterpriseId = _enterpriseId;

	}
}
[Serializable]
public struct Subdependencie
{
	public string _id;
	public string name;
	public Study[] studies; // To be filled by parser
	//public string[] adminsEmails;
	public string dependencieId;
	public ImageJ[] images;

	public Subdependencie(string id, string _name, string _dependencieId, ImageJ[] img)
	{
		_id = id;
		name = _name;
		studies = null;
		//adminsEmails = _email;
		dependencieId = _dependencieId;
		images = img;
	}
}
[Serializable]
public struct ImageJ
{
	public string name;
	public string imageId;
	public string imageUrl;

	public ImageJ(string n, string i, string u)
	{
		name = n;
		imageId = i;
		imageUrl = u;
	}
}

[Serializable]
public struct Study
{
	public string off_id;
    public string name;
	public string date;
	public string pdf_id;
	public string off_url;
	public string pdf_url;

    public Study(string _off_url, string _name, string pdfid, string _date, string offid, string _pdf_url)
	{
		off_id = offid;
        name = _name;
		date = _date;
        pdf_id = pdfid;
        off_url = _off_url;
		pdf_url = _pdf_url;
	}
}

