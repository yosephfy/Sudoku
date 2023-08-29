using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class BoardDataSave : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static string dir = Application.persistentDataPath;
#else
    private static string dir = Directory.GetCurrentDirectory();
#endif

    private static string file = @"\board_data.ini";
    private static string path = dir + file;

    public static void DeleteDataFile()
    {
        File.Delete(path);
    }

    public static bool GameDataFileExists()
    {
        return File.Exists(path);
    }

    public static void SaveBoard(Sudoku sudoku, List<box> boxes, float time, int errors)
    {
        File.WriteAllText(path, string.Empty);
        StreamWriter writer = new StreamWriter(path, false);
        string unsolved_string = "#unsolved:";
        string solved_string = "#solved:";
        string notes = "#notes:";
        string time_string = "#time:";
        string level_string = "#level:";
        string error_string = "#error:";

        unsolved_string += grid.Instance.ToString();

        solved_string += sudoku.ToString();

        foreach (var sqr in boxes)
        {
            notes += "@" + sqr.GetBoxIndex() + ">";
            foreach (var note_ in sqr.GetActiveNotes())
            {
                notes += note_ + "!";
            }
            notes += ",";
        }

        time_string += time.ToString();

        level_string += lvl(sudoku);

        error_string += errors.ToString();

        writer.WriteLine(unsolved_string);
        writer.WriteLine(solved_string);
        writer.WriteLine(notes);
        writer.WriteLine(time_string);
        writer.WriteLine(level_string);
        writer.WriteLine(error_string);

        writer.Close();
    }

    private static string lvl(Sudoku sudoku)
    {
        return sudoku.GetLevel() switch
        {
            20 => "easy",
            30 => "medium",
            40 => "hard",
            50 => "vhard",
            _ => "Idk",
        };
    }

    public static Sudoku GetSavedGrid()
    {
        string line;
        StreamReader file = new StreamReader(path);

        Sudoku sud = new Sudoku(9, LevelSelector.GetLevel());
        int[,] solved = new int[9, 9];
        int[,] unsolved = new int[9, 9];

        int solvedIndexRow = 0, solvedIndexCol = 0;
        int unsolvedIndexRow = 0, unsolvedIndexCol = 0;

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#unsolved")
            {
                string[] substring = Regex.Split(word[1], ",");

                foreach (var value in substring)
                {
                    int square_number = -1;
                    if (int.TryParse(value, out square_number))
                    {
                        if (unsolvedIndexCol >= 9)
                        {
                            unsolvedIndexRow++;
                            unsolvedIndexCol = 0;
                        }
                        unsolved[unsolvedIndexRow, unsolvedIndexCol] = square_number;
                        unsolvedIndexCol++;
                    }
                }
            }

            if (word[0] == "#solved")
            {
                string[] substring = Regex.Split(word[1], ",");

                foreach (var value in substring)
                {
                    int square_number = -1;
                    if (int.TryParse(value, out square_number))
                    {
                        if (solvedIndexCol >= 9)
                        {
                            solvedIndexRow++;
                            solvedIndexCol = 0;
                        }
                        solved[solvedIndexRow, solvedIndexCol] = square_number;
                        solvedIndexCol++;
                    }
                }
            }
        }

        file.Close();

        sud.FillFromFile(solved, unsolved);

        return sud;
    }

    public static List<int> GetNotesData(int index)
    {
        string line;
        StreamReader file = new StreamReader(path);
        List<int> result = new List<int>();

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#notes")
            {
                string[] substring = word[1].Split(','); //Regex.Split(word[2], ",");

                foreach (var note in substring)
                {
                    string[] sub = note.Split('>');
                    if (sub[0] == "@" + index.ToString())
                    {
                        string[] subList = Regex.Split(sub[1], "!");
                        foreach (var value in subList)
                        {
                            int square_number = -1;
                            if (int.TryParse(value, out square_number))
                            {
                                result.Add(square_number);
                            }
                        }
                    }
                }
            }
        }
        file.Close();
        return result;
    }

    public static float GetTimeData()
    {
        string line;
        StreamReader file = new StreamReader(path);

        float readTime = 0;

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#time")
            {
                float.TryParse(word[1], out readTime);
            }
        }

        return readTime;
    }

    public static string GetLevelData()
    {
        string line;
        StreamReader file = new StreamReader(path);

        string readLevel = "";

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#level")
            {
                readLevel = word[1];
            }
        }

        return readLevel;
    }

    public static int GetErrorData()
    {
        string line;
        StreamReader file = new StreamReader(path);

        int readLevel = 0;

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#error")
            {
                int.TryParse(word[1], out readLevel);
            }
        }

        return readLevel;
    }
}