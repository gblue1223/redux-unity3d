public static partial class ReduxMiddleware
{
	public delegate Redux.Dispatch Thunk(Redux.Dispatch dispatch);

	public static Redux.Middleware createThunk = api => next => action => {
		if (action.GetType().FullName == "ReduxMiddleware+Thunk") {
			return ((Thunk)action)(api.dispatch);
		}

		return next(action);
	};
}