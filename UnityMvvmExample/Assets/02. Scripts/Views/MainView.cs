using Assets._02._Scripts.Mvvm.Adapter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityMvvmExample.Mvvm;
using UnityMvvmExample.ViewModels;

public class MainView : ViewBase<MainViewModel>
{
    [Header("Button Text Binding")]
    [SerializeField] private TMP_Text p1_resultText;
    [SerializeField] private TMP_InputField p1_displayNameInput;
    [SerializeField] private Button p1_applyButton;

    [Header("DropDown Binding")]
    [SerializeField] private TMP_Dropdown p2_dropdown;

    private void Start()
    {
        p1_resultText.SetBinding(TMP_TextProperty.Text, nameof(ViewModel.DisplayName));
        p1_displayNameInput.SetBinding(TMP_InputFieldProperty.Text, nameof(ViewModel.Name));
        p1_applyButton.SetCommand(ViewModel.ApplyCommand);
    }
}