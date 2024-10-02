using System;

[Serializable]
public class CustomDropdownItemOption
{
    public string text;
    public Action onClick;
    public Action<int> onClickWithIndex;

    public CustomDropdownItemOption(string text, Action onClick, Action<int> onClickWithIndex)
    {
        this.text = text;
        this.onClick = onClick;
        this.onClickWithIndex = onClickWithIndex;
    }
}
