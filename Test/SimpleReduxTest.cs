using UnityEngine;
using System.Collections;

public class SimpleReduxTest : MonoBehaviour {
	public static class Actions {
		public class Sum {
			public int ret;
			public Sum (int x, int y) {
				this.ret = x + y;
			}
		}
		public class Multiply {
			public int ret;
			public Multiply (int x, int y) {
				this.ret = x * y;
			}
		}
	}

	public static class Reducers {
		public static Redux.Reducer sumResult = (object state, object action) => {
			if (action.GetType ().FullName == "Redux+INITIAL_ACTION") {
				return 1;
			}
			if (action.GetType ().Name == "Sum") {
				var a = (Actions.Sum)action;
				return a.ret;
			}
			return state;
		};

		public static Redux.Reducer multiplyResult = (object state, object action) => {
			if (action.GetType ().FullName == "Redux+INITIAL_ACTION") {
				return 2;
			}
			if (action.GetType ().Name == "Multiply") {
				var a = (Actions.Multiply)action;
				return a.ret;
			}
			return state;
		};
	}

	Redux.Store store;

	// Use this for initialization
	void Start () {
		Debug.Log ("----------- Add reducers");

		this.store = Redux.createStore (
			Redux.combineReducers(new Redux.Reducer[]{
				Reducers.sumResult,
				Reducers.multiplyResult
			}),
			null,
			Redux.applyMiddleware(new Redux.Middleware[]{
				ReduxMiddleware.createLogger,
				ReduxMiddleware.createCrashReport
			})
		);

		Debug.Log ("----------- Subscribe");

		var unsubscribe = this.store.subscribe (this.OnChangeState);
		{
			Debug.Log ("----------- Dispatch");

			this.store.dispatch(new Actions.Sum (10, 20));
			this.store.dispatch(new Actions.Multiply (10, 20));

			Debug.Log ("----------- Remove reducers");

			Redux.removeReducers (new Redux.Reducer[]{
				Reducers.multiplyResult
			});

			Debug.Log ("----------- Dispatch");

			try {
				this.store.dispatch(new Actions.Sum (100, 200));
				this.store.dispatch(new Actions.Multiply (100, 200));
			} catch (Redux.Error e) {
				Debug.LogError (e.Message);
			}
		}

		Debug.Log ("----------- Unsubscribe");

		unsubscribe();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnChangeState (Redux.Store store) {
		Debug.Log ("sum: " + store.getState(Reducers.sumResult));
		Debug.Log ("multiply: " + store.getState(Reducers.multiplyResult));
	}
}