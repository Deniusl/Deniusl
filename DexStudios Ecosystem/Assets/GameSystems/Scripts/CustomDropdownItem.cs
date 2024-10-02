using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomDropdownItem : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;
    [SerializeField] private Button m_button;
    [SerializeField] private CustomDropdownItemOption _option;
    [SerializeField] private int _index;

    public CustomDropdownItemOption Option => _option;
    public int Index => _index;

    public void Initialize(CustomDropdownItemOption option, CustomDropdown dropdown, int index)
    {
        _index = index;
        _option = option;
        m_text.text = option.text;
        m_button.onClick.AddListener(() => {
            option.onClickWithIndex?.Invoke(index);
            dropdown.SetSelectedItem(this);
            option.onClick?.Invoke();
            dropdown.Hide();
        });
    }
}
