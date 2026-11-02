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

The `WorldData` constructor builds a private `Dictionary<int, Location>` named `_locationsById`. Each key is a location ID, and its value references the corresponding `Location` object. The original location and connection arrays remain available for iteration.

The `WorldQueries.FindLocationById(int id)` method now calls `WorldData.FindLocationById(int id)`. That method searches the prepared dictionary using `TryGetValue` and returns the matching `Location`, or `null` if the ID is missing. Calling `Add` while building the dictionary rejects duplicate IDs by throwing an `ArgumentException`.

The index is built once and reused by queries. It provides average O(1) lookup at the cost of expected O(V) preparation time and O(V) extra storage. The dictionary references existing objects rather than copying them. The `WorldQueries.FindOutgoingConnections(int fromLocationId)` method still scans the connections array.

## 3. Algorithms and adaptation

Explain your route finder at a useful high level and how the travel-budget query adapts the same ideas.

## 4. Complexity

| Operation | Your representation/algorithm | Complexity | Short explanation |
| --- | --- | --- | --- |
| `FindLocationById(int id)` | Dictionary keyed by ID | Average O(1), worst-case O(V) | Hash lookup replaces the sequential scan. In the worst case, hash collisions can require checking up to V entries in the same bucket. |
| `FindOutgoingConnections(int fromLocationId)` | Scan connections array | O(E) | Checks every connection and builds a result list. |
| Priority queue enqueue | | | |
| Priority queue dequeue-min | | | |
| Route finding | | | |

Preparing the location index takes expected O(V) time and O(V) additional storage.

## 5. Correctness and testing

The existing tests in `WorldQueriesTests.cs` check that `FindLocationById` returns the matching location for a nonconsecutive ID and returns `null` for a missing ID. They also check that `FindOutgoingConnections` returns only connections with the requested origin.

Added tests cover two edge cases:

- `FindLocationById_EmptyWorld_ReturnsNull`: looking up an ID in an empty world returns `null`.
- `WorldData_DuplicateLocationId_ThrowsArgumentException`: constructing `WorldData` with duplicate location IDs throws an `ArgumentException`.

Running `dotnet test FrontierDispatch.sln` from the `Standalone` directory produced **5 passed, 3 skipped, 0 failed**. The tests for Phases 3-5 remain skipped.

## 6. Performance investigation

The starter restored and built successfully. Tests: 3 passed, 3 skipped, 0 failed.

Run from the repository root in Windows PowerShell using the default Debug configuration:

```powershell
dotnet run --project Standalone/src/FrontierDispatch.App -- --baseline
```

| Measurement | Original baseline | Location index |
| --- | --- | --- |
| Locations (V) | 2,500 | 2,500 |
| Connections (E) | 9,800 | 9,800 |
| Iterations | 5,000 | 5,000 |
| Elapsed query time | 445 ms | 403 ms |
| Checksum | 6,267,100 | 6,267,100 |

The location-index run was about 9.4% faster than the original baseline, with the same checksum. This is an intermediate comparison from individual runs, so it does not establish a reliable speedup. Outgoing-connection queries still scan all connections. The timer excludes world construction, so dictionary preparation time and such have not yet been measured.

## 7. Development reflection

Describe one bug, wrong assumption, failed approach, or surprising result and how you investigated it.

## 8. Tools and AI assistance

Briefly describe relevant assistance, if any.

## 9. VG extension

Only if attempting VG: heuristic, traversal constraints, correctness, and Dijkstra/A* comparison.
