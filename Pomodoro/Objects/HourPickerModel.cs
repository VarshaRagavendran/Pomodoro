using System.Collections.Generic;
using UIKit;
using Foundation;
using System;

public class HourPickerModel : UIPickerViewModel
{
    static int[] numberList;
    public EventHandler ValueChanged;
    public int SelectedValue;

    public HourPickerModel()
    {
        numberList = new int[] { 00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10};
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
