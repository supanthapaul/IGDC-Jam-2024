using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LockAndDoor;
using UnityEngine;

public class ChamberOpener : IOpener
{
    [SerializeField]
    private Transform chamber;
    [SerializeField]
    private float yOffset = 2f;
    [SerializeField]
    private float duration = 0.5f;
    
    private bool _state;
    private float _startY;

    private void Start()
    {
        _startY = chamber.position.y;
    }

    [ContextMenu("Open Door")]
    public void OnOpen()
    {
        SetOpen(true);
    }

    [ContextMenu("Close Door")]
    public void OnClose()
    {
        SetOpen(false);
    }

    public override void SetOpen(bool isOpen)
    {
        if (_state == isOpen) return;

        _state = isOpen;
        chamber.DOMoveY(isOpen ? _startY + yOffset : _startY, duration).SetEase(Ease.InSine);
    }
}
