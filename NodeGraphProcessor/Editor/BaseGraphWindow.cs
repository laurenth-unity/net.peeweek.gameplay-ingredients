﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;
using UnityEngine.Experimental.UIElements;
using UnityEditor.Experimental.UIElements;
using UnityEditor.Experimental.UIElements.GraphView;

namespace GraphProcessor
{
	[System.Serializable]
	public abstract class BaseGraphWindow : EditorWindow
	{
		protected VisualElement		rootView;
		protected BaseGraphView		graphView;

		[SerializeField]
		protected BaseGraph			graph;

		public bool					isGraphLoaded
		{
			get { return graphView != null && graphView.graph != null; }
		}

		protected void OnEnable()
		{
			InitializeRootView();

			if (graph != null)
				InitializeGraph(graph);
		}

		protected void OnDisable()
		{
			if (graph != null)
				graphView.SaveGraphToDisk();
		}

		void InitializeRootView()
		{
			rootView = this.GetRootVisualContainer();

			rootView.name = "graphRootView";

			rootView.AddStyleSheetPath("GraphProcessorStyles/BaseGraphView");
		}

		public void InitializeGraph(BaseGraph graph)
		{
			this.graph = graph;

			if (graphView != null)
				rootView.Remove(graphView);

			//Initialize will provide the BaseGraphView
			Initialize(graph);

			graphView = rootView.Children().FirstOrDefault(e => e is BaseGraphView) as BaseGraphView;

			if (graphView == null)
			{
				Debug.LogError("GraphView has not been added to the BaseGraph root view !");
				return ;
			}

			graphView.Initialize(graph);
		}

		public virtual void OnGraphDeleted()
		{
			if (graph != null)
				rootView.Remove(graphView);

			graphView = null;
		}

		protected abstract void	Initialize(BaseGraph graph);
	}
}