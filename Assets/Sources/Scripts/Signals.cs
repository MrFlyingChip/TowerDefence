using UnityEngine;
using UnityEngine.Events;

public class SignalVoid : UnityEvent {}

public class SignalInt : UnityEvent<int> {}

public class SignalFloat : UnityEvent<float> {}

public class SignalVector3 : UnityEvent<Vector3> {}
