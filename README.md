# redux-unity3d
Redux for Unity3D (.NET 3.5)

This is C# version of Redux(http://redux.js.org/) for Unity3D. Almost every interfaces are same as the original Redux. 
However, I'm not expert in C#. So, it is welcome if you have an idea to elevate this library.

## create store
```c#
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
```

## subscribe, unsubscribe
```c#
var unsubscribe = this.store.subscribe (this.OnChangeState);
...
void OnChangeState (Redux.Store store) {
	Debug.Log ("sum: " + store.getState(Reducers.sumResult));
	Debug.Log ("multiply: " + store.getState(Reducers.multiplyResult));
}
...
unsubscribe();
```

## dispatch
```c#
this.store.dispatch(new Actions.Sum (100, 200));
this.store.dispatch(new Actions.Multiply (100, 200));
```

## remove reducers

I added a function to remove reducers for better performance but, I'm not sure whether this would be a good idea.
I haven't used this library for real project yet. So, I'll let you know later this was good idea or not.
Otherwise, you can protect me from a disaster through explanation why this is a bad idea.

```c#
Redux.removeReducers (new Redux.Reducer[]{
	Reducers.multiplyResult
});
```

### action, reducer
```c#
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
```

License
----
MIT
