# REPORT.md - Individual Assignment

## 1. Initial analysis

- **How are locations and connections represented in the baseline?**

  `WorldData` stores locations in an array and connections in another. Each connection has a starting location, an ending location, and a travel cost.

- **Which operations appear frequent?**

  Each of the 5,000 benchmark iterations looks up a location by ID and gets its outgoing connections.

- **Which operation(s) do you predict may scale poorly?**

  Looking up a location can require checking every location. Getting outgoing connections checks every connection and creates a new list every time. Both queries do more work as the world grows.

- **What will you investigate first?**

  I would start with the outgoing query because it scans every connection even when the caller only needs the count. Grouping connections by their starting location should make repeated queries cheaper.

## 2. Data representation

I kept both arrays so the program can still go through every location or connection. I added one dictionary that maps each location ID to its location and another that maps each source ID to its outgoing connections. The outgoing lists keep their original order. A missing location gives `null`, and a source with no outgoing connections gives an empty list.

I build both dictionaries when the world is created. This takes extra time and memory up front. The world queries, route finder, and movement range query can then reuse the same data.

The priority queue stores items and their priorities in a list arranged as a binary min-heap. Each parent has a priority less than or equal to its children's. When the heap has items, the root holds an item with the smallest priority. When I add an item, it moves up until the rule holds. When I remove the root, the last item takes its place and moves down as needed. Items can share a priority and can come out in either order.

## 3. Algorithms and adaptation

Dijkstra starts by putting the start location in the heap at cost 0. The heap gives me the next location with the lowest queued cost. For each outgoing connection, I add its travel cost to the cost of getting to the current location. If this gives a cheaper route to the next location, I save the new cost and the previous location, then add a new heap entry.

Travel costs are non-negative, so the first time a location comes out of the heap, its cheapest cost is known. A visited set lets me skip later entries for that location and count it only once. When I reach the goal, I follow the saved locations back to the start and reverse the path. If the start is the goal, the path contains just the start and costs 0. If the heap empties first, there is no route. Though, its important to mention that the route finder assumes valid location IDs and travel totals that fit in an `int`.

The movement range query uses the same heap and cost tracking. The goal is to return every location within the budget along with its cheapest cost. It does not need to save the path. A connection fits if its cost is less than or equal to the remaining budget. A missing start or a negative budget gives an empty result. If a connection is too expensive, the query skips it and checks the remaining connections.

## 4. Complexity

V is the number of locations, E is the number of connections, and n is the number of items in the heap. Expected times assume dictionary and set lookups take constant time on average.

| Operation / workload | Before | After | Expected complexity | Reason / interpretation |
| --- | --- | --- | --- | --- |
| Find a location by ID | Scan up to all V locations | Look up the ID in a dictionary | Expected O(1) | With well distributed hashes, the dictionary usually checks only a few entries. Many keys with the same hash can make a lookup take longer. |
| Get outgoing connections | Scan all E connections and build a list | Look up a prepared list by source ID | Expected O(1) to get the list | One dictionary lookup returns the prepared list. Hash collisions can add comparisons, and reading the entire list takes time proportional to the number of outgoing connections from that location. |
| Add to the priority queue | Starter method left incomplete | Append and move the item up the heap | O(log n) when the list has room | A binary heap has about log n levels. The new item follows at most one path up and can stop sooner if it is already in order. If the list grows, copying its items can make the insertion take O(n). |
| Remove the minimum | Starter method left incomplete | Replace the root and move the replacement down | O(log n) | Removing the last list entry takes constant time. The replacement follows at most one path down the heap, which has about log n levels. With one item, there is no need to move anything down. |
| Peek at the next item | Starter method left incomplete | Check the count and read the root | O(1) when the queue has items | Peek checks the stored count and reads the first list entry, so the work stays the same as the queue grows. If the queue is empty, it throws after checking the count. |
| Get the queue count | Use the list count | Use the list count | O(1) | The list updates its stored count as items are added or removed. Reading it takes the same amount of work regardless of how many items are in the list. |
| Find a route | Starter method left incomplete | Run Dijkstra with a binary min-heap | Expected O((V + E) log V) on the supplied grids | The search checks each outgoing connection at most once and can add a heap entry when it finds a cheaper path. With at most four outgoing connections per location on these grids, the heap grows at most in proportion to V, so each heap operation takes up to O(log V) time. Reaching the goal early can cut the search short. |
| Find locations within a budget | Starter method left incomplete | Run Dijkstra with a travel budget | Expected O((V + E) log V) on the supplied grids (same as the one above) | It uses the same heap and checks outgoing connections from each location it visits. A smaller budget can limit how many locations reach the heap. A large enough budget can let the search visit the entire grid. |

The indexes take expected O(V + E) time to build and use O(V + E) extra space. The searches also use up to O(V + E) extra space to keep track of locations and hold candidates in the heap.

## 5. Correctness and testing

I ran `dotnet test Standalone/FrontierDispatch.sln` and all 24 tests passed, with none skipped/ignored.

The starter package supplied world query tests and three tests for later phases. I enabled those three tests to check that the heap removes the lowest priority first, that a route from a location to itself costs 0, and that a zero budget includes the starting location.

Here are three of the tests I added.

| Test | Why I added it | Expected and observed result |
| --- | --- | --- |
| Cheaper route with more roads | Check that Dijkstra chooses a route by total travel cost. | I expected the two-road route costing 2 to beat the direct road costing 10. The route finder returned the two-road route with a total cost of 2. |
| Budget boundary | Check what happens when a road costs exactly as much as the budget. | I expected a road costing 3 to be excluded with a budget of 2 and included with a budget of 3. The query did both. |
| Heap insertion order | Check that the lowest priority comes out first regardless of insertion order. | I expected the first removal to return 1. It did for ascending, descending, and mixed inputs. |

## 6. Performance investigation

| Measurement | Starter | Completed version |
| --- | ---: | ---: |
| World preparation time, median of five runs | 1.613 ms | 2.292 ms |
| World preparation time, range | 1.461 to 1.688 ms | 1.986 to 2.400 ms |
| Preparation allocations per run | 1,093,072 bytes | 1,607,288 bytes |
| Repeated query time, median of five runs | 450.367 ms | 0.312 ms |
| Repeated query time, range | 441.974 to 450.808 ms | 0.308 to 0.458 ms |
| Query allocations per run | 440,000 bytes | 0 bytes |
| Checksum on every run | 6,267,100 | 6,267,100 |

I used the same measurement code in both versions and ran each version five times. I timed world preparation separately from the repeated queries so it was possible to see the cost of building the indexes and the time spent using them. I took the allocation reading before and after each part to be able to calculate the difference. The table shows the median along with the range of the five timings.

The completed version made the repeated queries faster in this benchmark. Median query time fell from 450.367 ms to 0.312 ms, and query allocations fell from 440 000 bytes to 0 bytes. The indexes let each query reuse the outgoing connections prepared when the world was built. Even the slowest query run with indexes was faster than the fastest starter run. Preparation showed the reverse pattern. Every run with indexes took longer to prepare than every starter run. The matching checksum shows that both versions produced the same combined total from the location IDs and outgoing connection counts.

That improvement came with a higher preparation cost. Median preparation time rose by 0.679 ms, and preparation allocated 514 216 more bytes. Even with fewer query allocations, the two measured parts together allocated 74 216 more bytes. The benchmark supports using the indexes when the same world is queried repeatedly. Its results cover world preparation and lookups.

A short scan may still be cheaper for a small world or only a few queries. But it may be more efficent if we would use larger worlds, but to confirm that, I would need to measure how preparation time and memory use grow.

## 7. Development reflection

I chose to redo the assignment scripts to see whether I could build them without relying on AI, and very much minimize googeling. I used what I had learned in the labs and during the assignment, discussed ideas with others (Simon primarly now during when I did the rework), and used Ctrl+click in Rider to read method definitions (that I were unsure of how they were setup). By rebuilding the scripts, it helped me check that I could apply and explain what I have learnt.

I found a reachability bug while writing the report. I meant to skip any road that was too expensive, but I used `return` inside the loop. That ended the whole search before it could check the other roads. The original budget test used only one road, so it passed either way. The fix uses `continue` instead to skip the expensive road and check the next one.

In an earlier version (the old code), I was surprised when the benchmark displayed 0 ms after I added the outgoing connection index. I checked `Program.cs` and found that the timer only covered the query loop and displayed whole milliseconds. The queries took less than 1 ms, and the index was built before the timer started. This made me more aware of what to check when a benchmark measures to more easily find the cause of the confusion.

## 8. Tools and AI assistance

I used AI to clarify the previous REPORT.md, from questioning me and helping me formulate notes better during my process which I had use for now when filling in this report (where the old one was before I emptied the file). 

My old code was written by me, but then AI was asked to poke holes through my line of thinking and question me into making the code better which helped me get a better grip of some of the theories discussed during the DSA course. But this caused me to in the end do what I stated in previous segment of choosing to redo it without AI.  

When the earlier benchmark (old code) displayed 0 ms, I asked AI to guide me with questions when I got lost, as I started to doubt my own code before thinking that the collecting of stats in the baseline was what caused it. So AI helped me by telling me just some things to draw up and left me to trace it from there to figure out what went wrong which lead me to cause previously mentioned.

## 9. VG extension

Not applicable, did not attempt VG due to time constraints :)
