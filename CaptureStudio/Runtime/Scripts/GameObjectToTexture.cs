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

            SceneView.RepaintAll(); //���並 ������ ������ Ȯ���ϰ� ���� ������� �ݿ��ϵ��� ��
            TargetCamera.Render(); //����Ÿ���ؽ�ó�� ���� ī�޶� ȭ�� ĸó
            SaveTextureToFileUtility.SaveRenderTextureToFile(TargetCamera.targetTexture, string.Format(textureOutputPath, currentObj.name), textureFileFormat);

            Debug.Log("Capture Success! : " + currentObj.name);

            currentObj.SetActive(false);
            DestroyImmediate(currentObj);
        }

        AssetDatabase.Refresh();
    }
}

#endif
