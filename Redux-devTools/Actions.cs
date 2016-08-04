using System;
using System.Diagnostics;

public static partial class Redux {
	public static partial class Devtools {

		public static class ActionTypes {
			public const string PERFORM = "@@redux/C#/devtools/PERFORM";
			public const string RESET = "@@redux/C#/devtools/RESET";
			public const string JUMP_TO_STATE = "@@redux/C#/devtools/JUMP_TO_STATE";
		}

		public static class ActionCreators {
			public static Redux.ActionCreator<object> perform = (action) => {
				return new Redux.Action {
					type = ActionTypes.PERFORM,
					data = new {
						timestamp = DateTime.Now,
						action = action
					}
				};
			};

			public static Redux.ActionCreator<DateTime> reset = (action) => {
				return new Redux.Action {
					type = ActionTypes.RESET,
					data = DateTime.Now
				};
			};

			public static Redux.ActionCreator<int> jumpToState = (index) => {
				return new Redux.Action {
					type = ActionTypes.JUMP_TO_STATE,
					data = index
				};
			};
		}
	};
}