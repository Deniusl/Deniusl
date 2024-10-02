using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomDropdown : MonoBehaviour
{
    [SerializeField] private List<CustomDropdownItemOption> _options = new List<CustomDropdownItemOption>();
    [SerializeField] private CustomDropdownItem _itemPrefab;
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private CanvasGroup _itemsParentCanvasGroup;

    [SerializeField] private CustomDropdownItem _selectedItem;

    [SerializeField] private float _fadeDuration;

    private List<CustomDropdownItem> _allItems = new List<CustomDropdownItem>();

    public bool IsExpanded;
    public CustomDropdownItem SelectedItem => _selectedItem;

    private void Start()
    {
        IsExpanded = false;

        _itemsParentCanvasGroup.interactable = false;
        _itemsParentCanvasGroup.alpha = 0f;
        _itemsParentCanvasGroup.gameObject.SetActive(false);
    }

    public void Initialize(List<CustomDropdownItemOption> options)
    {
        _allItems.Clear();
        _options = options;
        for (int i = 0; i < _options.Count; i++)
        {
            CustomDropdownItemOption option = _options[i];
            CustomDropdownItem item = Instantiate(_itemPrefab, _itemsParent).GetComponent<CustomDropdownItem>();
            item.gameObject.name = $"Item {i}: {option.text}";
            item.Initialize(option,this,i);
            _allItems.Add(item);
        }
        _selectedItem = _allItems[0];
    }

    public void Initialize()
    {
        if(_options == null || _options.Count == 0)
        {
            Debug.Log("Cant init dropdown with zero options");
            return;
        }

        _allItems.Clear();
        for (int i = 0; i < _options.Count; i++)
        {
            CustomDropdownItemOption option = _options[i];
            CustomDropdownItem item = Instantiate(_itemPrefab, _itemsParent).GetComponent<CustomDropdownItem>();
            item.Initialize(option, this, i);
            _allItems.Add(item);
        }
        _selectedItem = _allItems[0];
    }

    public void ClearOptions()
    {
        _options.Clear();
    }

    public void AddOptions(List<CustomDropdownItemOption> options)
    {
        _options.AddRange(options);
    }

    public void SetSelectedItem(CustomDropdownItem item)
    {
        _selectedItem = item;
    }
    public void SetSelectedItem(int index)
    {
        _selectedItem = _allItems[index];
    }
    public void Show()
    {
        _itemsParentCanvasGroup.gameObject.SetActive(true);
        FadeIn().OnComplete(() =>
        {
            IsExpanded = true;
            _itemsParentCanvasGroup.interactable = true;
        });
    }

    public void Hide()
    {
        _itemsParentCanvasGroup.interactable = false;
        FadeOut().OnComplete(() =>
        {
            IsExpanded = false;
            _itemsParentCanvasGroup.gameObject.SetActive(false);
        });
    }

    private Tweener FadeOut()
    {
        return _itemsParentCanvasGroup.DOFade(0f, _fadeDuration);
    }

    private Tweener FadeIn()
    {
        return _itemsParentCanvasGroup.DOFade(1f, _fadeDuration);
    }
}
