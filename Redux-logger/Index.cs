public static partial class ReduxMiddleware
{
	public static Redux.Middleware createLogger = api => next => action => {
		System.Diagnostics.Debug.WriteLine ("ReduxLogger.Action: " + action.GetType().FullName);
		UnityEngine.Debug.Log ("ReduxLogger.Action: " + action.GetType().FullName);
		var result = next(action);
		return result;
	};
}