using System;
using System.Diagnostics;

public static partial class Redux {
	public static partial class Devtools {

		public static class Actions {
			public class perform {
				public DateTime timestamp = DateTime.Now;
				public object action;
				public perform(object action) {
					this.action = action;
				}
			}

			public class reset {
				public DateTime timestamp = DateTime.Now;
			}

			public class jumpToState {
				public int index = 0;
				public jumpToState(int index) {
					this.index = index;
				}
			}
		}
	};
}