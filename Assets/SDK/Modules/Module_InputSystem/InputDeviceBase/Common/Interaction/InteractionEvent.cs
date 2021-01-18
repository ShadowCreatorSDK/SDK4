using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[Serializable]
public class InteractionEvent : UnityEvent<BaseEventData>
{ }
