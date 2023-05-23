using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimulationController))]
public class SimulationEditor : Editor
{
    SimulationController simulation;

    public override void OnInspectorGUI()
    {
        SimulationController simulation = (SimulationController)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Initialize Tiles"))
        {
            // Create the child objects of the tile container, setting their positions, scales, etc.
            simulation.Initialize();
        }

        if (GUILayout.Button("Scramble Tiles"))
        {
            // Scramble the tiles, randomizing their positions.
            simulation.ScrambleTiles();
        }

        if (GUILayout.Button("Simulate Step"))
        {
            // Simulate just the next move of the sort.
            bool singleStep = true;
            simulation.Sort(singleStep);
        }

        if (GUILayout.Button("Simulate Full"))
        {
            // Simulate the entire sort.
            simulation.Sort();
        }

        if (GUILayout.Button("Delete Tiles"))
        {
            // Delete any children of the tile container object.
            simulation.DestroyChildren();
        }
    }
}
