using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Captrue : MonoBehaviour {
    int ScreenHeight;
    int ScreenWidth;
    Camera cam;
    Rect rect;
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        ScreenHeight = 1024;
        ScreenWidth = 1024;
    }

    public void StartGetCapture()
    {
        CaptureCamera(cam, rect);
    }
    Texture2D CaptureCamera(Camera camera,Rect rects)
    {

        // 创建一个RenderTexture对象  
        RenderTexture rt = new RenderTexture(ScreenWidth, ScreenHeight, 1);
        // 设置相关相机的targetTexture为rt, 并手动渲染相机  
        camera.targetTexture = rt;
        camera.Render();
        // 激活这个rt, 并从中中读取像素。  
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D(ScreenWidth, ScreenHeight, TextureFormat.ARGB32, false);//TextureFormat设置成ARGB32，只渲染有像素的区域。
        //float vx = (v1.x > v2.x) ? v2.x : v1.x;      //取较小的x,y作为起始点
        //float vy = (v1.y > v2.y) ? v2.y : v1.y;
        screenShot.ReadPixels(new Rect(0, 0, 1024, 1024), 0, 0, false);//读取像素
        screenShot.Apply();

        // 重置相关参数，让camera继续在屏幕上显示  
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors   
        GameObject.Destroy(rt);
        //压缩纹理的尺寸
        int targetWidth = (int)(ScreenWidth * 0.5f);//压缩的比率
        int targetHeight = (int)(ScreenHeight * 0.5f);//压缩的比率
        Texture2D result = new Texture2D(targetWidth, targetHeight, screenShot.format, false);

        //float incX = (1.0f / (float)targetWidth);
        //float incY = (1.0f / (float)targetHeight);

        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = screenShot.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }

        result.Apply();
        // 最后将这些纹理数据，保存到一个png图片文件 
        byte[] bytes = result.EncodeToPNG();

        string filename = Application.dataPath + "/Screenshot.png";
        System.IO.File.WriteAllBytes(filename, bytes);
        return screenShot;

    }


    public void LoadImage()
    {

    }

    private IEnumerator StartLoadImage(Transform m_tran, string img)
    {
        yield return null;
        //bool isNextCoroutine = false;
        //HttpClient client = new HttpClient();
        //string imageurl = CGameConfig.imageHeadUrl + img;
        //client.GetByteArray(new System.Uri(imageurl), HttpCompletionOption.AllResponseContent, (r) =>
        //{
        //    if (this != null)
        //    {
        //        byte[] ba = r.Data;
        //        Texture2D tex = new Texture2D(536, 690, TextureFormat.ARGB32, false);
        //        tex.LoadImage(ba);
        //        Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        //        if (transform != null)
        //        {
        //            Transform mapTran = m_tran.Find("map");
        //            mapTran.SetAsLastSibling();
        //            mapTran.GetComponent<Image>().sprite = spr;
        //            mapTran.GetComponent<CanvasGroup>().alpha = 1f;
        //            //mapTran.GetComponent<CanvasGroup>().DOFade(1f, 1f);
        //            //Debug.Log("ooooo   " + img);
        //            DataManager.Instance.DicMapsSprite[img] = spr;
        //            if (isstart)
        //            {
        //                ShowUI(true);
        //                isstart = false;
        //            }
        //        }
        //    }
        //    isNextCoroutine = true;
        //});
        //while (!isNextCoroutine)
        //{
        //    yield return null;
        //}
        //ThreadCallTools.isRunCoroutine = false;
    }

    //将RenderTexture保存成一张png图片  
    public bool SaveRenderTextureToPNG(RenderTexture rt, string contents, string pngName)
    {
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        byte[] bytes = png.EncodeToPNG();
        if (!Directory.Exists(contents))
            Directory.CreateDirectory(contents);
        FileStream file = File.Open(contents + "/" + pngName + ".png", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
        file.Close();
        Texture2D.DestroyImmediate(png);
        png = null;
        RenderTexture.active = prev;
        return true;

    }  
}
