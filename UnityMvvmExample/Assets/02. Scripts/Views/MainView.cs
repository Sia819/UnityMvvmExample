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
        // ViewModel �ν��Ͻ�ȭ
        viewModel = new MainViewModel();

        // TMP_InputField�� ���� OnValueChangedAsObservable Ȯ�� �޼��� ���
        displayNameInput.onValueChanged.AsObservable()
            .Subscribe(value => viewModel.DisplayName = value)
            .AddTo(this);

        // ReactiveProperty�� ����Ͽ� Name �Ӽ� ����
        //viewModel.ToReactivePropertySlim(vm => vm.Name)
        //    .Subscribe(name => resultText.text = name)
        //    .AddTo(this);

        //applyButton.OnClickAsObservable()
        //    .Subscribe(_ => viewModel.Apply())
        //    .AddTo(this);
    }
}