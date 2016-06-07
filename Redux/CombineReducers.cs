public static partial class Redux {

	public static string getUndefinedStateErrorMessage (object reducer, object action) {
		var reducerName = reducer.GetType().FullName;
		var actionName = action.GetType().FullName;
		return 
			"Given action " + actionName + ", reducer '" + reducerName + 
			"' returned undefined. " + "To ignore an action, you must explicitly return the previous state.";
	}

	static Reducers finalReducers;

	public static CombineReducers combineReducers = (reducers) => {
		
		finalReducers = new Reducers ();
		foreach (var reducer in reducers) {
			finalReducers.Add (reducer.GetHashCode (), reducer);
		}

		return (stateTree, action) => {
			stateTree = stateTree != null ? stateTree : new StateTree();

			var hasChanged = false;
			var nextStateTree = new StateTree ();
			foreach (var reducerPair in finalReducers) {
				var reducer = reducerPair.Value;
				var previousStateForKey = stateTree.ContainsKey(reducerPair.Key) ? 
					stateTree [reducerPair.Key] : null;
				
				var nextStateForKey = reducer (previousStateForKey, action);
				if (nextStateForKey == null) {
					var msg = getUndefinedStateErrorMessage (reducer, action);
					throw new Error (msg);
				}
				nextStateTree.Add (reducerPair.Key, nextStateForKey);
				hasChanged = hasChanged || !nextStateForKey.Equals (previousStateForKey);
			}
			return hasChanged ? nextStateTree : stateTree;
		};
	};

	public static RemoveReducers removeReducers = (reducers) => {
		foreach (var reducer in reducers) {
			finalReducers.Remove (reducer.GetHashCode ());
		}
	};
}