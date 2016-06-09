using System;
using System.Diagnostics;

public static partial class Redux {
	public static partial class Devtools {

		public static Instrument instrument = (Listener listener) => {

			var monitoredState = new MonitoredState();

			MonitorAction monitorAction = action => {
				return ActionCreators.perform(action);
			};

			MonitorReducerImpl monitorReducerImple = null;
			monitorReducerImple = (finalReducer, stateTree, action_) => {
				var action = action_ as Action;
				var nextStateTree = stateTree;

				switch (action.type) {
				case Redux.ActionType.INIT: {
						nextStateTree = finalReducer(stateTree, action);
						var st = new ComputedStateTree(nextStateTree, DateTime.Now, action);
						monitoredState.computedStateTrees.Add(st);
						monitoredState.computedStateTreeIndex = 0;
					}
					break;

				case ActionTypes.PERFORM: {
						var a = action.to(new {
							timestamp = new DateTime(),
							action = new Object()
						});
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

				case ActionTypes.RESET: {
						monitoredState.computedStateTreeIndex = 0;
						nextStateTree = monitoredState.computedStateTrees[
							monitoredState.computedStateTreeIndex];
					}
					break;

				case ActionTypes.JUMP_TO_STATE: {
						var index = action.to<int>();
						if (monitoredState.computedStateTreeIndex != index) {
							var last = monitoredState.computedStateTrees.Count - 1;
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
