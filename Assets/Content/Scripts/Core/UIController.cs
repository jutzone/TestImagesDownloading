using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button loadButton, cancelButton;
    [SerializeField] private TMP_Dropdown showTypeDropdown;

    private void Awake()
    {
        loadButton.onClick.AddListener(() =>
        {
            CoreController.StartLoading.Invoke();
        });
        cancelButton.onClick.AddListener(() => CoreController.CancelLoading.Invoke());

        showTypeDropdown.onValueChanged.AddListener((s) =>
        {
            showTypeDropdown.Hide();
            CoreController.ShowType = (ShowType)s;
        });
    }

    private void OnDisable()
    {
        loadButton.onClick.RemoveListener(() =>
       {
           CoreController.StartLoading.Invoke();
       });
        cancelButton.onClick.AddListener(() => CoreController.CancelLoading.Invoke());

        showTypeDropdown.onValueChanged.RemoveListener((s) =>
        {
            showTypeDropdown.Hide();
            CoreController.ShowType = (ShowType)s;
        });
    }
}
