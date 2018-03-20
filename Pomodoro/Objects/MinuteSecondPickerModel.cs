using System.Collections.Generic;
using UIKit;
using Foundation;
using System;

public class MinuteSecondPickerModel : UIPickerViewModel
{
    static int[] numberList;
    public EventHandler ValueChanged;
    public int SelectedValue;

    public MinuteSecondPickerModel()
    {
        numberList = new int[60];
        for (int i = 0; i <= 59; i++){
            numberList[i] = i;
        }
    }

    public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
    {
        return numberList.Length;
    }
    public override nint GetComponentCount(UIPickerView pickerView)
    {
        return 1;
    }
    public override string GetTitle(UIPickerView pickerView, nint row, nint component)
    {
        return Convert.ToString(numberList[(int)row]);
    }
    public override void Selected(UIPickerView pickerView, nint row, nint component)
    {
        var number = numberList[(int)row];
        SelectedValue = number;
        ValueChanged?.Invoke(null, null);
    }

}
