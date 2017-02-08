using UnityEngine;
using System.Collections;

public partial class TodoExample : MonoBehaviour {

	Redux.Store store;

	// Use this for initialization
	void Start () {
		var enhancer = Redux.composeEnhancer (new Redux.Enhancer[] {
			Redux.applyMiddleware(new Redux.Middleware[] {
				ReduxMiddleware.createLogger,
				ReduxMiddleware.createCrashReport
			}),
			Redux.Devtools.instrument(this.onChangeMonitoredState)
		});

		this.store = Redux.createStore(TodoList.reducer, null, enhancer);
		this.store.subscribe(this.onChangeState);
		this.onChangeState(this.store);
	}

	// Update is called once per frame
	void Update () {
	}

	string visibilityFilter = "";
	string todoTitle = "";
	TodoList.Item[] todos;

	Redux.Devtools.Timeline timeline;

	TodoList.Item getVisibleTodo (TodoList.Item todo) {
		switch (this.visibilityFilter) {
		case "SHOW_COMPLETED":
			return todo.completed ? todo : null;
		case "SHOW_ACTIVE":
			return !todo.completed ? todo : null;
		case "SHOW_ALL":
			break;
		}
		return todo;
	}

	void OnGUI () {
		var x = 20;
		var y = 40;
		var w = 100;
		var h = 30;
		var xgap = 10;
		var ygap = 10;
		if (GUI.Toggle (new Rect (x, y, w, h), this.visibilityFilter == "SHOW_ALL", "Show All")) {
			if (this.visibilityFilter != "SHOW_ALL") {
				this.store.dispatch (TodoList.ActionCreators.setVisibilityFilter("SHOW_ALL"));
			}
		}
		x += w + xgap;
		if (GUI.Toggle (new Rect (x, y, w, h), this.visibilityFilter == "SHOW_ACTIVE", "Show Active")) {
			if (this.visibilityFilter != "SHOW_ACTIVE") {
				this.store.dispatch (TodoList.ActionCreators.setVisibilityFilter ("SHOW_ACTIVE"));
			}
		}
		x += w + xgap;
		if (GUI.Toggle (new Rect (x, y, w, h), this.visibilityFilter == "SHOW_COMPLETED", "Show Completed")) {
			if (this.visibilityFilter != "SHOW_COMPLETED") {
				this.store.dispatch (TodoList.ActionCreators.setVisibilityFilter ("SHOW_COMPLETED"));
			}
		}

		y += h + ygap;
		x = 20;
		w = 100;
		h = 30;
		todoTitle = GUI.TextField (new Rect (x, y, w, h), todoTitle);

		x += w + ygap;
		w = 80;
		if (GUI.Button(new Rect(x, y, w, h), "Add TODO")) {
			if (todoTitle.Length > 0) {
				this.store.dispatch (TodoList.ActionCreators.addTodo(todoTitle));
				todoTitle = "";
			}
		}

		y += h + ygap;
		x = 20;
		w = 100;
		h = 20;
		for (int i=0, len=this.todos.Length; i<len; ++i) {
			var todo = this.getVisibleTodo( todos [i] );
			if (todo == null) {
				continue;
			}
			var check = GUI.Toggle (new Rect (x, y, w, h), todo.completed, todo.text);
			if ((todo.completed && !check) || (!todo.completed && check)) {
				this.store.dispatch (TodoList.ActionCreators.toggleTodo(todo.id));
			}
			y += h + ygap;
		}

		x = 20;
		w = 200;
		h = 20;
		GUI.Label (new Rect (x, y, w, h), "Timeline (Action: " +
			(this.timeline.monitoredStateIndex + 1) + "/" +
			this.timeline.monitoredStates.Count + ")");

		y += h + ygap;
		x = 300;
		w = 20;
		if (GUI.Button(new Rect(x, y, w, h), "<")) {
			var i = this.timeline.monitoredStateIndex - 1;
			this.store.dispatch (Redux.Devtools.ActionCreators.jumpToState(i));
		}
		x += w + xgap;
		if (GUI.Button(new Rect(x, y, w, h), ">")) {
			var i = this.timeline.monitoredStateIndex + 1;
			if (i < this.timeline.monitoredStates.Count) {
				this.store.dispatch (Redux.Devtools.ActionCreators.jumpToState(i));
			}
		}

		y += h + ygap;
		x = 20;
		w = 300;
		var ret = GUI.HorizontalSlider (new Rect(x, y, w, h),
			this.timeline.monitoredStateIndex,
			0, this.timeline.monitoredStates.Count - 1);

		if (ret != this.timeline.monitoredStateIndex &&
			ret < this.timeline.monitoredStates.Count) {
			this.store.dispatch (Redux.Devtools.ActionCreators.jumpToState(
				Mathf.RoundToInt(ret)
			));
		}
	}

	void onChangeState (Redux.Store store) {
		this.todos = (TodoList.Item[])store.getState (TodoList.Reducers.todos);
		this.visibilityFilter = (string)store.getState (TodoList.Reducers.visibilityFilter);
	}

	void onChangeMonitoredState (Redux.Devtools.Timeline timeline) {
		this.timeline = timeline;
	}
}
