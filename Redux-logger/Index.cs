using System;

public static partial class ReduxMiddleware
{
	static void log (string text) {
		System.Diagnostics.Debug.WriteLine (DateTime.Now + ": " + text);
		text = text.Replace ('<', '(').Replace ('>', ')');
		UnityEngine.Debug.Log (DateTime.Now + ": <color=olive>" + text + "</color>");
	}

	public static Redux.Middleware createLogger = api => next => action => {
		var a = action as Redux.Action;
		log ("ReduxLogger.Action: " + a.ToString());
		return next(action);
	};
}