using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{

    public class EventManager
    {

        public static event EventHandler<SearchInputEventArgs> SearchInputEvent;

        public static void InvokeSearchInputEvent(object o, SearchInputEventArgs e)
        {
            SearchInputEvent.Invoke(o, e);
        }

        public class SearchInputEventArgs
        {
            public Vector3 StartPosition { get; set; }
            public Vector3 GoalPosition { get; set; }
        }

    }

}
