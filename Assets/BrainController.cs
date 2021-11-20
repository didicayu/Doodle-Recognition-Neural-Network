using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainController : MonoBehaviour
{
    Matrix a = new Matrix(2,3);
    Matrix b = new Matrix(3,2);

    Matrix result;

    nn NeuralNetwork;
    float[] output;

    float[][] TrainingInputs = new float[][]
    {
        new float[] {0,0},
        new float[] {0,1},
        new float[] {1,0},
        new float[] {1,1}
    };
    float[][] TrainingTargets = new float[][]{
        new float[]{0,0},
        new float[]{1,0},
        new float[]{1,1},
        new float[]{0,1}
    };
    // Start is called before the first frame update
    void Start()
    {
        NeuralNetwork = new nn(2,4,2);
        /*
        Debug.Log(NeuralNetwork.FeedForward(TrainingInputs[0])[0]);
        Debug.Log(NeuralNetwork.FeedForward(TrainingInputs[1])[0]);
        Debug.Log(NeuralNetwork.FeedForward(TrainingInputs[2])[0]);
        Debug.Log(NeuralNetwork.FeedForward(TrainingInputs[3])[0]);*/
        Debug.Log("---------------------------");
        //output = NeuralNetwork.FeedForward(input);
        int a;

        for (int i = 0; i < 50000; i++)
        {
            for (int j = 0; j < TrainingInputs.Length; j++)
            {
                a = Random.Range(0, TrainingInputs.Length);
                NeuralNetwork.Train(TrainingInputs[a], TrainingTargets[a]);
            }
        }

        
        Debug.Log(NeuralNetwork.FeedForward(TrainingInputs[0])[0]+" "+NeuralNetwork.FeedForward(TrainingInputs[0])[1]);
        Debug.Log(NeuralNetwork.FeedForward(TrainingInputs[1])[0]+" "+NeuralNetwork.FeedForward(TrainingInputs[1])[1]);
        Debug.Log(NeuralNetwork.FeedForward(TrainingInputs[2])[0]+" "+NeuralNetwork.FeedForward(TrainingInputs[2])[1]);
        Debug.Log(NeuralNetwork.FeedForward(TrainingInputs[3])[0]+" "+NeuralNetwork.FeedForward(TrainingInputs[3])[1]);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DebugMatrix(Matrix mat, string MatrixName){
        for (int i = 0; i < mat.rows; i++)
        {
            for (int j = 0; j < mat.cols; j++)
            {
                Debug.Log(MatrixName + "  Num in row " + i + " and col " + j + " contains: " + mat.matrix[i,j]);
            }
        }
    }

    float Add1ToN(float n){
        return n + 1;    
    }
}
