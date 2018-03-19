using System.Collections.Generic;
using UIKit;
using Foundation;
using System;

public class NumberPickerModel : UIPickerViewModel
{
    static int[] numberList;
    protected int selectedIndex = 0;
    public EventHandler ValueChanged;
    public int SelectedValue;

    public NumberPickerModel()
    {
        numberList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

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
