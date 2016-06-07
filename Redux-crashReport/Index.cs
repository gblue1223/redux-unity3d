public static partial class ReduxMiddleware
{
	public static Redux.Middleware createCrashReport = api => next => action => {
		try {
			return next(action);
		} catch (System.Exception e) {
			System.Diagnostics.Debug.WriteLine ("ReduxCrashReport: Caught an exception: " + e.Message);
			UnityEngine.Debug.LogError ("ReduxCrashReport: Caught an exception: " + e.Message);
			throw e;
		}
	};
}