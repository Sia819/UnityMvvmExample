using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UnityMvvmExample.ViewModels;
using System;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

public class MainView : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_InputField displayNameInput;
    [SerializeField] private Button applyButton;

    private MainViewModel viewModel;

    private void Awake()
    {
        // ViewModel 인스턴스화
        viewModel = new MainViewModel();
        viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void Start()
    {
        // TMP_InputField에 대한 OnValueChangedAsObservable 확장 메서드 사용
        displayNameInput.onValueChanged.AsObservable()
            .Subscribe(value => viewModel.Name = value)
            .AddTo(this);

        // ReactiveProperty를 사용하여 Name 속성 관찰
        applyButton.OnClickAsObservable()
            .Subscribe(_ => { viewModel.ApplyCommand.Execute(null); Debug.Log("Excute"); })
            .AddTo(this);
    }

    // ViewModel -> View
    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(MainViewModel.DisplayName):
                resultText.text = viewModel.DisplayName;
                break;
            default: break;
        }
    }

    private void OnDestroy()
    {
        viewModel.PropertyChanged -= OnViewModelPropertyChanged;
    }
}