using System;
using System.Diagnostics;
using System.Collections.Generic;

public static partial class Redux {
	public static partial class Devtools {
		#region Type

		public delegate void Listener (Timeline timeline);
		public delegate Enhancer Instrument (Listener listener);

		public class PastState {
			public StateTree stateTree;
			public DateTime timestamp;
			public object action;

			public PastState(StateTree stateTree, DateTime timestamp, object action) {
				this.stateTree = (StateTree)stateTree.Clone ();
				this.timestamp = timestamp;
				this.action = action;
			}
		}

		public class Timeline {
			public int monitoredStateIndex = 0;
			public List<PastState> monitoredStates = new List<PastState>();
			public bool timeTravling {
				get { return this.monitoredStateIndex < this.monitoredStates.Count - 1; }
			}
		};

		internal delegate object MonitorAction (object action);
		internal delegate StateTree MonitorReducerImpl (FinalReducer finalReducer, StateTree stateTree, object action);
		internal delegate FinalReducer MonitorReducer (FinalReducer finalReducer);

		#endregion
	}
}