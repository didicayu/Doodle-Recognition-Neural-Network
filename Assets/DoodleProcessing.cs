using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using TMPro;


public class DoodleProcessing : MonoBehaviour
{
    const int len = 784;
    byte[][] cats_training, rainbows_training, planes_training, bannana_training;
    byte[] cats_data, rainbows_data, planes_data, bannana_data;

    float[][][] TrainingData = new float[4][][];
    float[][][] TestingData = new float[4][][];

    int cat = 0;
    int rainbow = 1;
    int plane = 2;
    int bannana = 3;

    public int LabelIndex = 0;
    public int ImageIndex = 0;
    int[] val = new int[len];
    byte[] data;

    string targetCat = "cats";
    string targetPlane = "airplane";
    string targetBannana = "bannana";
    string targetRainbow = "rainbow";

    nn NeuralNetwork;

    public byte[] ReadAllBytes (string path){
        return File.ReadAllBytes(path);
    }

    Texture2D tex;
    public Texture2D DrawnTex;
    Texture2D ScalableTex;
    public GameObject targetSprite;

    public GameObject ParentTextGO;
    public TextMeshProUGUI CatText, RainbowText, AirplaneText, BannanaText, EpochCounter, testpercenttext;

    string path;
    // Start is called before the first frame update
    float[] result = new float[4];
    void Start()
    {

        path = Application.streamingAssetsPath + "/binaries/";

        GetAllData();
        InitializeJaggedArrays();
        AssignTrainingData();
        InitializeData();

        NormalizeData(); //converteix de 0 - 255 a 0 - 1.0

        NeuralNetwork = new nn(len,64,4); // Creates the neural network
        Debug.Log(NeuralNetwork);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DebugArrayImg(byte[][] array){
        tex = new Texture2D(28,28);
        int[] img1 = new int[len];
        int[] img2 = new int[len];

        for (int i = 0; i < len; i++)
        {
            img1[i] = array[0][i] & 0xff;
            img2[i] = array[1][i] & 0xff;

            if(img1[i] != img2[i]){
                Debug.Log("funcione hostia puta");
            }
        }
    }

    void ShowOnSprite(float[] array){

        tex = new Texture2D(28,28,TextureFormat.RGB48,false);

        data = array.FloatArrayToByteArray();

        int start = 0;
        for (int i = 0; i < len; i++)
        {
            int index = i + start;
            val[i] = data[index] & 0xff;
        }

        for (int y = 0; y < 28; y++)
        {
            for (int x = 0; x < 28; x++)
            {
                float col = val[28*y+x] * 255f;

                Color greyscale = new Color(col,col,col);
                tex.SetPixel(x,y,greyscale);
            }
        }

        tex.Apply();

        Sprite sprite = Sprite.Create(tex,new Rect(0,0,tex.width,tex.height),new Vector2(0.5f,0.5f),10);
        targetSprite.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void InitializeJaggedArrays(){
        int sublen = 1000;

        cats_training = new byte[sublen][];
        rainbows_training = new byte[sublen][];
        planes_training = new byte[sublen][];
        bannana_training = new byte[sublen][];

        for (int i = 0; i < sublen; i++)
        {
            cats_training[i] = new byte[len];
            rainbows_training[i] = new byte[len];
            planes_training[i] = new byte[len];
            bannana_training[i] = new byte[len];
        }
    }

    void AssignTrainingData(){
        
        for (int i = 0; i < 1000; i++)
        {
            int off = i * len;
            cats_training[i] = cats_data.SubArray(off, off+len);
            rainbows_training[i] = rainbows_data.SubArray(off, off+len);
            planes_training[i] = planes_data.SubArray(off, off+len);
            bannana_training[i] = bannana_data.SubArray(off, off+len);
        }      
    }

    byte[] GetData(string target){
        data = new byte[ReadAllBytes(path+target+"1000.bin").Length];
        data = ReadAllBytes(path+target+"1000.bin");

        return data;
    }

    void GetAllData(){
        cats_data = GetData(targetCat);
        rainbows_data = GetData(targetRainbow);
        planes_data = GetData(targetPlane);
        bannana_data = GetData(targetBannana);
    }

    void InitializeData(){
        
        for (int i = 0; i < 4; i++)
        {
            TrainingData[i] = new float[800][];
            TestingData[i] = new float[200][];
            for (int k = 0; k < 800; k++)
            {
                TrainingData[i][k] = new float[len];
            }
            for (int j = 0; j < 200; j++)
            {
                TestingData[i][j] = new float[len];
            }
        }
        for (int n = 0; n < 1000; n++)
        {
            for (int j = 0; j < len; j++)
            {
                if(n < 800)
                {
                    TrainingData[cat][n][j] = cats_training[n][j];
                    TrainingData[rainbow][n][j] = rainbows_training[n][j];
                    TrainingData[plane][n][j] = planes_training[n][j];
                    TrainingData[bannana][n][j] = bannana_training[n][j];
                }
                else{
                    TestingData[cat][n-800][j] = cats_training[n][j];
                    TestingData[rainbow][n-800][j] = rainbows_training[n][j];
                    TestingData[plane][n-800][j] = planes_training[n][j];
                    TestingData[bannana][n-800][j] = bannana_training[n][j];
                }
                
            }
        }
        
    }

    float[] targets = new float[] {0,0,0,0};
    int epoch = 0;

    public void StartTraining(){
        for (int i = 0; i < (800*4); i++)
        {
            int index = Random.Range(0,4);
            int ImgIndex = Random.Range(0,800);

            targets[index] = 1; //{1,0,0,0}[GAT] o {0,1,0,0}[ARC DE SANT MARTÍ] o {0,0,1,0}[AVIÓ] o {0,0,0,1}[PLATAN]

            //ENTRENA UNA SOLA IMATGE AMB LA SEVA TARGET CORRESPONENT
            NeuralNetwork.Train(TrainingData[index][ImgIndex], targets);

            for (int j = 0; j < targets.Length; j++)
            {
                targets[j] = 0;
            }
        }
        epoch++;

        EpochCounter.SetText("Trained for {0} Epochs", epoch);

        TestNeuralNetPerformance();
        
    }

    void NormalizeData(){
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 800; j++)
            {
                for (int k = 0; k < len; k++)
                {
                    TrainingData[i][j][k] = TrainingData[i][j][k] / 255f;
                }
            }

            for (int n = 0; n < 200; n++)
            {
                for (int o = 0; o < len; o++)
                {
                    TestingData[i][n][o] = TestingData[i][n][o] / 255f;
                }
            }
        }
    }

    public void GetDrawingData(){

        //Get Texture data from drawable texture
        ScalableTex = new Texture2D(28,28);
        ScalableTex = TextureScale.scaled(DrawnTex,28,28,FilterMode.Trilinear);
        ScalableTex.Apply();

        //Get Prediction Of Drawing
        float[] InputArray;
        InputArray = ScalableTex.GetPixels().ColorToFloat();

        //Show on screen
        ShowOnSprite(InputArray);

        //flip and normalize
        InputArray = reverseFuckery(InputArray); // Unity es retrassat i dibuixe les textures de abaix a dalt de esquerra a dreta en lloc de a dalt abaix esquerra dreta i es un puto mal de cap convertir-ho
        
        //Show Confidence percentages
        GetResult(InputArray);
        UpdateResult();
    }

    void UpdateResult(){
        //Show Confidence percentages
        CatText.SetText("Cat: {0}%",result[0]*100f);
        RainbowText.SetText("Rainbow: {0}%",result[1]*100f);
        AirplaneText.SetText("Airplane: {0}%",result[2]*100f);
        BannanaText.SetText("Bannana: {0}%",result[3]*100f);
    }

    void GetResult(float[] arr){
        for (int i = 0; i < 4; i++)
        {
            result[i] = NeuralNetwork.FeedForward(arr)[i];
        }
    }
    
    int k = 0;
    float[] reverseFuckery(float[] array){

        float[] result = new float[array.Length];

        float[][] original = new float[28][];
        float[][] flipped = new float[28][];

        for (int i = 0; i < 28; i++)
        {
            original[i] = new float[28];
            flipped[i] = new float[28];
        }

        for (int i = 0; i < 28; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                original[i][j] = array[k++];

            }
        }

        for (int i = 0; i < 28; i++)
        {
            flipped[i] = original[27-i];
        }

        k = 0;

        for (int i = 0; i < 28; i++)
        {
            for (int j = 0; j < 28; j++)
            {
                result[k++] = flipped[i][j];
            }
        }
        k = 0;
        return result;
    }

    public void TestNeuralNetPerformance(){

        float[] prediciton;
        int correct = 0;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 200; j++)
            {
                prediciton = NeuralNetwork.FeedForward(TestingData[i][j]);
                float maxValue = prediciton.Max();
                
                int maxIndex = prediciton.ToList().IndexOf(maxValue);
                
                if(maxIndex == i){
                    correct++;
                }
            }
        }

        float percentage = 100 * correct / (200f * 4);
        testpercenttext.SetText("Predicted succesfuly {0}% of the testing data", percentage);
    }

    

}

public static class Extensions{
    public static T[] SubArray<T>(this T[] array, int start, int final){
        T[] result = new T[final-start];

        for (int i = 0; i < (final-start); i++)
        {
            result[i] = array[start+i];
        }

        return result;
    }
    public static byte[] FloatArrayToByteArray(this float[] array){
        byte[] result = new byte[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            result[i] = (byte)(array[i] * 255); //MULTIPLICO PER 255 PERQUE ABANS HE DIVIDIT ENTRE 255 AL NORMALITZAR
            //FUNCIÓ NO PENSADA PER A SER USADA FORA DEL SHOWIMAGE DEBUG
        }
        return result;
    }

    public static float[] ColorToFloat(this Color[] col)
    {
        float[] result = new float[col.Length];

        for (int i = 0; i < col.Length; i++)
        {
            result[i] = col[i].r;
        }

        return result;
    }
}

