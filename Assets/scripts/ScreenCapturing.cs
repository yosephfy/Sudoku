using System.Linq;
using System;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenCapturing : MonoBehaviour
{
    public GameObject ScreenshotImg;
    private Sprite newSprite;

    public int captureWidth = 1920;
    public int captureHeight = 1080;

    // configure with raw, jpg, png, or ppm (simple raw format)
    public enum Format
    { RAW, JPG, PNG, PPM };

    public Format format = Format.JPG;

    // folder to write output (defaults to data path)
    private string outputFolder;

    // private variables needed for screenshot
    private Rect rect;

    private RenderTexture renderTexture;
    private Texture2D screenShot;
    private string FileNameIS;
    private bool isProcessing;

    //Initialize Directory
    private void Start()
    {
        outputFolder = "Resources"/*Application.persistentDataPath*/ + "/Screenshots/";
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
            Debug.Log("Save Path will be : " + outputFolder);
        }
    }

    private string CreateFileName(int width, int height)
    {
        //timestamp to append to the screenshot filename
        string timestamp = DateTime.Now.ToString("yyyyMMddTHHmmss");
        // use width, height, and timestamp for unique file
        var filename = string.Format("{0}/screen_{1}x{2}_{3}.{4}", outputFolder, width, height, timestamp, format.ToString().ToLower());
        // return filename
        return filename;
    }

    private void CaptureScreenshot()
    {
        isProcessing = true;
        // create screenshot objects
        if (renderTexture == null)
        {
            // creates off-screen render texture to be rendered into
            rect = new Rect(0, 0, captureWidth, captureHeight);
            renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
            screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        }
        // get main camera and render its output into the off-screen render texture created above
        Camera camera = Camera.main;
        camera.targetTexture = renderTexture;
        camera.Render();
        // mark the render texture as active and read the current pixel data into the Texture2D
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);
        // reset the textures and remove the render texture from the Camera since were done reading the screen data
        camera.targetTexture = null;
        RenderTexture.active = null;
        // get our filename
        string filename = "Assets/Resources/shotImg." + format.ToString().ToLower();//CreateFileName((int)rect.width, (int)rect.height);
        // get file header/data bytes for the specified image format
        byte[] fileHeader = null;
        byte[] fileData = null;
        //Set the format and encode based on it
        if (format == Format.RAW)
        {
            fileData = screenShot.GetRawTextureData();
        }
        else if (format == Format.PNG)
        {
            fileData = screenShot.EncodeToPNG();
        }
        else if (format == Format.JPG)
        {
            fileData = screenShot.EncodeToJPG();
        }
        else //For ppm files
        {
            // create a file header - ppm files
            string headerStr = string.Format("P6\n{0} {1}\n255\n", rect.width, rect.height);
            fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
            fileData = screenShot.GetRawTextureData();
        }
        // create new thread to offload the saving from the main thread
        new System.Threading.Thread(() =>
        {
            var file = System.IO.File.Create(filename);
            if (fileHeader != null)
            {
                file.Write(fileHeader, 0, fileHeader.Length);
            }
            file.Write(fileData, 0, fileData.Length);
            file.Close();
            Debug.Log(string.Format("Screenshot Saved {0}, size {1}", filename, fileData.Length));

            FileNameIS = filename;
            Debug.Log(FileNameIS);
            isProcessing = false;
        }).Start();
        //Cleanup
        Destroy(renderTexture);
        renderTexture = null;
        screenShot = null;
    }

    public static ScreenCapturing Instance;

    public void TakeScreenShot()
    {
        if (!isProcessing)
        {
            CaptureScreenshot();

            //SpriteRenderer spriteRenderer = ScreenshotImg.GetComponent<SpriteRenderer>();
            //newSprite = Resources.Load<Sprite>("shotImg") as Sprite;
            //
            //spriteRenderer.sprite = newSprite;
            //ScreenshotImg.GetComponent<Image>().sprite = newSprite;
            //Debug.Log(("Screenshots/shotImg." + format.ToString().ToLower()));
        }
        else
        {
            Debug.Log("Currently Processing");
        }
    }

    private void Update()
    {
        newSprite = Resources.Load<Sprite>("shotImg") as Sprite;
        ScreenshotImg.GetComponent<Image>().sprite = newSprite;
        ScreenshotImg.GetComponent<Image>().preserveAspect = true;
        ScreenshotImg.GetComponent<Image>().useSpriteMesh = true;
    }

    private void Awake()
    {
        if (Instance)
            Destroy(Instance);

        Instance = this;
    }
}