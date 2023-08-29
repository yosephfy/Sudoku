using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class grid : MonoBehaviour
{
    public GameObject boxObj;
    public GameObject Buttons;
    public List<GameObject> boxes = new List<GameObject>();
    public List<GameObject> InputNumbers = new List<GameObject>();

    public Text LevelObj, MistakeObj;

    public Text GameWinLevel, GameWinTime, GameWinErrors;
    public Text GameLoseLevel, GameLoseTime, GameLoseErrors;

    public float box_scale = 0.55f;

    public Vector2 start_pos = new Vector2(0.0f, 0.0f);

    public float square_gap = 1.0f;
    public float every_sqr_offset = 0.1f;

    private int level = 0;
    private int Errors = 0;

    private static Sudoku sudoku;

    private static List<ActionsInterface> actions = new List<ActionsInterface>();
    public static int ActionCounter = 1;

    private static bool isNotesOn = false;

    public static grid Instance;

    private static bool isContinued;
    private static bool GameWon;
    private static bool GameLost;

    public static void SetContinue(bool value)
    {
        isContinued = value;

        if (!isContinued)
        {
            actions = new List<ActionsInterface>();
            isNotesOn = false;
        }
    }

    public static bool IsContinued()
    {
        return isContinued;
    }

    private void Awake()
    {
        if (Instance)
            Destroy(Instance);

        Instance = this;

        GameWon = false;

        level = LevelSelector.GetLevel();
        if (isContinued)
        {
            sudoku = new Sudoku(BoardDataSave.GetSavedGrid());
            sudoku.SetLevel(level);
            TimeCounter.SetTime(BoardDataSave.GetTimeData());
            Errors = BoardDataSave.GetErrorData();
        }
        else
        {
            sudoku = new Sudoku(9, level);
            sudoku.FillValues();
            TimeCounter.SetTime(0);
        }
        TimeCounter.StartTime(true);
        LevelObj.text = LevelSelector.GetLevelDisplay(level);
    }

    private void SpawnGrids()
    {
        //Debug.Log(sudoku.toString());
        int index = 0;
        start_pos = new Vector2(-box_scale * 410, box_scale * 410);
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                boxes.Add(Instantiate(boxObj) as GameObject);
                boxes[boxes.Count - 1].GetComponent<box>().SetBoxIndex(index);
                boxes[boxes.Count - 1].GetComponent<box>().SetBoxDimention(i, j);
                boxes[boxes.Count - 1].transform.SetParent(transform);
                boxes[boxes.Count - 1].transform.localScale = new Vector3(box_scale, box_scale, box_scale);
                boxes[boxes.Count - 1].transform.localPosition = new Vector3(0, 0, 0);

                index++;
            }
        }
    }

    private void SetPositionGridSqr()
    {
        var sqr_rect = boxes[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;

        offset.x = sqr_rect.rect.width * sqr_rect.transform.localScale.x + every_sqr_offset;
        offset.y = sqr_rect.rect.height * sqr_rect.transform.localScale.y + every_sqr_offset;

        int col_num = 0;
        int row_num = 0;

        foreach (GameObject square in boxes)
        {
            if (col_num + 1 > 9)
            {
                row_num++;
                col_num = 0;
                square_gap_number.x = 0.0f;
                row_moved = false;
            }
            var pos_x_offset = offset.x * col_num + (square_gap_number.x * square_gap);
            var pos_y_offset = offset.y * row_num + (square_gap_number.y * square_gap);

            if (col_num > 0 && col_num % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += square_gap;
            }
            if (row_num > 0 && row_num % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += square_gap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector3(start_pos.x + pos_x_offset, start_pos.y - pos_y_offset, 0);
            col_num++;
        }
    }

    private void SetNumbers(Sudoku sud)
    {
        int index = 0;
        int[,] result = new int[9, 9];

        //if (isContinued)
        //{
        //    sud = new Sudoku(BoardDataSave.GetSavedGrid());
        //    //sudoku = new Sudoku(BoardDataSave.GetSavedGrid());
        //}

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                isNotesOn = false;
                if (boxes[index].GetComponent<box>().GetNumber() != sud.GetSolvedGrid()[i][j] || boxes[index].GetComponent<box>().GetNumber() == 0)
                {
                    result[i, j] = sud.GetGrid()[i][j];
                    boxes[index].GetComponent<box>().SetDefaultNum(sud.GetGrid()[i][j] != 0 && sud.GetGrid()[i][j] == sud.GetSolvedGrid()[i][j]);

                    boxes[index].GetComponent<box>().SetNumber(result[i, j]);
                    boxes[index].GetComponent<box>().SetCorrectNumber(sud.GetSolvedGrid()[i][j]);

                    //if (isContinued && BoardDataSave.GetNotesData(index).Count != 0)
                    //{
                    //    //isNotesOn = true;
                    //    foreach (int not in BoardDataSave.GetNotesData(index))
                    //    {
                    //        boxes[index].GetComponent<box>().SetNotes();
                    //    }
                    //}
                }
                index++;
            }
        }
    }

    public void SetUserInput(int inputIndex, int boxIndex)
    {
        if (boxes[boxIndex].GetComponent<box>().IsDefaultNum() == false)
        {
            if (!isNotesOn)
                SetActions(boxIndex, inputIndex, boxes[boxIndex].GetComponent<box>().GetNumber());
            else
                SetActions(boxIndex, inputIndex);

            boxes[boxIndex].GetComponent<box>().SetNumber(inputIndex);
            if (!IsNotesClicked() && inputIndex != 0)
                grid.Instance.IncrementErrors(inputIndex != boxes[boxIndex].GetComponent<box>().GetCorrectNumber() ? 1 : 0);
            MistakeObj.text = Errors.ToString();

            ClickedBox(boxes[boxIndex].GetComponent<box>().GetDimentions(), boxIndex);
        }

        for (int i = 1; i < 9; i++)
        {
            InputButtons.Instance.SetInputNumsActive(i, !grid.Instance.IsInputNumberFull(i));
        }

        //SetActions(boxIndex, inputIndex);

        ActionCounter = 1;
    }

    public void ClickedBox(int[] indexDimention, int indexNum)
    {
        SetAllBoxColor(ColorTheme.Instance.GetTheme());
        VerticalLine(indexDimention[1]);
        HorizontalLine(indexDimention[0]);
        SquareBox(indexDimention[0], indexDimention[1]);
        SimilarNumbers(indexNum);
        boxes[indexNum].GetComponent<box>().SetBoxColor(ColorSetter.GiveColorGrid(ColorTheme.Instance.GetTheme()));
    }

    public void SetAllBoxColor(string theme)
    {
        foreach (var sqr in boxes)
        {
            sqr.GetComponent<box>().SetBoxColor(ColorSetter.GiveColorGrid(theme));
            sqr.GetComponent<box>().SetNumColor();
        }
    }

    private int SimilarNumbers(int index)
    {
        int count = 0;
        foreach (var sqr in boxes)
        {
            if (sqr.GetComponent<box>().GetNumber() != 0 & sqr.GetComponent<box>().GetNumber() == boxes[index].GetComponent<box>().GetNumber())
            {
                sqr.GetComponent<box>().SetBoxColor(ColorSetter.GiveColorSimilarNum(ColorTheme.Instance.GetTheme()));
                count++;
            }
        }
        return count;
    }

    private void VerticalLine(int indexNumber)
    {
        foreach (var sqr in boxes)
        {
            if (sqr.GetComponent<box>().GetDimentions()[1] == indexNumber)
            {
                sqr.GetComponent<box>().SetBoxColor(ColorSetter.GiveColorSimilarNum(ColorTheme.Instance.GetTheme()));
            }
        }
    }

    private void SquareBox(int indexRow, int indexCol)
    {
        int k = 0, h = 0;
        foreach (var sqr in boxes)
        {
            for (int i = 0; i < 9; i = i + 3)
                for (int j = 0; j < 9; j = j + 3)
                {
                    if (indexCol >= i & indexCol < i + 3 & indexRow >= j & indexRow < j + 3)
                    {
                        k = i;
                        h = j;
                    }
                }
        }

        // Debug.Log(k + " " + h);
        for (int i = k; i < k + 3; i++)
        {
            for (int j = h; j < h + 3; j++)
            {
                foreach (var sqr in boxes)
                {
                    if (sqr.GetComponent<box>().GetDimentions()[0] == j && sqr.GetComponent<box>().GetDimentions()[1] == i)
                        sqr.GetComponent<box>().SetBoxColor(ColorSetter.GiveColorSimilarNum(ColorTheme.Instance.GetTheme()));
                }
            }
        }
    }

    private void HorizontalLine(int indexNumber)
    {
        foreach (var sqr in boxes)
        {
            if (sqr.GetComponent<box>().GetDimentions()[0] == indexNumber)
            {
                sqr.GetComponent<box>().SetBoxColor(ColorSetter.GiveColorSimilarNum(ColorTheme.Instance.GetTheme()));
            }
        }
    }

    public void SetHints()
    {
        int[] copy = sudoku.giveHint();

        foreach (var sqr in boxes)
        {
            if (sqr.GetComponent<box>().GetDimentions()[0] == copy[0] & sqr.GetComponent<box>().GetDimentions()[1] == copy[1])
            {
                sqr.GetComponent<box>().SetDefaultNum(true);
                sqr.GetComponent<box>().SetNumber(sudoku.GetSolvedGrid()[copy[0]][copy[1]]);
            }
        }
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

            while (arr[r][c] != 0)
            {
                r = rand.Next(9);
                c = rand.Next(9);
            }

            arr[r][c] = sudoku.GetSolvedGrid()[r][c];
            return new int[] { r, c };
        }
    }

    public static void SetActions(int ind, int num, int prevNum)
    {
        actions.Add(new RegularNumbers(ind, num, prevNum));

        //if (ActionCounter > 1)
        //{
        //    for (int i = 0; i < ActionCounter - 2; i++)
        //    {
        //        actions.RemoveAt(i);
        //    }
        //}
    }

    public static void SetActions(int ind, int num)
    {
        actions.Add(new NotesNumbers(ind, num, -1));
    }

    public void UndoAction()
    {
        if (actions.Count - ActionCounter < 0)
            return;

        int indexNum = actions[actions.Count - ActionCounter].GetIndex();
        int numberNum = actions[actions.Count - ActionCounter].GetNumber();
        int prevNum = actions[actions.Count - ActionCounter].GetPrevNumber();

        if (prevNum == 0 && numberNum == 0)
        {
            actions.RemoveAt(actions.Count - ActionCounter);
            UndoAction();
        }

        if (prevNum == -1)
        {
            isNotesOn = true;
            boxes[indexNum].GetComponent<box>().SetNumber(numberNum);
        }
        else
        {
            isNotesOn = false;
            boxes[indexNum].GetComponent<box>().SetNumber(prevNum);
        }
        Debug.Log("setting " + indexNum + " from " + numberNum + " to " + prevNum);
        actions.RemoveAt(actions.Count - ActionCounter);

        //    ActionCounter++;
    }

    public void NotesClicked()
    {
        isNotesOn = !isNotesOn;

        //foreach (var sqr in boxes)
        //{
        //    sqr.GetComponent<box>().SetNotesActive(isNotesOn);
        //    //sqr.GetComponent<box>().SetActiveNumber(!isNotesOn);
        //}
    }

    public static bool IsNotesClicked()
    {
        return isNotesOn;
    }

    public List<box> GetBoxes()
    {
        List<box> result = new List<box>();
        foreach (var sqr in boxes)
        {
            result.Add(sqr.GetComponent<box>());
        }

        return result;
    }

    public override string ToString()
    {
        string output = "";

        foreach (var sqr in boxes)
        {
            output += sqr.GetComponent<box>().GetNumber().ToString() + ",";
        }

        return output;
    }

    private void OnDisable()
    {
        BoardDataSave.DeleteDataFile();

        if (!GameWon && !GameLost)
            BoardDataSave.SaveBoard(sudoku, GetBoxes(), TimeCounter.GetTime(), Errors);

        Stat.SaveCurrentStat(GameWon, GameLost, TimeCounter.GetTime(), LevelSelector.GetLevel(level));

        //
    }

    public bool IsGridFull()
    {
        foreach (var sqr in boxes)
        {
            if (sqr.GetComponent<box>().GetNumber() == 0)
            {
                return false;
            }
        }

        return true;
    }

    public static void CheckIfWon()
    {
        int index = 0;
        bool value = true;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid.Instance.boxes[index].GetComponent<box>().GetNumber() != sudoku.GetSolvedGrid()[i][j])
                {
                    value = false;
                }

                index++;
            }
        }

        if (value)
        {
            //foreach (var sqr in grid.Instance.boxes)
            //{
            //    sqr.GetComponent<box>().SetNumber(0);
            //}
            ScreenCapturing.Instance.TakeScreenShot();
            TimeCounter.Instance.WinGame();
            grid.Instance.SetWinData();
            GameWon = true;
        }
    }

    public void SetWinData()
    {
        GameWinLevel.text = LevelSelector.GetLevel(level);
        GameWinTime.text = TimeCounter.toString(TimeCounter.GetTime());
        GameWinErrors.text = Errors.ToString();
    }

    public static void SetPaused(bool timeStarted)
    {
        foreach (var sqr in grid.Instance.boxes)
        {
            sqr.SetActive(timeStarted);
        }
    }

    public void IncrementErrors(int num)
    {
        Errors += num;

        if (Errors >= 3)
        {
            TimeCounter.Instance.LoseGame();
            grid.Instance.SetLoseData();
            GameLost = true;
        }
    }

    private void SetLoseData()
    {
        GameLoseLevel.text = LevelSelector.GetLevel(level);
        GameLoseTime.text = TimeCounter.toString(TimeCounter.GetTime());
        GameLoseErrors.text = Errors.ToString();
    }

    public int GetErrors()
    {
        return Errors;
    }

    public void SetRestartGame()
    {
        GameLost = false;
        GameWon = false;

        int index = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (sudoku.GetSolvedGrid()[i][j] == sudoku.GetGrid()[i][j] || sudoku.GetGrid()[i][j] == 0)
                    boxes[index].GetComponent<box>().SetNumber(sudoku.GetGrid()[i][j]);
                else
                    boxes[index].GetComponent<box>().SetNumber(0);

                index++;
            }
        }

        for (int i = 1; i < 9; i++)
        {
            InputButtons.Instance.SetInputNumsActive(i, true);
        }

        //SetNumbers(sudoku);
        Errors = 0;
        MistakeObj.text = Errors.ToString();
        actions.Clear();
        Stat.SaveCurrentStat(false, false, TimeCounter.GetTime(), LevelSelector.GetLevel(level));
        TimeCounter.Instance.RestartGame();
    }

    public void SetContinueGame()
    {
        GameLost = false;
        GameWon = false;
        //SetNumbers(sudoku);
        Errors = 0;
        MistakeObj.text = Errors.ToString();
        actions.Clear();
        //Stat.SaveCurrentStat(false, false, TimeCounter.GetTime(), LevelSelector.GetLevel(level));
        TimeCounter.Instance.ContinueGame();
    }

    public bool IsInputNumberFull(int num)
    {
        int index = 0, counter = 0;
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (boxes[index].GetComponent<box>().GetNumber() == num)
                    counter++;

                index++;
            }
        }

        return counter == 9;
    }

    // Start is called before the first frame update
    private void Start()
    {
        SpawnGrids();
        SetPositionGridSqr();
        SetNumbers(sudoku);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}