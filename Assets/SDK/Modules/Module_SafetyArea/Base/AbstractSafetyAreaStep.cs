﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractSafetyAreaStep : ISafetyAreaStep
{
    private Action onEnterStepCallback;
    private Action onExitStepCallback;
    

    public virtual void RegistOnEnterStepCallback(Action callback)
    {
        onEnterStepCallback += callback;
    }

    public virtual void UnRegistOnEnterStepCallback(Action callback)
    {
        onEnterStepCallback -= callback;
    }

    public virtual void RegistOnExitStepCallback(Action callback)
    {
        onExitStepCallback += callback;
    }

    public virtual void UnRegistOnExitStepCallback(Action callback)
    {
        onExitStepCallback -= callback;
    }

    public virtual void OnEnterStep()
    {
        onEnterStepCallback?.Invoke();
    }

    public virtual void OnExitStep()
    {
        onExitStepCallback?.Invoke();
    }
}
