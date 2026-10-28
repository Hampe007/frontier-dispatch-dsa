#nullable enable

using System.Collections.Generic;
using FrontierDispatch.Core;
using UnityEngine;

namespace FrontierDispatch.Unity
{
    public sealed class StackQueueDemoController : MonoBehaviour
    {
        private readonly Stack<Vector3> _undo = new Stack<Vector3>();
        private readonly Queue<DispatchJob> _jobs = new Queue<DispatchJob>();
        private GameObject? _courier;
        private GameObject? _jobMarker;
        private int _nextJobId = 1;
        private string _lastAction = "Ready.";

        private void Start()
        {
            _courier = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _courier.name = "Courier";
            _courier.transform.position = new Vector3(-3f, 0f, 0f);
            _courier.transform.localScale = Vector3.one * 0.8f;

            _jobMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            _jobMarker.name = "CurrentJob";
            _jobMarker.transform.position = new Vector3(3f, 0f, 0f);
            _jobMarker.transform.localScale = Vector3.one * 0.65f;
        }

        private void MoveCourier(float delta)
        {
            if (_courier == null) return;
            _undo.Push(_courier.transform.position);
            _courier.transform.position += new Vector3(delta, 0f, 0f);
            _lastAction = $"Move: pushed previous position. Stack count = {_undo.Count}.";
        }

        private void Undo()
        {
            if (_courier == null || _undo.Count == 0)
            {
                _lastAction = "Undo stack is empty.";
                return;
            }

            _courier.transform.position = _undo.Pop();
            _lastAction = $"Undo: popped most recent position. Stack count = {_undo.Count}.";
        }

        private void AddJob()
        {
            int id = _nextJobId++;
            _jobs.Enqueue(new DispatchJob(id, $"Job {id}"));
            _lastAction = $"Enqueued Job {id}. Queue count = {_jobs.Count}.";
        }

        private void ProcessJob()
        {
            if (_jobs.Count == 0)
            {
                _lastAction = "Dispatch queue is empty.";
                return;
            }

            DispatchJob job = _jobs.Dequeue();
            if (_jobMarker != null)
            {
                _jobMarker.transform.position = new Vector3(3f, 0f, job.Id % 5 - 2f);
            }
            _lastAction = $"Processed {job.Description}. Queue count = {_jobs.Count}.";
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(20f, 20f, 440f, 330f), GUI.skin.box);
            GUILayout.Label("STACK - courier undo history (LIFO)");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Move left")) MoveCourier(-1f);
            if (GUILayout.Button("Move right")) MoveCourier(1f);
            if (GUILayout.Button("Undo")) Undo();
            GUILayout.EndHorizontal();
            GUILayout.Label($"Undo count: {_undo.Count}");

            GUILayout.Space(18f);
            GUILayout.Label("QUEUE - dispatch jobs (FIFO)");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Enqueue job")) AddJob();
            if (GUILayout.Button("Process next")) ProcessJob();
            GUILayout.EndHorizontal();
            GUILayout.Label($"Queued jobs: {_jobs.Count}");

            GUILayout.Space(18f);
            GUILayout.Label(_lastAction);
            GUILayout.Space(8f);
            GUILayout.Label("The demo uses standard Stack<T>/Queue<T>. Lab 2 implements simplified versions to expose the representation.");
            GUILayout.EndArea();
        }
    }
}
