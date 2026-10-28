# Frontier Dispatch — Individual Assignment Starter

This is the frozen assignment-start version of the Frontier Dispatch course project.

The detailed assignment specification in Omniway is authoritative for requirements and grading. This file explains how the starter is structured and what to do first.

## One Core, two ways to run it

The assessed source lives under:

```text
Assets/DSA/Core/
```

That code has no `UnityEngine` dependency. The standalone .NET project compiles the **same source files** through:

```text
Standalone/FrontierDispatch.sln
```

This gives you two useful environments:

- **Standalone .NET:** build, run tests, measure the baseline workload, and work on the algorithms without opening Unity.
- **Unity:** visualize and apply the same Core data in a game context.

You are not required to write Unity-dependent code for G or VG unless the assignment specification explicitly says otherwise.

## Important files

Baseline world/data code:

```text
Assets/DSA/Core/WorldData.cs
Assets/DSA/Core/WorldQueries.cs
Assets/DSA/Core/WorldDatasetFactory.cs
```

Assignment contracts:

```text
Assets/DSA/Core/Assignment/MinPriorityQueue.cs
Assets/DSA/Core/Assignment/DijkstraRouteFinder.cs
Assets/DSA/Core/Assignment/ReachabilityQuery.cs
Assets/DSA/Core/Assignment/RouteResult.cs
```

Tests:

```text
Standalone/tests/FrontierDispatch.Tests/WorldQueriesTests.cs
Standalone/tests/FrontierDispatch.Tests/AssignmentStarterTests.cs
```

Report template:

```text
REPORT.md
```

Unity visualization:

```text
Assets/DSA/Scenes/Assignment_WorldMap.unity
```

The World Map scene visualizes the baseline world and its current queries. It is **not** a completed pathfinding demo.

## First task: establish the baseline

Before implementing assignment algorithms, run the supplied project as-is.

From the repository root:

```bash
dotnet restore Standalone/FrontierDispatch.sln
dotnet build Standalone/FrontierDispatch.sln
dotnet test Standalone/FrontierDispatch.sln
dotnet run --project Standalone/src/FrontierDispatch.App -- --baseline
```

The untouched starter is intended to have:

- a successful restore/build;
- the existing world-query tests passing;
- the later-phase assignment-contract tests skipped/ignored;
- a working baseline workload using the straightforward world representation.

Record the baseline result. Absolute timing differs between machines; later comparisons should be made on the same machine under reasonably similar conditions.

## Unity

Open the repository root in **Unity 6.3 LTS** and let Unity finish importing.

The project is distributed as an already initialized Unity project. You should **not** need to create a new Unity project, copy `Assets`, or run the course scene builder.

The Unity layer is optional for the assessed algorithmic implementation. If Unity setup is blocking you, continue with the standalone solution and ask for help with the editor setup separately.

## Assignment phases represented in the starter

`MinPriorityQueue<T>` is intentionally incomplete and is intended to become your binary min-heap priority queue.

`DijkstraRouteFinder` is intentionally incomplete and is intended to use your prepared world representation and priority queue.

`ReachabilityQuery` is intentionally incomplete and is intended to adapt shortest-path reasoning to a maximum travel-cost budget.

The tests in `AssignmentStarterTests.cs` document a few later contracts. They are initially ignored so that the untouched starter remains green. Enable the relevant tests when you reach those phases, and add your own meaningful tests as required by the assignment.

## Recommended starting order

1. Verify the untouched starter.
2. Read `WorldData.cs` and `WorldQueries.cs`.
3. Run and record the supplied baseline workload.
4. Begin `REPORT.md` with your initial analysis.
5. Identify repeated operations and representation bottlenecks.
6. Only then move into the later implementation phases from the assignment specification.

Do not jump directly to Dijkstra before understanding and measuring the baseline.
