using System;
using System.Diagnostics;
using System.Collections.Generic;

public static partial class Redux {
	public static partial class Devtools {
		#region Type

		public delegate void Listener (MonitoredState state);
		public delegate Enhancer Instrument (Listener listener);

		public class ComputedStateTree : StateTree {
			public DateTime timestamp;
			public object action;
			public ComputedStateTree(StateTree stateTree,
				DateTime timestamp, object action) : base(stateTree) {
				this.timestamp = timestamp;
				this.action = action;
			}
		}

		public class MonitoredState {
			public int computedStateTreeIndex = 0;
			public List<ComputedStateTree> computedStateTrees = new List<ComputedStateTree>();
		};

		internal delegate object MonitorAction (object action);
		internal delegate StateTree MonitorReducerImpl (FinalReducer finalReducer, StateTree stateTree, object action);
		internal delegate FinalReducer MonitorReducer (FinalReducer finalReducer);

		#endregion
	}
}