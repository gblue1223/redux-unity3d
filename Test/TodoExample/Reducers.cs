using UnityEngine;
using System.Collections;

public partial class TodoExample : MonoBehaviour {
	public class Item {
		public int id;
		public string text;
		public bool completed;
		public Item (int id, string text, bool completed) {
			this.id = id;
			this.text = text;
			this.completed = completed;
		}
	}

	public static class Reducers {
		#region todo

		private static Redux.Reducer todo = (object state, object action_) => {
			var action = action_ as Redux.Action;
			switch (action.type) {
			case "addTodo": {
					var data = action.to(new { id=0, text="" });
					return new Item (data.id, data.text, false);
				}
			
			case "toggleTodo": {
					var s = (Item)state;
					var id = action.to<int>();
					if (s.id != id) {
						return s;
					}
					return new Item (s.id, s.text, !s.completed);
				}
			}
			return state;
		};

		public static Redux.Reducer todos = (object state, object action_) => {
			var action = action_ as Redux.Action;
			if (action.isInitialAction) {
				return new Item[0];
			}
			switch (action.type) {
			case "addTodo": {
					var s = (Item[])state;
					var ns = new Item[s.Length + 1];
					s.CopyTo(ns, 0);
					ns[s.Length] = (Item)todo(state, action);
					return ns;
				}

			case "toggleTodo": {
					var s = (Item[])state;
					var ns = new Item[s.Length];
					for (int i=0, len=ns.Length; i<len; ++i) {
						ns[i] = (Item)todo(s[i], action);
					}
					return ns;
				}
			}
			return state;
		};

		#endregion

		#region visibilityFilter

		public static Redux.Reducer visibilityFilter = (object state, object action_) => {
			var action = action_ as Redux.Action;
			if (action.isInitialAction) {
				return "SHOW_ALL";
			}
			switch (action.type) {
			case "setVisibilityFilter": {
					var filter = action.to<string>();
					return filter;
				}
			}
			return state;
		};

		#endregion
	}
}