using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sudoku
{
    private int[][] mat;
    private int[][] fullGrid;
    private int N; // number of columns/rows.
    private int SRN; // square root of N
    private int K; // No. Of missing digits

    // Constructor
    public Sudoku(int N, int K)
    {
        this.N = N;
        this.K = K;

        // Compute square root of N
        double SRNd = Math.Sqrt(N);
        SRN = (int)SRNd;

        mat = new int[N][];
        for (int i = 0; i < N; i++)
        {
            mat[i] = new int[N];
        }
        fullGrid = new int[N][];
        for (int i = 0; i < N; i++)
        {
            fullGrid[i] = new int[N];
        }
    }

    //copy constructor
    public Sudoku(Sudoku game)
    {
        //mat = game.mat;
        //fullGrid = game.fullGrid;
        //N = game.N;
        //SRN = game.SRN;
        //K = game.K;

        this.N = game.N;
        this.K = game.K;

        // Compute square root of N
        double SRNd = Math.Sqrt(game.N);
        SRN = (int)SRNd;

        mat = new int[game.N][];
        for (int i = 0; i < game.N; i++)
        {
            mat[i] = new int[game.N];
            for (int j = 0; j < game.N; j++)
            {
                mat[i][j] = game.mat[i][j];
            }
        }
        fullGrid = new int[game.N][];
        for (int i = 0; i < game.N; i++)
        {
            fullGrid[i] = new int[game.N];
            for (int j = 0; j < game.N; j++)
            {
                fullGrid[i][j] = game.fullGrid[i][j];
            }
        }
    }

    public void FillFromFile(int[,] solved, int[,] unsolved)
    {
        for (int i = 0; i < mat.Length; i++)
        {
            mat[i] = new int[N];
            for (int j = 0; j < mat[i].Length; j++)
            {
                mat[i][j] = unsolved[i, j];
            }
        }

        for (int i = 0; i < fullGrid.Length; i++)
        {
            fullGrid[i] = new int[N];
            for (int j = 0; j < fullGrid[i].Length; j++)
            {
                fullGrid[i][j] = solved[i, j];
            }
        }
    }

    // Sudoku Generator
    public void FillValues()
    {
        // Fill the diagonal of SRN x SRN matrices
        FillDiagonal();

        // Fill remaining blocks
        fillRemaining(0, SRN);

        // Remove Randomly K digits to make game
        RemoveKDigits();
    }

    // Fill the diagonal SRN number of SRN x SRN matrices
    private void FillDiagonal()
    {
        for (int i = 0; i < N; i = i + SRN)

            // for diagonal box, start coordinates->i==j
            fillBox(i, i);
    }

    // Returns false if given 3 x 3 block contains num.
    private bool unUsedInBox(int rowStart, int colStart, int num)
    {
        for (int i = 0; i < SRN; i++)
            for (int j = 0; j < SRN; j++)
                if (mat[rowStart + i][colStart + j] == num)
                    return false;

        return true;
    }

    // Fill a 3 x 3 matrix.
    private void fillBox(int row, int col)
    {
        int num;
        for (int i = 0; i < SRN; i++)
        {
            for (int j = 0; j < SRN; j++)
            {
                do
                {
                    num = randomGenerator(N);
                }
                while (!unUsedInBox(row, col, num));

                mat[row + i][col + j] = num;
                fullGrid[row + i][col + j] = num;
            }
        }
    }

    // Random generator
    private int randomGenerator(int num)
    {
        System.Random rand = new System.Random();
        return (int)Math.Floor((double)(rand.NextDouble() * num + 1));
    }

    // Check if safe to put in cell
    private bool CheckIfSafe(int i, int j, int num)
    {
        return (unUsedInRow(i, num) &&
                UnUsedInCol(j, num) &&
                unUsedInBox(i - i % SRN, j - j % SRN, num));
    }

    // check in the row for existence
    private bool unUsedInRow(int i, int num)
    {
        for (int j = 0; j < N; j++)
            if (mat[i][j] == num)
                return false;
        return true;
    }

    // check in the row for existence
    private bool UnUsedInCol(int j, int num)
    {
        for (int i = 0; i < N; i++)
            if (mat[i][j] == num)
                return false;
        return true;
    }

    // A recursive function to fill remaining
    // matrix
    private bool fillRemaining(int i, int j)
    {
        // System.out.println(i+" "+j);
        if (j >= N && i < N - 1)
        {
            i = i + 1;
            j = 0;
        }
        if (i >= N && j >= N)
            return true;

        if (i < SRN)
        {
            if (j < SRN)
                j = SRN;
        }
        else if (i < N - SRN)
        {
            if (j == (int)(i / SRN) * SRN)
                j = j + SRN;
        }
        else
        {
            if (j == N - SRN)
            {
                i = i + 1;
                j = 0;
                if (i >= N)
                    return true;
            }
        }

        for (int num = 1; num <= N; num++)
        {
            if (CheckIfSafe(i, j, num))
            {
                mat[i][j] = num;
                fullGrid[i][j] = num;
                if (fillRemaining(i, j + 1))
                    return true;

                mat[i][j] = 0;
                fullGrid[i][j] = 0;
            }
        }
        return false;
    }

    // Remove the K no. of digits to
    // complete game
    public void RemoveKDigits()
    {
        int count = K;
        while (count != 0)
        {
            int cellId = randomGenerator(N * N) - 1;

            // System.out.println(cellId);
            // extract coordinates i and j
            int i = (cellId / N);
            int j = (cellId % 9) + 1;
            if (j != 0)
                j = j - 1;

            // System.out.println(i+" "+j);
            if (mat[i][j] != 0)
            {
                count--;
                mat[i][j] = 0;
            }
        }
    }

    public override string ToString()
    {
        string output = "";

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
                output += fullGrid[i][j] + ",";
        }

        return output;
    }

    public int[][] GetGrid()
    {
        int[][] output = new int[N][];

        for (int i = 0; i < N; i++)
        {
            output[i] = new int[N];
            for (int j = 0; j < N; j++)
                output[i][j] = mat[i][j];
        }
        return output;
    }

    public int[][] GetSolvedGrid()
    {
        int[][] output = new int[N][];

        for (int i = 0; i < N; i++)
        {
            output[i] = new int[N];
            for (int j = 0; j < N; j++)
                output[i][j] = fullGrid[i][j];
        }
        return fullGrid;
    }

    public int[] giveHint()
    {
        return GiveHintAux(mat, 0, 0);
    }

    private int[] GiveHintAux(int[][] arr, int row, int col)
    {
        if (row >= arr.Length - 1)
        {
            return new int[2];
        }
        else if (col >= arr[row].Length - 1)
        {
            col = 0;
            return GiveHintAux(arr, row + 1, col);
        }
        else
        {
            System.Random rand = new System.Random();
            int r = rand.Next(9);
            int c = rand.Next(9);

            // System.out.println(r + " " + c);

            int k = 0;
            while (k < 81 & arr[r][c] != 0)
            {
                r = rand.Next(9);
                c = rand.Next(9);
                k++;
            }

            arr[r][c] = fullGrid[r][c];
            return new int[] { r, c };
        }
    }

    public int GetLevel()
    {
        return K;
    }

    public void SetLevel(int level)
    {
        K = level;
    }
}

// This code is contributed by rrrtnx.