# Assignment Core

This folder contains the main algorithmic components that you will implement and adapt during the individual assignment.

The project is intentionally runnable before these classes are complete.

Do not treat every `TODO` as something that must be solved immediately. Follow the assignment phases and establish the supplied baseline first.

## Files

### `MinPriorityQueue.cs`

A deliberately incomplete minimum-priority queue.

Its public operations represent the behavior needed later by the route-finding code:

```text
Enqueue(item, priority)
Peek()
Dequeue()
Count
```

For the assignment, this is intended to become your own **binary min-heap priority queue**.

The important requirement is not only that it returns the correct item, but that the representation and operations have the intended complexity.

---

### `DijkstraRouteFinder.cs`

A deliberately incomplete route finder.

Its job is to find a minimum-cost route through the Frontier Dispatch road network.

It should eventually use:

- the world representation you have chosen/prepared;
- your priority queue;
- Dijkstra's shortest-path reasoning.

Do not start here before you have examined the baseline world representation and completed the earlier assignment phases.

---

### `ReachabilityQuery.cs`

A deliberately incomplete bounded-reachability query.

Instead of asking:

> What is the cheapest route to one destination?

this query asks conceptually:

> Which locations can be reached without exceeding a maximum travel-cost budget?

This is intended to make you **adapt** shortest-path reasoning rather than simply copy the route finder.

---

### `RouteResult.cs`

Provided support code representing the result of a route search.

This file is not intended to be a major implementation task.

Read it so that you understand what a successful route finder is expected to return.

---

## Related Core files

The assignment classes operate on the existing Frontier Dispatch world model under:

```text
Assets/DSA/Core/
```

In particular, inspect:

```text
WorldData.cs
WorldQueries.cs
WorldDatasetFactory.cs
Location.cs
Connection.cs
```

`WorldQueries.cs` represents the straightforward baseline implementation you should analyze before optimizing or replacing its repeated work.

`WorldDatasetFactory.cs` provides deterministic worlds used for testing and performance measurement.

---

## Tests

Relevant tests are under:

```text
Standalone/tests/FrontierDispatch.Tests/
```

The starter contains:

```text
WorldQueriesTests.cs
AssignmentStarterTests.cs
```

The existing baseline tests should pass immediately.

Some assignment-contract tests are initially skipped. Enable the relevant test when you reach that assignment phase, and add your own meaningful tests rather than relying only on the supplied ones.

---

## One source of truth

The code in this folder is compiled both by:

```text
Unity
```

and by:

```text
Standalone/FrontierDispatch.sln
```

There is not a separate Unity implementation and standalone implementation of these algorithms.

Normally, implement and test the algorithmic work through the standalone solution. Use Unity when the visualization helps you understand or demonstrate the resulting behavior.