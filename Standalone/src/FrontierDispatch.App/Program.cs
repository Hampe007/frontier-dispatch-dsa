using System;
using System.Diagnostics;
using FrontierDispatch.Core;

if (args.Length > 0 && args[0] == "--baseline")
{
    RunBaseline();
    return;
}

// Small sanity-check application.
// This is separate from the larger performance workload below.
WorldData world = SampleWorldFactory.CreateSmall();
var queries = new WorldQueries(world);

Console.WriteLine("Frontier Dispatch - assignment starter");
Console.WriteLine($"Small demo locations: {world.Locations.Count}");
Console.WriteLine($"Find 734: {queries.FindLocationById(734)?.Name ?? "missing"}");
Console.WriteLine();
Console.WriteLine("Run with --baseline to record the supplied baseline workload.");

static void RunBaseline()
{
    // Build the dataset before timing the repeated runtime queries.
    // Measure world preparation separately from query work.
    
    var sw1 = Stopwatch.StartNew();
    
    long allocatedBeforeWorldCreation = GC.GetAllocatedBytesForCurrentThread();
    
    WorldData world = WorldDatasetFactory.CreateMedium();
    var queries = new WorldQueries(world);
    
    long allocatedAfterWorldCreation = GC.GetAllocatedBytesForCurrentThread();
    
    sw1.Stop();
    
    long allocatedBytesWorldCreation = allocatedAfterWorldCreation - allocatedBeforeWorldCreation;
    
    // The checksum provides a stable sanity check that the workload still
    // produces equivalent results after implementation changes.
    int checksum = 0;

    const int iterations = 5000;

    var sw2 = Stopwatch.StartNew();

    long allocatedBeforeWorldQuery = GC.GetAllocatedBytesForCurrentThread();
    
    for (int i = 0; i < iterations; ++i)
    {
        // Deterministically visit many different location IDs rather than
        // repeatedly querying the same location.
        int id = (i * 997) % world.Locations.Count;

        Location? location = queries.FindLocationById(id);
        checksum += location?.Id ?? 0;
        checksum += queries.FindOutgoingConnections(id).Count;
    }

    long allocatedAfterWorldQuery = GC.GetAllocatedBytesForCurrentThread();

    sw2.Stop();

    long allocatedBytesWorldQuery = allocatedAfterWorldQuery - allocatedBeforeWorldQuery;
    
    Console.WriteLine("Baseline world-query workload");
    Console.WriteLine($"Locations: {world.Locations.Count}");
    Console.WriteLine($"Connections: {world.Connections.Count}");
    Console.WriteLine($"Iterations: {iterations}");
    Console.WriteLine($"World preparation: {sw1.Elapsed.TotalMilliseconds:F3} ms");
    Console.WriteLine($"Preparation allocations: {allocatedBytesWorldCreation} bytes");
    Console.WriteLine($"Query time: {sw2.Elapsed.TotalMilliseconds:F3} ms");
    Console.WriteLine($"Query allocations: {allocatedBytesWorldQuery} bytes");
    Console.WriteLine($"Checksum: {checksum}");
    Console.WriteLine("Absolute time varies by machine. Keep this result for before/after comparison.");
}
