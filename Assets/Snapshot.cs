using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Snapshot : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject go;
    public Canvas canvas;
    public Camera cam;
    void Start()
    {
        DoSnapshot(go, canvas, cam);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void DoSnapshot(GameObject go, Canvas canvas, Camera cam)
    {
        var ins = GameObject.Instantiate(go, canvas.transform, false);
 
        ins.SetActive(true);
 
        string fileName = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(go)) + ".png";
        Debug.Log(fileName);
        string astPath = "Assets/Prefabs/snapshots/" + fileName;
        fileName = Application.dataPath + "/Prefabs/snapshots/" + fileName;
        FileInfo info = new FileInfo(fileName);
        if (info.Exists)
            File.Delete(fileName);
        else if (!info.Directory.Exists)
            info.Directory.Create();
     
        var renderTarget = RenderTexture.GetTemporary(1920, 1080);
        cam.aspect = 1920.0f/1080f;
        cam.orthographic = true;
        cam.targetTexture = renderTarget;
        cam.Render();
 
        RenderTexture.active = renderTarget;
        Texture2D tex = new Texture2D(renderTarget.width, renderTarget.height);
        tex.ReadPixels(new Rect(0, 0, renderTarget.width, renderTarget.height), 0, 0);
 
        File.WriteAllBytes(fileName, tex.EncodeToPNG());
 
        cam.targetTexture = null;
        Object.DestroyImmediate(ins);
    }
}
