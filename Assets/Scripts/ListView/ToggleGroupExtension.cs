using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ToggleGroup))]
public class ToggleGroupExtension : MonoBehaviour
{
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private int currentId;
    public int CurrentValue { get => currentId; set => currentId = value; }
    public ToggleGroup ToggleGroup { get => toggleGroup; set => toggleGroup = value; }

    public void SetCurrentId(int elementId)
    {
        bool tmp = (this.currentId == elementId);
        this.currentId = elementId;
        if (tmp) { this.onValueChanged?.Invoke(this.currentId); }
    }

    public bool GetElementValue(int elementId)
    {
        return elementId == this.currentId;
    }

    public event Action<int> onValueChanged;
}