using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nn
{
    int input_nodes;
    int hidden_nodes;
    int output_nodes;

    float learning_rate = 0.1f;

    Matrix weights_ih;
    Matrix weights_ho;

    Matrix bias_h;
    Matrix bias_o;

    public nn(int NumI, int NumH, int NumO){
        input_nodes = NumI;
        hidden_nodes = NumH;
        output_nodes = NumO;

        weights_ih = new Matrix(hidden_nodes, input_nodes);
        weights_ho = new Matrix(output_nodes, hidden_nodes);

        weights_ih.Randomize();
        weights_ho.Randomize();

        bias_h = new Matrix(hidden_nodes, 1);
        bias_o = new Matrix(output_nodes, 1);
        bias_h.Randomize();
        bias_o.Randomize();
    }

    public float[] FeedForward(float[] input_array){

        float[] sofmaxOutput;

        //Generating hidden outputs
        Matrix inputs = Matrix.fromArray(input_array);
        Matrix hidden = Matrix.Multiply(weights_ih, inputs);
        hidden.Add(bias_h);
        //activation function
        hidden.Map(sigmoid);

        //Generating Outputs
        Matrix output = Matrix.Multiply(weights_ho, hidden);
        output.Add(bias_o);
        output.Map(sigmoid);
        
        sofmaxOutput = softmax(output.toArray());

        //Sending it back
        //return output.toArray();
        return sofmaxOutput; //MAKES SURE ALL THE OUTPUT VALUES ADD UP TO 1 FOR PROBABILTY PURPOSES

    }

    float sigmoid(float x){
        return 1 / (1 + Mathf.Exp(-x));
    }

    float dsigmoid(float y){
        return y * (1 - y);
    }

    float[] softmax(float[] array){
        float sum = 0;
        float[] output = new float[array.Length];

        for (int i = 0; i < array.Length; i++)
        {
            sum += Mathf.Exp(array[i]);
        }
        for (int j = 0; j < array.Length; j++)
        {
            output[j] = Mathf.Exp(array[j]) / sum;
        }

        return output;
    }

    public void Train(float[] input_array, float[] target_array){

        //Generating hidden outputs
        Matrix inputs = Matrix.fromArray(input_array);
        Matrix hidden = Matrix.Multiply(weights_ih, inputs);

        hidden.Add(bias_h);
        //activation function
        hidden.Map(sigmoid);

        //Generating Outputs
        Matrix outputs = Matrix.Multiply(weights_ho, hidden);

        outputs.Add(bias_o);
        outputs.Map(sigmoid);

        Matrix TargetsMat = Matrix.fromArray(target_array);

        //Calculate the error
        //ERROR = TARGETS - OUTPUTS
        Matrix output_errors = Matrix.subtract(TargetsMat, outputs);

        //calculate gradient
        Matrix gradients = Matrix.Map(outputs, dsigmoid);
        gradients.Multiply(output_errors);

        gradients.Multiply(learning_rate);

        Matrix hidden_t = Matrix.Transpose(hidden);
        Matrix weight_ho_deltas = Matrix.Multiply(gradients, hidden_t);

        //adjust weights and biases by deltas
        weights_ho.Add(weight_ho_deltas);
        bias_o.Add(gradients);

        //calculate the hidden layer errors
        Matrix weights_ho_t = Matrix.Transpose(weights_ho);

        Matrix hidden_errors = Matrix.Multiply(weights_ho_t, output_errors);
        
        //calculate hidden gradient
        Matrix hidden_gradient = Matrix.Map(hidden, dsigmoid);

        //Debug.Log("cols mat A: " + hidden_gradient.cols + " rows mat A: "+ hidden_gradient.rows + " rows mat B: " + hidden_errors.rows + " cols mat B: " + hidden_errors.cols);

        hidden_gradient.Multiply(hidden_errors); //matrix dimension error
        hidden_gradient.Multiply(learning_rate);

        //calculate input to hidden deltas
        Matrix inputs_T = Matrix.Transpose(inputs);
        Matrix weight_ih_deltas = Matrix.Multiply(hidden_gradient, inputs_T);
 

        weights_ih.Add(weight_ih_deltas);
        bias_h.Add(hidden_gradient);


    }
}
