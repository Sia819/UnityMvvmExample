using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityMvvmExample.Mvvm;
using UnityMvvmExample.ViewModels;

public class MainView : ViewBase<MainViewModel>
{
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_InputField displayNameInput;
    [SerializeField] private Button applyButton;

    private void Start()
    {
        TwoWayBinding<string>(nameof(ViewModel.Name),
                              () => displayNameInput.text,
                              value => displayNameInput.text = value,
                              displayNameInput.onValueChanged);

        OneWayBinding<string>(nameof(ViewModel.DisplayName),
                              value => resultText.text = value);

        CommandBinding(applyButton.onClick, 
                       () => ViewModel.ApplyCommand.Execute(null));
    }
}