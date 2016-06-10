using System;
using System.Linq;

public static partial class Redux {

	internal static class ActionType {
		public const string INIT = "@@redux/C#/INIT";
	};
	internal static ActionCreator initialAction = () => {
		return new Action{ type = ActionType.INIT };
	};

	public static CreateStore createStore = (finalReducer, initialStateTree, enhancer) => {
		if (enhancer != null) {
			return enhancer (createStore) (finalReducer, initialStateTree, null);
		}

		var currentReducer = finalReducer;
		var currentStateTree = initialStateTree;
		var currentListeners = new Listeners ();
		var nextListeners = currentListeners;
		var isDispatching = false;
		Store store = new Store();

		System.Action ensureCanMutateNextListeners = () => {
			if (nextListeners.Equals (currentListeners)) {
				nextListeners = new Listeners (currentListeners);
			}
		};

		store.getStateTree = () => {
			return currentStateTree;
		};

		store.getState = (reducer) => {
			if (!currentStateTree.ContainsKey (reducer.GetHashCode ())) {
				throw new Error ("Reducer '" + reducer.GetHashCode () + "' not found.");
			}
			return currentStateTree [reducer.GetHashCode ()];
		};

		store.subscribe = (listener) => {
			if (listener == null) {
				throw new Error ("Expected listener to be a function.");
			}

			var isSubscribed = true;

			ensureCanMutateNextListeners ();
			nextListeners.AddLast (listener);

			Unsubscribe unsubscribe = () => {
				if (!isSubscribed) {
					return;
				}

				isSubscribed = false;

				ensureCanMutateNextListeners ();
				nextListeners.Remove (listener);
			};
			return unsubscribe;
		};

		store.dispatch = (action) => {
			if (action == null) {
				throw new Error ("Actions not defined");
			}

			if (isDispatching) {
				throw new Error ("Reducers may not dispatch actions.");
			}

			try {
				isDispatching = true;
				currentStateTree = currentReducer (currentStateTree, action);
			} finally {
				isDispatching = false;
			}

			var listeners = currentListeners = nextListeners;
			foreach (var listener in listeners) {
				listener (store);
			}

			return action;
		};

		store.replaceReducer = (nextReducer) => {
			currentReducer = nextReducer;
			store.dispatch(initialAction ());
		};

		store.dispatch (initialAction ());
		return store;
	};
}
