public static partial class ReduxMiddleware
{
	public delegate object Thunk(Redux.Dispatch dispatch);

	public static Redux.Middleware createThunk = api => next => action => {
		try {
			return (action as Thunk)(api.dispatch);
		}
		catch (System.Exception) {
			return next(action);
		}
	};
}