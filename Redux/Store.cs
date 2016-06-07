public static partial class Redux {

	public class Store {
		internal Store () {}

		private Subscribe subscribe_;
		public Subscribe subscribe {
			get { return this.subscribe_; }
			internal set { this.subscribe_ = value; }
		}

		private GetStateTree getStateTree_;
		public GetStateTree getStateTree {
			get { return this.getStateTree_; }
			internal set { this.getStateTree_ = value; }
		}

		private GetState getState_;
		public GetState getState {
			get { return this.getState_; }
			internal set { this.getState_ = value; }
		}

		private ReplaceReducer replaceReducer_;
		public ReplaceReducer replaceReducer {
			get { return this.replaceReducer_; }
			internal set { this.replaceReducer_ = value; }
		}

		private Dispatch dispatch_;
		public Dispatch dispatch {
			get { return this.dispatch_; }
			internal set { this.dispatch_ = value; }
		}
	}
}