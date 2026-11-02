# REPORT.md - Individual Assignment

Keep this concise and specific to your implementation.

## 1. Initial analysis

**How are locations and connections represented in the baseline?**

- `WorldData` stores locations and connections in two separate arrays, exposed through `IReadOnlyList` properties. Its constructor copies the supplied collections into these arrays.

**Which operations appear frequent?**

- The supplied baseline workload performs one location lookup and one outgoing-connection query per iteration, for 5,000 iterations.

**Which operation(s) do you predict may scale poorly?**

- `FindLocationById` searches the locations sequentially until it finds a matching ID. Its worst-case time complexity is O(V), where V is the number of vertices (locations).

- `FindOutgoingConnections` scans the entire connections array and collects matching connections in a new list. Its time complexity is O(E), where E is the number of edges (connections) in the world graph. Each call also allocates a result list.

- These repeated scans are likely to become expensive as the world grows.

**What will you investigate first?**

- I will investigate these two queries first because both run 5,000 times in the baseline workload. Each query spends time checking entries that are unrelated to the requested location. Storing locations in a dictionary keyed by ID should allow average O(1) lookups. Grouping connections by their source location should let us retrieve the relevant list directly, avoiding a full scan and a new result list for every query. I expect this to reduce query time and allocations, at the cost of extra memory and time to prepare the data. I will measure these costs alongside the query improvements to see whether the changes benefit the workload overall.

## 2. Data representation

Describe important changes/retained choices, operations affected, and time/memory trade-offs.

## 3. Algorithms and adaptation

Explain your route finder at a useful high level and how the travel-budget query adapts the same ideas.

## 4. Complexity

| Operation | Your representation/algorithm | Complexity | Short explanation |
| --- | --- | --- | --- |
| Find location by ID | | | |
| Get outgoing connections | | | |
| Priority queue enqueue | | | |
| Priority queue dequeue-min | | | |
| Route finding | | | |

## 5. Correctness and testing

Important tests/edge cases and what they demonstrate.

## 6. Performance investigation

The starter restored and built successfully. Tests: 3 passed, 3 skipped, 0 failed.

Run from the repository root in Windows PowerShell using the default Debug configuration:

```powershell
dotnet run --project Standalone/src/FrontierDispatch.App -- --baseline
```

| Measurement | Baseline |
| --- | --- |
| Locations (V) | 2,500 |
| Connections (E) | 9,800 |
| Iterations | 5,000 |
| Elapsed query time | 445 ms |
| Checksum | 6,267,100 |

## 7. Development reflection

Describe one bug, wrong assumption, failed approach, or surprising result and how you investigated it.

## 8. Tools and AI assistance

Briefly describe relevant assistance, if any.

## 9. VG extension

Only if attempting VG: heuristic, traversal constraints, correctness, and Dijkstra/A* comparison.
