public static partial class ReduxMiddleware
{
	public delegate object Thunk(Redux.Dispatch dispatch);

	public static Redux.Middleware createThunk = api => next => action => {
		var thunk = action as Thunk;
		if (thunk != null) {
			return thunk(api.dispatch);
		}
		else {
			return next(action);
		}
	};
}