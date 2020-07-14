using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		MapGenerator mg = (MapGenerator)target;

		GUILayout.BeginHorizontal();

		GUILayout.Label("Seed: ");
		mg.seedCE = GUILayout.TextField(mg.seedCE);

		GUILayout.Label("Round: ");
		mg.roundCE = GUILayout.TextField(mg.roundCE);

		GUILayout.EndHorizontal();

		if (GUILayout.Button("Refresh map"))
		{
			mg.MapGeneratorContructor(mg.seedCE, Int16.Parse(mg.roundCE));
		}
	}
}
