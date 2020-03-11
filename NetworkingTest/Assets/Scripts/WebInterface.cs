using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebInterface : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetText());
        StartCoroutine(UploadPNG());
    }


    public string screenShotURL = "https://studenthome.hku.nl/~Lotte.dejonge/";
    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://studenthome.hku.nl/~Lotte.dejonge/");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
             // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }

    IEnumerator UploadPNG()
    {
        // We should only read the screen after all rendering is complete
        yield return new WaitForEndOfFrame();

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);

        // Create a Web Form
        WWWForm form = new WWWForm();
        form.AddField("frameCount", Time.frameCount.ToString());
        form.AddBinaryData("fileUpload", bytes, "screenShot.png", "image/png");

        // Upload to a cgi script
        using (var w = UnityWebRequest.Post(screenShotURL, form))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError)
            {
                print(w.error);
            }
            else
            {
                print("Finished Uploading Screenshot");
            }
        }
    }
}
