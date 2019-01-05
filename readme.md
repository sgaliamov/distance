# Distance

Solution contains implementation of [k-d tree](https://en.wikipedia.org/wiki/K-d_tree) to find k nearest neighbors on Earth surface.

For reference it contains [brute force](./distance/BruteForce/LocationsBruteRepository.cs) implementation and implementation based on [MS SQL Server Spatial Index](./distance/Sql/LocationsSqlRepository.cs).

## Benchmark

``` ini
BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.472 (1803/April2018Update/Redstone4)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2531256 Hz, Resolution=395.0608 ns, Timer=TSC
.NET Core SDK=2.2.101
  [Host]     : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT

50000 points
```

|     Method |          Mean |       Error |        StdDev |        Median | Ratio | Rank |
|----------- |--------------:|------------:|--------------:|--------------:|------:|-----:|
|   KdTree |      2.731 ms |   0.0699 ms |     0.0653 ms |      2.711 ms | 0.000 |    1 |
|  SqlServer | 19,529.218 ms | 432.9697 ms | 1,163.1449 ms | 19,381.186 ms | 1.000 |    3 |
| BruteForce |     17.587 ms |   0.3512 ms |     0.7095 ms |     17.492 ms | 0.001 |    2 |

## Links

* <https://en.wikipedia.org/wiki/Spatial_database#Spatial_index>
* <http://epsg.io/4326>
* <https://www.movable-type.co.uk/scripts/latlong.html>
* <https://www.youtube.com/watch?v=E1_WCdUAtyE>
