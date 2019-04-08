using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTrackableEventHandler : DefaultTrackableEventHandler
{
    public static readonly SignalVoid onTrackingFound = new SignalVoid();
    public static readonly SignalVoid onTrackingLost = new SignalVoid();
    
    protected override void OnTrackingFound()
    {
        onTrackingFound.Invoke();
    }

    protected override void OnTrackingLost()
    {
        onTrackingLost.Invoke();
    }
}
