using System;

public static partial class ReduxMiddleware
{
	static void log (string text) {
		System.Diagnostics.Debug.WriteLine (DateTime.Now + ": " + text);
		UnityEngine.Debug.Log (DateTime.Now + ": " + text);
	}

	public static Redux.Middleware createLogger = api => next => action => {
		var a = action as Redux.Action;
		log ("ReduxLogger.Action: " + a.ToString());
		return next(action);
	};
}