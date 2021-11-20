using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using Color = UnityEngine.Color;


public class ProcessData : MonoBehaviour
{
    Texture2D tex;
    public GameObject targetSprite;
    public string filename = "cat";
    public string OutFileName = "cats1000";
    string pathfile = "Assets/";

    public int index = 1;
    public bool debug = false;
    public bool save = false;

    public int Total = 100;

    public byte[] ReadAllBytes (string path){
        return File.ReadAllBytes(path);
    }

    byte[] data;
    int[] val = new int[784];

    byte[] outdata;

    // Start is called before the first frame update
    void Start()
    {
        filename += ".npy";

        tex = new Texture2D(28,28);
        Debug.Log(filename);

        data = new byte[ReadAllBytes(pathfile+filename).Length];
        data = ReadAllBytes(pathfile+filename);

        outdata = new byte[Total*784];

        GetImage();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && debug){
            GetImage();
        }
        if(Input.GetMouseButton(0) && save){
            SaveImage(Total);
        }
    }

    void GetImage(){
        int start = 80 + 784 * (index - 1);
        for (int i = 0; i < 784; i++)
        {
            int index = i + start;
            val[i] = data[index] & 0xff;
        }

        for (int i = 0; i < 28; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                //Debug.Log(28*i+j);
                int col = val[28*i+j];
                Color greyscale = new Color(col,col,col);
                tex.SetPixel(i,j,greyscale);
            }
        }

        tex.Apply();

        Sprite sprite = Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f));
        targetSprite.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void SaveImage(int numOfImg){
        int outindex = 0;
        for (int n = 0; n < numOfImg; n++)
        {
            int start = 80 + n * 784;
            for (int i = 0; i < 784; i++)
            {
                int index = i + start;
                val[i] = data[index] & 0xff;
                outdata[outindex] = (byte)val[i];
                outindex++;
            }     
        }
        File.WriteAllBytes(pathfile+OutFileName+".bin",outdata);
    }
}
