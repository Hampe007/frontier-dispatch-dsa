# Frontier Dispatch — Individual Assignment Starter

This repository is the frozen starter project for the individual assignment in **Data Structures and Algorithms**.

Start with **ASSIGNMENT_README.md** and the assignment specification provided in Omniway.

## Important

- The assessed algorithmic code lives under `Assets/DSA/Core/`.
- The same Core source files are compiled by `Standalone/FrontierDispatch.sln`.
- Unity is provided as an application/visualization layer; Unity-dependent implementation is not required for G or VG unless the assignment specification explicitly says otherwise.
- Do **not** run `Tools > DSA > Build Course Scenes`. The distributed project already contains the course scenes.
- Do **not** begin by implementing Dijkstra. First establish and record the supplied baseline.

## First verification

From the repository root:

```bash
dotnet restore Standalone/FrontierDispatch.sln
dotnet build Standalone/FrontierDispatch.sln
dotnet test Standalone/FrontierDispatch.sln
dotnet run --project Standalone/src/FrontierDispatch.App -- --baseline
```

The untouched starter should build successfully. The existing world-query tests should pass, while later assignment-contract tests remain skipped/ignored until their corresponding phases.

For Unity, open the **repository root** as a Unity 6.3 LTS project and let the initial import finish. The project already contains the scenes and metadata required for normal use.
