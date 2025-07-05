using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways]
public class GameObjectToTexture : MonoBehaviour
{
    public Camera TargetCamera;
    public SaveTextureToFileUtility.SaveTextureFileFormat textureFileFormat = SaveTextureToFileUtility.SaveTextureFileFormat.PNG;
    public string textureOutputPath = "Assets/CaptureStudio/Output/{0}";

    public List<GameObject> GameObjectToTextureQueue = new();
    private Queue<GameObject> textureQueue = new();


    public void StartCapture()
    {
        textureQueue.Clear();
        foreach (var gobj in GameObjectToTextureQueue)
        {
            var target = Instantiate(gobj);
            target.name = gobj.name;
            target.transform.position = Vector3.zero;
            target.SetActive(false);
            textureQueue.Enqueue(target);
        }

        CaptureRenderTexture();
    }

    private void CaptureRenderTexture()
    {
        GameObject currentObj = null;
        while (textureQueue.Count != 0)
        {
            currentObj = textureQueue.Dequeue();
            currentObj.SetActive(true);

            SceneView.RepaintAll(); //씬뷰를 강제로 갱신해 확실하게 씬이 현재상태 반영하도록 함
            TargetCamera.Render(); //랜더타겟텍스처에 현재 카메라 화면 캡처
            SaveTextureToFileUtility.SaveRenderTextureToFile(TargetCamera.targetTexture, string.Format(textureOutputPath, currentObj.name), textureFileFormat);

            Debug.Log("Capture Success! : " + currentObj.name);

            currentObj.SetActive(false);
            DestroyImmediate(currentObj);
        }

        AssetDatabase.Refresh();
    }
}

#endif
