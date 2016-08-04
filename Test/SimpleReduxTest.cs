using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

public class SimpleReduxTest : MonoBehaviour {
	public static class ActionCreators {
		public static Redux.ActionCreator<int, int> sum = (x, y) => {
			return new Redux.Action {
				type = "sum",
				data = x + y
			};
		};

		public static Redux.ActionCreator<int, int> multiply = (x, y) => {
			return (ReduxMiddleware.Thunk)(dispatch => {
				return dispatch(new Redux.Action {
					type = "multiply",
					data = x * y
				});
			});
		};
	}

	public class Reducers {
		public static Redux.Reducer sumResult = (state, action) => {
			var a = action as Redux.Action;
			if (a.isInitialAction) {
				return 1;
			}
			if (a.type == "sum") {
				return (int)a.data;
			}
			return state;
		};

		public static Redux.Reducer multiplyResult = (state, action) => {
			var a = action as Redux.Action;
			if (a.isInitialAction) {
				return 2;
			}
			if (a.type == "multiply") {
				return (int)a.data;
			}
			return state;
		};
	}

	Redux.Store store;

	// Use this for initialization
	void Start () {
		Debug.Log ("----------- Add reducers");

		var reducers = new Redux.Reducer[] {
			Reducers.sumResult,
			Reducers.multiplyResult
		};

//		var type = typeof (Reducers);
//		var methods = type.GetMethods (
//			System.Reflection.BindingFlags.NonPublic |
//			System.Reflection.BindingFlags.Public |
//			System.Reflection.BindingFlags.Static |
//			System.Reflection.BindingFlags.FlattenHierarchy);
//
//		var reducers = new List<Redux.Reducer> ();
//		foreach (var method in methods) {
//			if (method.DeclaringType.FullName == type.FullName) {
//				//reducers
//				Expression.Lambda<Redux.Reducer> (
//					Expression.Call (Expression.Convert (input, o.GetType ()), method), input).Compile ();
//			}
//		}

		this.store = Redux.createStore (
			Redux.combineReducers(reducers),
			null,
			Redux.applyMiddleware(new Redux.Middleware[]{
				ReduxMiddleware.createThunk,
				ReduxMiddleware.createLogger,
				ReduxMiddleware.createCrashReport
			})
		);

		Debug.Log ("----------- Subscribe");

		var unsubscribe = this.store.subscribe (this.onChangeState);
		{
			Debug.Log ("----------- Dispatch");

			this.store.dispatch(ActionCreators.sum (10, 20));
			this.store.dispatch(ActionCreators.multiply (10, 20));

			Debug.Log ("----------- Remove reducers");

			Redux.removeReducers (new Redux.Reducer[]{
				Reducers.multiplyResult
			});

			Debug.Log ("----------- Dispatch");

			try {
				this.store.dispatch(ActionCreators.sum (100, 200));
				this.store.dispatch(ActionCreators.multiply (100, 200));
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

	void onChangeState (Redux.Store store) {
		Debug.Log ("sum: " + store.getState(Reducers.sumResult));
		Debug.Log ("multiply: " + store.getState(Reducers.multiplyResult));
	}
}