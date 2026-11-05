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

The location index is built once and reused by queries. It provides average O(1) lookup at the cost of expected O(V) preparation time and O(V) extra storage. The dictionary references existing objects rather than copying them.

The constructor also builds `_outgoingByLocationId`, a private `Dictionary<int, List<Connection>>`. Each source location ID maps to a list of its outgoing connections. Connections are added in their original array order, preserving the order returned by the baseline.

The `WorldQueries.FindOutgoingConnections(int fromLocationId)` method now calls the matching method on `WorldData`. It retrieves the prepared list using `TryGetValue`, or returns `Array.Empty<Connection>()` when no connections match. The return type remains `IReadOnlyList<Connection>`. Queries reuse the lists instead of scanning all connections and allocating a new result list each time. Preparing this index takes expected O(E) time and extra storage for the lists and their connection references.

The `MinPriorityQueue<T>` stores a binary min-heap in a private `List<Entry>`. Each entry pairs an item with its integer priority, so moving an entry keeps the item and priority together. The root at index 0 contains an item with the lowest priority.

The position of an entry in the list is its index. For an entry at `index`, its parent is at `(index - 1) / 2` using integer division, provided the entry is not the root. Its left child is at `2 * index + 1`, and its right child is at `2 * index + 2`, if those positions exist in the list. Each parent has a priority less than or equal to the priorities of its children. This keeps the minimum at the root without sorting the entire list. Equal priorities are allowed without guaranteeing their removal order.

Enqueue appends an entry and sifts it upward. Dequeue replaces the root with the last entry, removes the last slot, and sifts the replacement down through the smaller child. The list provides indexed access without separate node objects and links, but its backing array may have unused capacity and must copy entries when it grows.

## 3. Algorithms and adaptation

Explain your route finder at a useful high level and how the travel-budget query adapts the same ideas.

## 4. Complexity

| Operation | Your representation/algorithm | Complexity | Short explanation |
| --- | --- | --- | --- |
| `FindLocationById(int id)` | Dictionary keyed by ID | Average O(1), worst-case O(V) | Hash lookup replaces the sequential scan. In the worst case, hash collisions can require checking up to V entries in the same bucket. |
| `FindOutgoingConnections(int fromLocationId)` | Dictionary of lists keyed by source ID | Average O(1) retrieval; O(k) iteration | Returns the prepared list directly. Iterating it visits its k connections. Worst-case lookup is O(V) if hash collisions require checking all source entries. |
| Priority queue enqueue | Append and sift up in a binary heap | Amortized O(log n); O(n) for an insertion that grows the list | Sifting visits at most the heap height. List growth copies existing entries. |
| Priority queue dequeue-min | Replace root with last entry and sift down | O(log n) | Removing the last slot is O(1); sifting follows one path down the heap. |
| Priority queue Peek / Count | Read root / list count | O(1) | Neither operation traverses the heap. |
| Route finding | | | |

Preparing the location index takes expected O(V) time and O(V) additional storage. Preparing the outgoing index takes expected O(E) time and O(V + E) additional storage for valid worlds: at most V source entries plus E connection references. Together, the indexes take expected O(V + E) preparation time and O(V + E) extra storage.

For the priority queue, n is the number of stored entries. The heap uses O(n) storage as it grows; the list retains capacity after removals. Each sift uses O(1) extra space.

## 5. Correctness and testing

The existing tests in `WorldQueriesTests.cs` check that `FindLocationById` returns the matching location for a nonconsecutive ID and returns `null` for a missing ID. They also check that `FindOutgoingConnections` returns only connections with the requested origin.

Added tests cover the following cases:

- `FindLocationById_EmptyWorld_ReturnsNull`: looking up an ID in an empty world returns `null`.
- `WorldData_DuplicateLocationId_ThrowsArgumentException`: constructing `WorldData` with duplicate location IDs throws an `ArgumentException`.
- `FindOutgoingConnections_UnknownSource_ReturnsEmpty`: an unknown source ID returns an empty collection.
- `FindOutgoingConnections_LocationWithNoOutgoing_ReturnsEmpty`: an existing location without outgoing connections returns an empty collection.
- `FindOutgoingConnections_MixedSources_PreservesMatchingOrder`: connections from other sources are excluded, and matching connections retain their original order.

Before Phase 3, the test suite produced **8 passed, 3 skipped, 0 failed**.

For Phase 3, I enabled the priority-queue starter test and added `MinPriorityQueueTests.cs`:

- `EmptyQueue_PeekAndDequeueThrow`: `Peek` and `Dequeue` throw `InvalidOperationException` for an empty queue, and `Count` remains zero.
- `SingleItem_CanBeRemovedAndQueueReused`: repeated insertion and removal works after the queue becomes empty, and `Peek` leaves `Count` unchanged.
- `DifferentInsertionOrders_DequeueInPriorityOrder`: ascending, descending, and mixed priorities are removed in ascending order, with `Count` checked after each removal.
- `MixedOperations_PreserveMinimumAndDuplicateItems`: insertions and removals preserve the minimum, including negative priorities. Repeated `Peek` calls preserve `Count`, and distinct items with equal priorities are both returned without requiring a tie order.

Running `dotnet test FrontierDispatch.sln` from the `Standalone` directory after these additions produced **15 passed, 2 skipped, 0 failed**. Only the Phase 4 and Phase 5 starter tests remain skipped.

## 6. Performance investigation

The starter restored and built successfully. Tests: 3 passed, 3 skipped, 0 failed.

Run from the repository root in Windows PowerShell using the default Debug configuration:

```powershell
dotnet run --project Standalone/src/FrontierDispatch.App -- --baseline
```

| Measurement | Original baseline | Location index only | Both indexes |
| --- | --- | --- | --- |
| Locations (V) | 2,500 | 2,500 | 2,500 |
| Connections (E) | 9,800 | 9,800 | 9,800 |
| Iterations | 5,000 | 5,000 | 5,000 |
| Elapsed query time | 445 ms | 403 ms | 0 ms reported (less than 1 ms) |
| Checksum | 6,267,100 | 6,267,100 | 6,267,100 |

With only the location index, the run took 403 ms while outgoing queries still scanned all connections. With both indexes, the run reported 0 ms. The application prints `Stopwatch.ElapsedMilliseconds` in whole milliseconds, so this means the measured interval was below 1 ms, not that the queries took no time. The supplied timer was unchanged.

Previously, the 5,000 outgoing queries checked 49 million connections in total. They now retrieve prepared lists, and the workload only reads each list's `Count`. The checksum remained unchanged. These are individual runs rather than a reliable estimate of the speedup. The timer excludes world construction; index preparation time and allocated bytes have not yet been measured.

## 7. Development reflection

I was surprised when the workload reported 0 ms after adding the outgoing-connection index. I expected the queries to become faster, but the size of the drop made me question whether the measurement was correct.

I first reviewed my changes and the timing code in `Program.cs`. When I could not explain the result, I asked AI to guide me with questions so I could work through the reasoning myself. The workload previously checked 49 million connections. With the prepared indexes, it instead performs dictionary lookups and reads each outgoing list's `Count`.

I also confirmed that the timer reports whole milliseconds and excludes index construction. A result of 0 ms therefore means the measured query time was below 1 ms, not that the queries took no time. This helped me understand why i need to consider both what a workload measures and how it displays the project results in the future.

While writing the priority-queue tests, I searched for a way to run multiple test cases through one test method. I found the [Adding more features section of Microsoft's NUnit tutorial](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-csharp-with-nunit#adding-more-features), which introduced me to NUnit's `[TestCase]` attribute. This made it possible for me to write `DifferentInsertionOrders_DequeueInPriorityOrder` using the same test method for ascending, descending, and mixed insertion orders. Each attribute supplies a different array to the method, while the assertions stay the same. This let me check several insertion patterns without duplicating the test code.

## 8. Tools and AI assistance

Briefly describe relevant assistance, if any.

- After my changes, I noticed the baseline time dropped from 445 ms to 0 ms. I used AI to help investigate this unexpected drop, which guided me to discover that reusing the outgoing lists eliminated 49 million connection checks, leaving dictionary lookups and reads of each outgoing list's `Count`. Since index construction happens outside the timer and we use `sw.ElapsedMilliseconds` to display the result, this explains why the measured time dropped below 1 ms and why it displayed as 0 ms.

- I have also started to use AI to review my report and give feedback and comments on what I had written like a "critic", including suggestions for improvement and things to consider in future work when documenting my work.

## 9. VG extension

Only if attempting VG: heuristic, traversal constraints, correctness, and Dijkstra/A* comparison.
