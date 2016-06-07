public static partial class Redux {

//	public static T compose<T> (T[] funcs) {
//		if (funcs.Length == 0) {
//			return arg => arg;
//		}
//
//		if (funcs.Length == 1) {
//			return funcs [0];
//		}
//
//		var last = funcs [funcs.Length - 1];
//		return r => {
//			T composed = last (r);
//			for (var i = funcs.Length - 2; i >= 0; --i) {
//				var f = funcs [i];
//				composed = f (composed);
//			}
//			return composed;
//		};
//	}

	public static ComposeEnhancer composeEnhancer = (funcs) => {
		if (funcs.Length == 0) {
			return arg => arg;
		}

		if (funcs.Length == 1) {
			return funcs [0];
		}

		var last = funcs [funcs.Length - 1];
		return v => {
			var composed = last (v);
			for (var i = funcs.Length - 2; i >= 0; --i) {
				var f = funcs [i];
				composed = f (composed);
			}
			return composed;
		};
	};

	public static ComposeDispatch composeDispatch = (funcs) => {
		if (funcs.Length == 0) {
			return arg => arg;
		}

		if (funcs.Length == 1) {
			return funcs [0];
		}

		var last = funcs [funcs.Length - 1];
		return v => {
			var composed = last (v);
			for (var i = funcs.Length - 2; i >= 0; --i) {
				var f = funcs [i];
				composed = f (composed);
			}
			return composed;
		};
	};
}