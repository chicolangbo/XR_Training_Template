using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
public class AreaCreater : EditorWindow
{
    public Vector3[] vertMove;
    public Vector3[] Vertices;
    public Vector2[] UV;
    public int[] Triangles;
    float meshSize = 0.5f;
    Mesh mesh;

    GameObject pivotController;

    public GameObject centerOnj;
    GameObject centerPos;
    public Material baseMat;
    public GameObject heightGizmo;
    public GameObject widthGimo;

    GameObject widthPivotCont;

    List<GameObject> heightPivotContList;

    Vector2 grid;

    List<Mesh> meshs = new List<Mesh>();
    List<MeshData> meshDatas = new List<MeshData>();

    List<List<MeshData>> meshDataList = new List<List<MeshData>>();

    List<Material> materialList = new List<Material>();

    [MenuItem("INVENTIS/Area Creater")]
    public static void ShowWindow()
    {
        var window = (AreaCreater)GetWindow(typeof(AreaCreater));
        window.Show();
    }

    private void OnEnable()
    {
        GetAlphabetMaterials();
        GetPrefabObjects();
    }

    void GetPrefabObjects()
    {
        centerOnj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/VR_Training/Tools/AreaCreater/Prefab/Center.prefab", typeof(GameObject));
        heightGizmo = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/VR_Training/Tools/AreaCreater/Prefab/Pivot_Height.prefab", typeof(GameObject));
        widthGimo = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/VR_Training/Tools/AreaCreater/Prefab/Pivot_Width.prefab", typeof(GameObject));
        baseMat = (Material)AssetDatabase.LoadAssetAtPath("Assets/VR_Training/Tools/AreaCreater/Marterial/Back.mat", typeof(Material));
  
    }

    private void OnGUI()
    {
        grid = EditorGUILayout.Vector2Field("Grid", grid);
        centerOnj = (GameObject)EditorGUILayout.ObjectField("CENTER OBJ",centerOnj, typeof(GameObject));
        heightGizmo = (GameObject)EditorGUILayout.ObjectField("HEIGHT GIZMO", heightGizmo, typeof(GameObject));
        widthGimo = (GameObject)EditorGUILayout.ObjectField("WEIGHT GIZMO", widthGimo, typeof(GameObject));
        baseMat = (Material)EditorGUILayout.ObjectField("BASE MATERIAL", baseMat, typeof(Material));

        widthPivotCont = (GameObject)EditorGUILayout.ObjectField("width controller", widthPivotCont, typeof(GameObject));
        if(heightPivotContList != null)
        {

            for (int i = 0; i < heightPivotContList.Count; i++)
            {
                heightPivotContList[i] = (GameObject)EditorGUILayout.ObjectField("height "+ i+" controller", heightPivotContList[i], typeof(GameObject));

            }
        }


        if (GUILayout.Button("CreateMesh"))
        {
            //CreateMesh();
            CreateMeshs();

        }

        if (GUILayout.Button("Clear"))
        {
            for (int x = 0; x < meshDataList.Count; x++)
            {

                var meshList = meshDataList[x];

                for (int y = 0; y < meshList.Count; y++)
                {
                    DestroyImmediate(meshList[y].centerObj);
                    DestroyImmediate(meshList[y].meshObj);
                }

            }
            meshDataList.Clear();
            meshDataList = null;

            DestroyImmediate(widthPivotCont);
            widthPivotCont = null;
        }
        if(materialList.Count > 0)
        {
            foreach(var t in materialList)
                GUILayout.Label(t.name);
        }

        if(UV != null)
        {
            for (int i = 0; i < UV.Length; i++)
            {
                UV[i] = EditorGUILayout.Vector2Field("", UV[i]);
            }
        }
    }

    void GetAlphabetMaterials()
    {
        materialList = new List<Material>();

        var assetPath = Application.dataPath;
        var matPath = Path.Combine(assetPath, "VR_Training/Tools/AreaCreater/AlphabetMat");
        DirectoryInfo dir = new DirectoryInfo(matPath);
        var files = dir.GetFiles();
        List<string> matsPath = new List<string>();

        foreach (var f in files)
        {
            if (!f.FullName.Contains("meta"))
            {
                var path = f.FullName.Split(new string[] { "Assets" }, System.StringSplitOptions.None)[1];
                matsPath.Add("Assets" + path);
            }
        }

        foreach (var path in matsPath)
        {
            Material mat = (Material)AssetDatabase.LoadAssetAtPath(path, typeof(Material));
            materialList.Add(mat);
        }

    }


    void CreateMeshs()
    {
        heightPivotContList = new List<GameObject>();
        MeshSetup();
        int matIndex = 0;
        for (int x = 0; x < (int)grid.x; x++)
        {
            // Y LIST
            List<MeshData> dataList = new List<MeshData>();
            
            for (int y = 0; y < (int)grid.y; y++)
            {
                var meshData = new MeshData();
                var obj = GetMeshObject(y);
                obj.transform.position = new Vector3(x, 0, -y);

                var localToWorld = obj.transform.localToWorldMatrix;
                //var worldToLoacl = obj.transform.worldToLocalMatrix;

                meshData.index = y;
                meshData.mesh = obj.GetComponent<MeshFilter>().sharedMesh;

                //mesh obj
                meshData.meshObj = obj;

                // center obj
                var worldPos = localToWorld.MultiplyPoint3x4(meshData.mesh.bounds.center);
                meshData.centerObj = Instantiate(centerOnj);
                meshData.centerObj.transform.position = worldPos;
                meshData.centerObj.GetComponent<MeshRenderer>().material = materialList[matIndex];

                // whdth Gizmo
                if (x == 0 && y == 0)
                {
                    widthPivotCont = Instantiate(widthGimo);
                    widthPivotCont.name = "widthPivotCont";
                    widthPivotCont.transform.position = mesh.vertices[0];
                }

                // height Gizmo
                if(x <= 0)
                {
                    meshData.pivotCont = new GameObject("heightPivotCont " + y);
                    var worldVertixPos = localToWorld.MultiplyPoint3x4(mesh.vertices[3]);
                    meshData.pivotCont.transform.position = worldVertixPos;
                    meshData.pivotCont.transform.SetParent(obj.transform);

                    var gizmoPrefb = Instantiate(heightGizmo, Vector3.zero, Quaternion.identity, meshData.pivotCont.transform);
                    heightPivotContList.Add(meshData.pivotCont);
                    gizmoPrefb.transform.localPosition = Vector3.zero;
                }

                meshs.Add(obj.GetComponent<MeshFilter>().sharedMesh);
                dataList.Add(meshData);
                matIndex++;
            }

            meshDataList.Add(dataList);

        }

       
    }


    GameObject GetMeshObject(int index)
    {
        GameObject obj = new GameObject("Mesh_" + index);

        Mesh _mesh = new Mesh();
        obj.AddComponent<MeshFilter>().mesh = _mesh;
        _mesh.vertices = Vertices;
        _mesh.triangles = Triangles;
        _mesh.uv = UV;
        _mesh.Optimize();
        _mesh.RecalculateNormals();

        mesh = _mesh;

        obj.AddComponent<MeshRenderer>();
        obj.GetComponent<MeshRenderer>().material = baseMat;

        return obj;
    }

    void CreateMesh()
    {
        GameObject obj = new GameObject("mesh");

        MeshSetup();
        Mesh _mesh = new Mesh();
        obj.AddComponent<MeshFilter>().mesh = _mesh;
        _mesh.vertices = Vertices;
        _mesh.triangles = Triangles;
        _mesh.uv = UV;
        _mesh.Optimize();
        _mesh.RecalculateNormals();

        mesh = _mesh;

        obj.AddComponent<MeshRenderer>();
        obj.GetComponent<MeshRenderer>().material = baseMat;

        var posObj = obj.GetComponent<MeshFilter>().sharedMesh;
        
        var pos = posObj.bounds.center;

        centerPos = Instantiate(centerOnj, pos,Quaternion.identity);
  

        pivotController = new GameObject("pivot");
        pivotController.transform.position = posObj.vertices[3];
        heightGizmo = Instantiate(heightGizmo, Vector3.zero, Quaternion.identity, pivotController.transform);
        heightGizmo.transform.localPosition = Vector3.zero;
    }

    float y0 = 0.5f;
    float y1 = 2.0f;
    

    enum PivotType
    {
        Top,Bottom,Left,Right
    }

    void SetCenterObjPosition()
    {

        for (int x = 0; x < meshDataList.Count; x++)
        {

            var meshList = meshDataList[x];

            for (int y = 0; y < meshList.Count; y++)
            {
                var localToWorld = meshList[y].meshObj.transform.localToWorldMatrix;
                meshList[y].centerObj.transform.position = localToWorld.MultiplyPoint3x4(meshList[y].mesh.bounds.center);
            }

        }

    }
    void ResizeHeightMesh(int meshIdx, int pivotContIdx, PivotType  pivotType) 
    {

        for (int x = 0; x < meshDataList.Count; x++)
        {

            var mesh = meshDataList[x][meshIdx].mesh;
            var cont = meshDataList[0][pivotContIdx].pivotCont;

            var vertMove = mesh.vertices;
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                switch (pivotType)
                {
                    case PivotType.Bottom:
                        if (i == 0 || i == 1) vertMove[i] = mesh.vertices[i];
                        else vertMove[i] = new Vector3(mesh.vertices[i].x, mesh.vertices[i].y, cont.transform.localPosition.z);
                        break;
                    case PivotType.Top:
                        if (i == 2 || i == 3) vertMove[i] = mesh.vertices[i];
                        else vertMove[i] = new Vector3(mesh.vertices[i].x, mesh.vertices[i].y, cont.transform.localPosition.z + 1);
                        break;
                }
            }

            mesh.vertices = vertMove;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

        }
    }

    void ResizeWidthMesh(  PivotType pivotType)
    {

        var xIndex = pivotType == PivotType.Left ? 0 : meshDataList.Count - 1;
        var meshDatas = meshDataList[xIndex];

        for (int i = 0; i < meshDatas.Count; i++)
        {
            var _mesh = meshDatas[i].mesh;
            var _vertMove = _mesh.vertices;

            for (int j = 0; j < _mesh.vertices.Length; j++)
            {
                switch (pivotType)
                {
                    case PivotType.Left:
                        if (j == 1 || j == 2) _vertMove[j] = _mesh.vertices[j];
                        else _vertMove[j] = new Vector3(widthPivotCont.transform.localPosition.x, _mesh.vertices[j].y, _mesh.vertices[j].z);
                        break;
                    case PivotType.Right:
                        if (j == 0 || j == 3) _vertMove[j] = _mesh.vertices[j];
                        else _vertMove[j] = new Vector3(0+ Mathf.Abs( widthPivotCont.transform.localPosition.x)  , _mesh.vertices[j].y, _mesh.vertices[j].z);
                        break;
                }
            }
            
            if(pivotType == PivotType.Left)
                meshDatas[i].pivotCont.transform.localPosition = new Vector3(widthPivotCont.transform.localPosition.x, meshDatas[i].pivotCont.transform.localPosition.y, meshDatas[i].pivotCont.transform.localPosition.z);

            _mesh.vertices = _vertMove;
            _mesh.RecalculateBounds();
            _mesh.RecalculateNormals();
        }
    }


    private void Update()
    {
        if(meshDataList != null && meshDataList.Count > 0)
        {

            for (int i = 0; i < meshDataList[0].Count; i++)
            {
                ResizeHeightMesh(i, i, PivotType.Bottom);
                if(i < meshDataList[0].Count-1)
                    ResizeHeightMesh(i+1, i, PivotType.Top);

                ResizeWidthMesh(PivotType.Left);
                ResizeWidthMesh(PivotType.Right);
                SetCenterObjPosition();
            }

            /*
            ResizeHeightMesh(0, 0, PivotType.Bottom);
            ResizeHeightMesh(1, 0, PivotType.Top);

            ResizeHeightMesh(1, 1, PivotType.Bottom);
            ResizeHeightMesh(2, 1, PivotType.Top);

            ResizeHeightMesh(2, 2, PivotType.Bottom);
            */
        }

        if(pivotController!= null && centerPos != null)
        {
            vertMove = mesh.vertices;
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                if (i == 0 || i == 1) vertMove[i] = mesh.vertices[i];
                else vertMove[i] = new Vector3(mesh.vertices[i].x , mesh.vertices[i].y, pivotController.transform.position.z);
            }

            //var uv_y = pivotController.transform.position.z;
            //y0 = (0.5f + uv_y) * -1f;
            //y1 = 1.5f + uv_y;

            //for (int i = 0; i < UV.Length; i++)
            //{
            //    if (i == 0 || i ==3)
            //        UV[i] = new Vector2(UV[i].x, y0);
            //    else
            //        UV[i] = new Vector2(UV[i].x, y1);
            //}

          //  Debug.Log(y0 + " " + y1);

            mesh.vertices = vertMove;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.uv = UV;
            centerPos.transform.position = mesh.bounds.center;
        }
        

    }


    void MeshSetup()
    {
        // 시계방향으로 위치 설정 (사이즈 : 가로 1 세로 1 )
        Vertices = new Vector3[] { new Vector3(-meshSize, 0, meshSize), new Vector3(meshSize, 0, meshSize), new Vector3(meshSize, 0, -meshSize), new Vector3(-meshSize, 0, -meshSize) };
        //Vertices = new Vector3[] { new Vector3(-meshSize, meshSize ,0 ), new Vector3(meshSize, meshSize, 0), new Vector3(meshSize, -meshSize, 0 ), new Vector3(-meshSize, -meshSize, 0) };
        UV = new Vector2[] { new Vector2(0, 0),new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
        // 삼각 인덱스
        Triangles = new int[] { 0, 1, 2, 0, 2, 3 };

    }
}

[System.Serializable]
public class MeshData
{
    public Mesh mesh;
    public GameObject meshObj;
    public GameObject pivotCont;
    public GameObject centerObj;
    public int index;
}
