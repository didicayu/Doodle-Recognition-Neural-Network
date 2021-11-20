using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Matrix
{
    public int rows;
    public int cols;
    
    public float[,] matrix;

    public Matrix(int rows, int cols){
        this.rows = rows;
        this.cols = cols;
        this.matrix = new float[rows, cols];

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i,j] = 0;
            }
        }
        
    }

    public void Multiply(float n){
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i,j] = matrix[i,j] * n;
            }
        }
    }
    public void Multiply(Matrix n){
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.cols; j++)
                {
                    this.matrix[i,j] *= n.matrix[i,j];
                }
            }
    }
    public void Map(Func<float, float> fn){ // Apply function to evey element of the matrix
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                var value = this.matrix[i,j];
                matrix[i,j] = fn(value);
            }
        }
    }
    public static Matrix Map(Matrix mat,Func<float, float> fn){ // Apply function to evey element of the matrix

        Matrix result = new Matrix(mat.rows, mat.cols);
        for (int i = 0; i < mat.rows; i++)
        {
            for (int j = 0; j < mat.cols; j++)
            {
                var value = mat.matrix[i,j];
                result.matrix[i,j] = fn(value);
            }
        }

        return result;
    }
    public static Matrix Multiply(Matrix mat1, Matrix mat2){

        Matrix result = new Matrix(mat1.rows, mat2.cols);
        if(mat1.cols != mat2.rows){
            Debug.LogError("Columns of Matrix A must match Rows of Matrix B");
            return null;
        }
        else{
            for (int i = 0; i < result.rows; i++)
            {
                for (int j = 0; j < result.cols; j++)
                {
                    float sum = 0;
                    for (int k = 0; k < mat1.cols; k++)
                    {
                        sum += mat1.matrix[i,k] * mat2.matrix[k,j];
                    }
                    result.matrix[i,j] = sum;
                }
            }

            return result;
        }
    }

    public void Add(float n){
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i,j] = matrix[i,j] + n;
            }
        }
    }

    public void Add(Matrix mat){
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i,j] += mat.matrix[i,j];
            }
        }
    }

    public static Matrix Transpose(Matrix mat){
        Matrix result = new Matrix(mat.cols,mat.rows);

        for (int i = 0; i < mat.rows; i++)
        {
            for (int j = 0; j < mat.cols; j++)
            {
                result.matrix[j,i] = mat.matrix[i,j];
            }
        }

        return result;
    }

    public void Randomize(){
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i,j] = Random.Range(-1f,1f);
            }
        }
    }

    public static Matrix subtract(Matrix a, Matrix b){
        Matrix result = new Matrix(a.rows, a.cols);
        for (int i = 0; i < a.rows; i++)
        {
            for (int j = 0; j < a.cols; j++)
            {
                result.matrix[i,j] = a.matrix[i,j] - b.matrix[i,j]; 
            }
        }
        return result;
    }

    public static Matrix fromArray(float[] arr){
        Matrix m = new Matrix(arr.Length, 1);

        for (int i = 0; i < arr.Length; i++)
        {
            m.matrix[i,0] = arr[i];
        }

        return m;
    }

    int k;
    public float[] toArray(){
        k = 0;
        float[] arr = new float[this.rows*this.cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                arr[k++] = matrix[i,j];
            }
        }

        return arr;
    }

    
}
