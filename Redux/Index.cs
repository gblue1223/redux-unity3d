using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public static partial class Redux {
	
	public class Error : Exception {
		public Error(string msg) : base(msg) {}
	};

	public class StateTree : Dictionary<Reducer, object>, System.ICloneable {
		public object Clone () {
			var st = new StateTree ();
			foreach (var pair in this) {
				var v = pair.Value as System.ICloneable;
				if (v != null) {
					st.Add (pair.Key, v.Clone ());
				} else {
					st.Add (pair.Key, pair.Value);
				}
			}
			return st;
		}

		public override string ToString () {
			var sb = new StringBuilder ();
			sb.Append ("{ ");
			foreach (var pair in this) {
				if (sb.Length > 2) {
					sb.Append (",\r\n");
				}
				sb.Append ("\"");
				sb.Append (pair.Key.Method.ReflectedType.FullName);
				sb.Append (".");
				sb.Append (pair.Key.Method.Name);
				sb.Append ("\"");
				sb.Append (": ");

				sb.Append ("\"");
				sb.Append (pair.Value.ToString ());
				sb.Append ("\"");
			}
			sb.Append (" }");
			return sb.ToString();
		}
	};

	public delegate object ActionCreator ();
	public delegate object ActionCreator<A1> (A1 a1);
	public delegate object ActionCreator<A1, A2> (A1 a1, A2 a2);
	public delegate object ActionCreator<A1, A2, A3> (A1 a1, A2 a2, A3 a3);
	public delegate object ActionCreator<A1, A2, A3, A4> (A1 a1, A2 a2, A3 a3, A4 a4);
	public delegate object ActionCreator<A1, A2, A3, A4, A5> (A1 a1, A2 a2, A3 a3, A4 a4, A5 a5);
	public class Action {
		public string type;
		public object data;
		public bool isInitialAction {
			get { return this.type == ActionType.INIT; }
		}
		public T to<T>() {
			return (T)this.data;
		}
		public T to<T>(T type) {
			return (T)this.data;
		}
		public override string ToString() {
			if (this.data != null) {
				return this.type + " { " + this.data + " }";
			}
			return this.type + " { }";
		}
	};

	public delegate object Reducer (object prevState, object action);
	public class Reducers : Dictionary<int, Reducer>, System.ICloneable {
		public object Clone () {
			var dic = new Reducers ();
			foreach (var pair in this) {
				dic.Add (pair.Key, pair.Value);
			}
			return dic;
		}
	};

	public delegate void Listener (Store store);
	public class Listeners : LinkedList<Listener>, System.ICloneable {
		public object Clone () {
			var ll = new Listeners ();
			foreach (var v in this) {
				ll.AddLast (v);
			}
			return ll;
		}
	};

	#region CreateStore

	public delegate Store CreateStore(FinalReducer finalReducer,
		StateTree initialStateTree = null, Enhancer enhancer = null);

	#endregion

	#region CombineReducers

	public delegate StateTree FinalReducer(StateTree stateTree, object action);
	public delegate FinalReducer CombineReducers (Reducer[] reducers);
	public delegate void RemoveReducers (Reducer[] reducers);

	#endregion

	#region Store

	public delegate StateTree GetStateTree ();
	public delegate object GetState (Reducer reducer);
	public delegate void ReplaceReducer (FinalReducer nextReducer);

	public delegate void Unsubscribe ();
	public delegate Unsubscribe Subscribe (Listener listener);
	public delegate object Dispatch (object action);

	#endregion

	#region Middleware

	public struct MiddlewareAPI {
		public GetStateTree getStateTree;
		public GetState getState;
		public Dispatch dispatch;
	};

	public delegate ComposedDispatch Middleware (MiddlewareAPI api);
	public delegate Enhancer ApplyMiddleware (Middleware[] middlewares);

	#endregion

	#region Compose

	public delegate CreateStore Enhancer (CreateStore createStore);
	public delegate Enhancer ComposeEnhancer (Enhancer[] funcs);

	public delegate Dispatch ComposedDispatch (Dispatch next);
	public delegate ComposedDispatch ComposeDispatch (ComposedDispatch[] funcs);

	#endregion
}