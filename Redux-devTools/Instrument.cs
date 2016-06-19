using System;
using System.Diagnostics;

public static partial class Redux {
	public static partial class Devtools {

		static void log (string text) {
			System.Diagnostics.Debug.WriteLine (DateTime.Now + ": " + text);
			UnityEngine.Debug.Log (DateTime.Now + ": " + text);
		}

		public static Instrument instrument = (Listener listener) => {

			var timeline = new Timeline();

			MonitorAction monitorAction = action => {
				return ActionCreators.perform(action);
			};

			MonitorReducerImpl monitorReducerImple = null;
			monitorReducerImple = (finalReducer, stateTree, action_) => {
				var action = action_ as Action;
				var nextStateTree = stateTree;

				switch (action.type) {
				case Redux.ActionType.INIT: {
						var st = new PastState(new StateTree(), DateTime.Now, action);
						timeline.monitoredStates.Add(st);
						timeline.monitoredStateIndex = 0;
						nextStateTree = finalReducer(stateTree, action);
						log ("Devtools.InitialStateTree: " + nextStateTree.ToString());
					}
					break;

				case ActionTypes.RESET: {
						var monitoredState = timeline.monitoredStates[
							timeline.monitoredStateIndex = 0];
						
						stateTree = monitoredState.stateTree;
						nextStateTree = finalReducer(stateTree, monitoredState.action);
						log ("Devtools.InitialStateTree: " + nextStateTree);
					}
					break;

				case ActionTypes.PERFORM: {
						var a = action.to(new {
							timestamp = new DateTime(),
							action = new Object()
						});

						nextStateTree = monitorReducerImple(finalReducer, stateTree, a.action);
					}
					break;

				case ActionTypes.JUMP_TO_STATE: {
						var index = action.to<int>();
						if (timeline.monitoredStateIndex == index) {
							break;
						}

						var last = timeline.monitoredStates.Count - 1;
						index = index < 0 ? 0 : index;
						index = index >= last ? last : index;
						var monitoredState = timeline.monitoredStates[
							timeline.monitoredStateIndex = index];
						
						log ("Devtools.MonitoredStateTree: " + monitoredState.stateTree);
						log ("Devtools.Action: " + monitoredState.action);
						stateTree = monitoredState.stateTree;
						nextStateTree = finalReducer(stateTree, monitoredState.action);
						log ("Devtools.NextStateTree: " + nextStateTree);
					}
					break;

				default: {
						if (!timeline.timeTravling) {
							var st = new PastState(stateTree, DateTime.Now, action);
							timeline.monitoredStates.Add(st);
							timeline.monitoredStateIndex = timeline.monitoredStates.Count - 1;
						}

						nextStateTree = finalReducer(stateTree, action);
						log ("Devtools.NextStateTree: " + nextStateTree);
					}
					return nextStateTree;
				}

				if (listener != null) {
					listener(timeline);
				}
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