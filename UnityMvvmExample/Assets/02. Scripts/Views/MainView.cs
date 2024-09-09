using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UnityMvvmExample.ViewModels;
using System;
using CommunityToolkit.Mvvm.Input;

public class MainView : MonoBehaviour
{
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_InputField displayNameInput;
    [SerializeField] private Button applyButton;

    private MainViewModel viewModel;

    private void Start()
    {
        // ViewModel 인스턴스화
        viewModel = new MainViewModel();

        // TMP_InputField에 대한 OnValueChangedAsObservable 확장 메서드 사용
        displayNameInput.onValueChanged.AsObservable()
            .Subscribe(value => viewModel.DisplayName = value)
            .AddTo(this);

        // ReactiveProperty를 사용하여 Name 속성 관찰
        //viewModel.ToReactivePropertySlim(vm => vm.Name)
        //    .Subscribe(name => resultText.text = name)
        //    .AddTo(this);

        //applyButton.OnClickAsObservable()
        //    .Subscribe(_ => viewModel.Apply())
        //    .AddTo(this);
    }
}