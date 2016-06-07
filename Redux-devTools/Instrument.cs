using System;
using System.Diagnostics;

public static partial class Redux {
	public static partial class Devtools {

		public static Instrument instrument = (Listener listener) => {

			var monitoredState = new MonitoredState();

			MonitorAction monitorAction = action => {
				return new Redux.Devtools.Actions.perform(action);
			};

			MonitorReducerImpl monitorReducerImple = null;
			monitorReducerImple = (finalReducer, stateTree, action) => {
				var actionType = action.GetType ().FullName;
				var nextStateTree = stateTree;

				switch (actionType) {
				case "Redux+INITIAL_ACTION":{
						nextStateTree = finalReducer(stateTree, action);
						var st = new ComputedStateTree(nextStateTree, DateTime.Now, action);
						monitoredState.computedStateTrees.Add(st);
						monitoredState.computedStateTreeIndex = 0;
					}
					break;
				case "Redux+Devtools+Actions+perform": {
						var a = (Redux.Devtools.Actions.perform)action;
						stateTree = monitorReducerImple(finalReducer, stateTree, a.action);
						nextStateTree = finalReducer(stateTree, a.action);
						if (!stateTree.Equals(nextStateTree)) {
							var st = new ComputedStateTree(nextStateTree, a.timestamp, a.action);
							monitoredState.computedStateTrees.Add(st);
							monitoredState.computedStateTreeIndex = 
								monitoredState.computedStateTrees.Count - 1;
						}
					}
					break;
				case "Redux+Devtools+Actions+reset": {
						monitoredState.computedStateTreeIndex = 0;
						nextStateTree = monitoredState.computedStateTrees[
							monitoredState.computedStateTreeIndex];
					}
					break;
				case "Redux+Devtools+Actions+jumpToState": {
						var a = (Redux.Devtools.Actions.jumpToState)action;
						if (monitoredState.computedStateTreeIndex != a.index) {
							var last = monitoredState.computedStateTrees.Count - 1;
							var index = a.index;
							index = index < 0 ? 0 : index;
							index = index >= last ? last : index;
							nextStateTree = monitoredState.computedStateTrees[
								monitoredState.computedStateTreeIndex = index];
						}
					}
					break;

				default:
					return nextStateTree;
				}

				listener(monitoredState);
				return nextStateTree;
			};

			MonitorReducer monitorReducer = finalReducer => (stateTree, action) => {
				return monitorReducerImple (finalReducer, stateTree, action);
			};

			return createStore => (finalReducer, initialStateTree, enhancer) => {

				var store = createStore(monitorReducer(finalReducer), initialStateTree, enhancer);
				var dispatch = store.dispatch;

				store.dispatch = (object action) => {
					dispatch(monitorAction(action));
					return action;
				};

				return store;
			};
		};
	}
}