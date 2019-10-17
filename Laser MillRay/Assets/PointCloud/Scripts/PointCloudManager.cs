using UnityEngine;
using UnityEngine.VR;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using MyExtensions;
using System.Collections.Generic;

public class PointCloudManager : MonoBehaviour {

    public Transform cameraTransform;
    public Transform playerCenter;
    private static PointCloudManager instance;
    public GameObject canvas;
    public bool gazeTooltip = false;

    public static PointCloudManager Instance
    {
        get
        {
            return instance;
        }
    }
	
	[Header("Resources")]
	public TooltipBox tooltipBox;
	public LoadingBar loadingBar;
    //public StateManager stateManager;

	[Header("Debuging")]
	public bool doDebug = false;
	public Text widthT;
	public Text maxminT;
	public Text midT;
	public Text dirT;
    
    [HideInInspector]
    public string file;

    public Material matVertex;
	[Space(5f)]
	public bool invertYZ = false;

	private bool loaded = false;

	// PointCloud
	private GameObject pointCloud;

	private float maxLenght = 0f;
    
	private float scale = 1f;

    [SerializeField]
    private float pointScale = 0.01f;


    public int numPoints;
	public int numPointGroups;
    private int limitPoints = 65532/4;

	private Vector3[] points;
	private float[] widths;
	private Quaternion[] pointsDir;
	private Color[] colors;

	private Vector3 minValue;
	private int currentTarget;
	Vector3 forDir = Vector3.right;
	Vector3 lookPos = Vector3.zero;

    Vector3 lookDir = Vector3.zero;
	Vector3 screenTouch = Vector3.zero;

    private bool isCalculating = false;

    public  void CalcularEspesor(Vector3 touch, Draggable drag)
    {
        if (!isCalculating)
        {
            isCalculating = true;
            StartCoroutine(CCalcularEspesor(touch, drag));
            isCalculating = false;
        }
    }
    private IEnumerator CCalcularEspesor(Vector3 touch, Draggable drag)
    {

        if (touch.y > Screen.height * 0.38f)
        {
            if (loaded)
            {
                tooltipBox.Hide();


                screenTouch = touch;
                Ray r = cameraTransform.GetComponent<Camera>().ScreenPointToRay(screenTouch);
                lookDir = ((r.origin + r.direction) - cameraTransform.position).normalized;

                if (Vector3.Distance(lookPos, cameraTransform.position) > 0.05f)
                {
                    lookPos = cameraTransform.position;
                    yield return StartCoroutine(OrderPoints());
                }


                //int a = currentTarget;
                currentTarget = BubbleDot(lookDir, 0, numPoints - 1);

                //Debug.Log(currentTarget);

                if (currentTarget >= 0)
                {
                    //a = currentTarget;
                    tooltipBox.Show(widths[(int)pointsDir[currentTarget].w].ToString("0") + "mm", points[(int)pointsDir[currentTarget].w], colors[(int)pointsDir[currentTarget].w]);
                    drag.SetDireccion(points[(int)pointsDir[currentTarget].w]);
                }

            }
        }
    }

	//private bool isLoaded = false;

	void Start ()
    {
        instance = this;
	}

	void Update()
	{
        canvas.SetActive(!loadingBar.IsShowing);

        if (gazeTooltip)
        {
            lookDir = cameraTransform.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)).direction;
            tooltipBox.transform.position = lookDir * 2f + cameraTransform.position;

            if (loaded)
            {
                int a = currentTarget;
                currentTarget = BubbleDot(lookDir, 0, numPoints - 1);

                //Debug.Log(currentTarget);

                if (a != currentTarget)
                {
                    if (currentTarget >= 0)
                    {
                        a = currentTarget;
                        tooltipBox.Show(widths[(int)pointsDir[currentTarget].w].ToString("0") + "mm", points[(int)pointsDir[currentTarget].w], colors[(int)pointsDir[currentTarget].w]);
                    }
                    else
                    {
                        tooltipBox.Hide();
                    }
                }

            }
        }
    }
	
	int BubbleDot(Vector3 dir,int min,int max)
	{

		float dirDot = Vector3.Dot(dir.normalized, forDir);
		float minDot = Vector3.Dot(pointsDir[min].ToVector3().normalized, forDir);
		float maxDot = Vector3.Dot(pointsDir[max].ToVector3().normalized, forDir);


		//float g=(Vector3.Dot (pointsDir [numPoints].ToVector3 ().normalized, forDir) - Vector3.Dot (pointsDir [0].ToVector3 ().normalized, forDir))/numPoints;


		int mid =(min + (max - min) / 2);// min + (((minDot + ((maxDot - minDot) /g);// 


		float midDot = Vector3.Dot(pointsDir[mid].ToVector3().normalized, forDir);


		


		if (Mathf.Abs(maxDot-minDot) < 0.05f) {
            //Debug.Log(max - min);
			return BubbleMag (dir, min, max);
		}

		else 
		{
			if (midDot > dirDot) {
				//Debug.Log (s + " = mayor / " + minDot + " / " + midDot + " / " + maxDot);
				return BubbleDot (dir, min, mid);
			} else {
				//Debug.Log (s + " =  menor (" + min + ")" + minDot + " / (" + mid + ")" + midDot + " / (" + max + ")" + maxDot);
				return BubbleDot (dir, mid, max);
			}
		}
		//return -2;
	}
	int BubbleMag(Vector3 dir,int min, int max)
	{
		int a = (min + (max - min)) / 2;

		float dirDot = Vector3.Dot (dir.normalized, forDir);
		float minDot = Vector3.Dot(pointsDir[min].ToVector3().normalized, forDir);
		float maxDot = Vector3.Dot(pointsDir[max].ToVector3().normalized, forDir);
		float midDot = Vector3.Dot(pointsDir[a].ToVector3().normalized, forDir);

		midT.text = (max-min).ToString();
		maxminT.text = minDot + " / " + midDot + " / " + maxDot;// min + " / " + mid + " / " + max;
		dirT.text = dirDot.ToString();

		for (int i = min; i < max; i++)
		{


			if (Vector3.Dot(pointsDir[i].ToVector3().normalized, dir) > Vector3.Dot(pointsDir[a].ToVector3().normalized, dir))
			{
				a = i;
			}
		}
		
		return a;
	}
	public void LoadFile()
    {
        StartCoroutine(LoadOFF());
	}
	
	// Start Coroutine of reading the points from the OFF file and creating the meshes
    IEnumerator LoadOFF()
    {
		DebugUnity.Log(file);
        StringReader sr = new StringReader(file);
		sr.ReadLine(); // OFF
        string firstLine = sr.ReadLine().Replace("# ","");
        string[] buffer = firstLine.Split(); // nPoints, nFaces
        //Debug.Log("\"" + buffer[0] + "\"");
		numPoints = int.Parse(buffer[0]);
		points = new Vector3[numPoints];
		widths = new float[numPoints];
		pointsDir = new Quaternion[numPoints];
		
		colors = new Color[numPoints];
		minValue = new Vector3();

		for (int i = 0; i < numPoints; i++)
		{
			buffer = sr.ReadLine().Split();

			if (!invertYZ)
				points[i] = new Vector3(float.Parse(buffer[0])-1.9f, float.Parse(buffer[1]), float.Parse(buffer[2]));
			else
				points[i] = new Vector3(float.Parse(buffer[0])-1.9f, float.Parse(buffer[2]), float.Parse(buffer[1]));

			if (buffer.Length >= 6)
				colors[i] = new Color(int.Parse(buffer[3]) / 255.0f, int.Parse(buffer[4]) / 255.0f, int.Parse(buffer[5]) / 255.0f);
			else
				colors[i] = Color.cyan;

			if (points[i].magnitude > maxLenght)
				maxLenght = points[i].magnitude;

			if (buffer.Length >= 7)
				widths[i] = float.Parse(buffer[6]);// / 100f;
			else
				widths[i] = 0f;

			pointsDir[i].w = i;


			// GUI
			if (i % Mathf.FloorToInt(numPoints / 20) == 0)
			{
				loadingBar.Show ("Cargando Puntos",(i*1f) / numPoints);
				yield return null;
			}
		}
		loadingBar.Hide ();

		//Scale and recenter
		yield return StartCoroutine(ReshapePoints());

		yield return StartCoroutine(OrderPoints());

		// Instantiate Point Groups
		numPointGroups = Mathf.CeilToInt (numPoints*1.0f / limitPoints*1.0f);

		pointCloud = new GameObject ("OFF");

		for (int i = 0; i < numPointGroups-1; i ++) {
			InstantiateMesh (i, limitPoints);

			if (i%10==0){
				loadingBar.Show ("Generando Malla",(i*1f) / numPoints);
				yield return null;
			}
		}
		loadingBar.Hide ();

		InstantiateMesh (numPointGroups-1, numPoints- (numPointGroups-1) * limitPoints);
		//pointCloud.transform.LookAt(pointCloud.transform.position + Vector3.up);
		//Store PointCloud
		//UnityEditor.PrefabUtility.CreatePrefab ("Assets/Resources/PointCloudMeshes/" + filename + ".prefab", pointCloud);

        //pointCloud.transform.rotation = Quaternion.Euler(90f, 0f, 0f) * Quaternion.identity;

		loaded = true;
	}

	private IEnumerator ReshapePoints()
	{
		//Scale and recenter points to be more useful

		scale = (1f / maxLenght)*4f;

		for (int i=0;i<numPoints;i++)
		{
			points[i] *= scale;
			points[i].x -= 1.9f;
			points [i] = Quaternion.AngleAxis (-90f, Vector3.right)*points[i];
			if (i % Mathf.FloorToInt(numPoints / 40) == 0)
			{
				loadingBar.Show ("Reacomodando Puntos",(i*1f) / numPoints);
				yield return null;
			}
		}
		loadingBar.Hide ();
	}

	private IEnumerator OrderPoints(bool showProgress = true)
	{
		forDir = Vector3.right;

		SortingPoints.forDir = forDir;

		for (int i = 0; i < points.Length; i++)
		{
                
            Vector3 buff = points[i] - (lookPos);
                

            pointsDir[i] = new Quaternion(buff.x, buff.y, buff.z, i);//pointCloud.transform.position+

            if(showProgress)
            {
                if (i % Mathf.FloorToInt(points.Length / 40) == 0)
                {
                    loadingBar.Show("Calculando Direcciones", (i * 1f) / numPoints);
                    yield return null;
                }
            }
     
			

		}
		loadingBar.Hide ();

		SortingPoints.TopDownMergeSort(pointsDir, new Quaternion[pointsDir.Length], pointsDir.Length);
	}
	void InstantiateMesh(int meshInd, int nPoints)
    {
		// Create Mesh
		GameObject pointGroup = new GameObject ("OFF" + meshInd);
		pointGroup.AddComponent<MeshFilter> ();
		pointGroup.AddComponent<MeshRenderer> ();
		pointGroup.GetComponent<Renderer>().material = matVertex;

		pointGroup.GetComponent<MeshFilter> ().mesh = CreateMesh (meshInd, nPoints);
		pointGroup.transform.parent = pointCloud.transform;
		pointGroup.transform.localScale = Vector3.one;
	}

    Mesh CreateMesh(int id, int nPoints)
    {

        Mesh mesh = new Mesh();

        Vector3[] myPoints = new Vector3[nPoints * 4];
        List<int> myTriangles = new List<int>(0);

        int[] indecies = new int[nPoints];
        Color[] myColors = new Color[nPoints * 4];

        for (int i = 0; i < nPoints; ++i)
        {

            int u = i * 4;
            int trueID = id * limitPoints + i;
            myPoints[u + 0] = new Vector3(points[trueID].x + 0 * pointScale, points[trueID].y + 1 * pointScale, points[trueID].z + 0 * pointScale);
            myPoints[u + 1] = new Vector3(points[trueID].x + 1 * pointScale, points[trueID].y + 0 * pointScale, points[trueID].z + 1 * pointScale);
            myPoints[u + 2] = new Vector3(points[trueID].x + -1 * pointScale, points[trueID].y + 0 * pointScale, points[trueID].z + 1 * pointScale);
            myPoints[u + 3] = new Vector3(points[trueID].x + 0 * pointScale, points[trueID].y + 0 * pointScale, points[trueID].z + -1 * pointScale);

            int[] triangles = {
            u+0, u+2, u+1, //face front
            u+1, u+2, u+3, //face Bot
			u+0, u+1, u+3, //face right
            u+0, u+3, u+2, //face left
            };

            myTriangles.AddRange(triangles);

            //myPoints[i] = points[id*limitPoints + i] - minValue;
            indecies[i] = i;
            for (int a = 0; a < 4; a++)
            {
                myColors[u + a] = colors[trueID];

            }
        }


        mesh.vertices = myPoints;
        mesh.colors = myColors;
        mesh.triangles = myTriangles.ToArray();

        mesh.RecalculateNormals();

        return mesh;
    }
	void calculateMin(Vector3 point)
    {
		if (minValue.magnitude == 0)
			minValue = point;


		if (point.x < minValue.x)
			minValue.x = point.x;
		if (point.y < minValue.y)
			minValue.y = point.y;
		if (point.z < minValue.z)
			minValue.z = point.z;
	}
	public void DestroyPointCloud()
	{
		if (pointCloud != null)
		{
            file = "";
			Destroy(pointCloud);
		}
		
	}


}
