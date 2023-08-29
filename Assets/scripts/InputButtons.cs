using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButtons : MonoBehaviour
{
    public GameObject Buttons;
    public GameObject EraseButton;
    public List<GameObject> InputNumbers = new List<GameObject>();

    public static InputButtons Instance;

    private void Awake()
    {
        if (Instance)
            Destroy(Instance);

        Instance = this;
    }

    private void SetInputGrid()
    {
        for (int i = 0; i < 9; i++)
        {
            InputNumbers.Add(Instantiate(Buttons) as GameObject);
            InputNumbers[InputNumbers.Count - 1].transform.SetParent(this.transform);
            InputNumbers[InputNumbers.Count - 1].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            InputNumbers[InputNumbers.Count - 1].GetComponent<InputNums>().SetNum(i + 1);
            InputNumbers[InputNumbers.Count - 1].transform.localPosition = new Vector3((i * 57) - 228, 0, 1);
        }
        InputNumbers.Add(EraseButton as GameObject);
        InputNumbers[InputNumbers.Count - 1].GetComponent<InputNums>().SetNum(0);
    }

    public void SetInputNumsActive(int num, bool value)
    {
        InputNumbers[num - 1].SetActive(value);
    }

    // Start is called before the first frame update
    private void Start()
    {
        SetInputGrid();
    }

    // Update is called once per frame
    private void Update()
    {
    }
}