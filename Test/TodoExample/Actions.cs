using UnityEngine;
using System.Collections;

public partial class TodoExample : MonoBehaviour {
	public static class ActionCreators {
		static int nextTodoId = 0;

		public static Redux.ActionCreator<string> addTodo = (text) => {
			return new Redux.Action {
				type = "addTodo",
				data = new {
					id = nextTodoId++,
					text = text
				}
			};
		};

		public static Redux.ActionCreator<int> toggleTodo = (id) => {
			return new Redux.Action {
				type = "toggleTodo",
				data = id
			};
		};

		public static Redux.ActionCreator<string> setVisibilityFilter = (filter) => {
			return new Redux.Action {
				type = "setVisibilityFilter",
				data = filter
			};
		};
	}
}