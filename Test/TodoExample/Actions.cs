//using UnityEngine;
//using System.Collections;
//
//public partial class TodoExample : MonoBehaviour {
//	public static class Actions {
//		static int nextTodoId = 0;
//
//		public struct addTodo {
//			public int id;
//			public string text;
//			public addTodo (string text) {
//				this.id = nextTodoId++;
//				this.text = text;
//			}
//		}
//
//		public struct toggleTodo {
//			public int id;
//			public toggleTodo (int id) {
//				this.id = id;
//			}
//		}
//
//		public struct setVisibilityFilter {
//			public string filter;
//			public setVisibilityFilter (string filter) {
//				this.filter = filter;
//			}
//		}
//	}
//}