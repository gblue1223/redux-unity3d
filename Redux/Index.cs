using System;
using System.Collections.Generic;

public static partial class Redux {
	
	public class Error : Exception {
		public Error(string msg) : base(msg) {}
	};

	public class StateTree : Dictionary<int, object>{
		public StateTree() : base() {}
		public StateTree(StateTree stateTree) : base(stateTree) {}
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
	};

	public delegate object Reducer (object prevState, object action);
	public class Reducers : Dictionary<int, Reducer>{
		public Reducers() : base() {}
		public Reducers(Reducers reducers) : base(reducers) {}
	};

	public delegate void Listener (Store store);
	public class Listeners : LinkedList<Listener>{
		public Listeners() : base() {}
		public Listeners(Listeners listeners) : base(listeners) {}
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