# Distance

Solution contains implementation of [k-d tree](https://en.wikipedia.org/wiki/K-d_tree) to find k nearest neighbors on Earth surface.

For reference it also contains [brute force](./distance/BruteForce/LocationsBruteRepository.cs) version and implementation based on [MS SQL Server Spatial Index](./distance/Sql/LocationsSqlRepository.cs).

## Benchmarks

``` ini
BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17134.472 (1803/April2018Update/Redstone4)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2531256 Hz, Resolution=395.0608 ns, Timer=TSC
.NET Core SDK=2.2.101
  [Host]     : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
```

### 50000 points

|     Method |          Mean |       Error |      StdDev |        Median |    Ratio | RatioSD | Rank |
|----------- |--------------:|------------:|------------:|--------------:|---------:|--------:|-----:|
|     KdTree |      2.743 ms |   0.0536 ms |   0.0910 ms |      2.726 ms |     1.00 |    0.00 |    1 |
| BruteForce |     16.029 ms |   0.3157 ms |   0.4527 ms |     15.966 ms |     5.82 |    0.27 |    2 |
|  SqlServer | 18,720.957 ms | 373.7142 ms | 771.7846 ms | 18,660.128 ms | 6,888.28 |  329.33 |    3 |

### 100000 points

|     Method |         Mean |      Error |     StdDev |       Median |  Ratio | RatioSD | Rank |
|----------- |-------------:|-----------:|-----------:|-------------:|-------:|--------:|-----:|
|     KdTree |     4.705 ms |  0.0934 ms |  0.1777 ms |     4.699 ms |   1.00 |    0.00 |    1 |
| BruteForce |    33.926 ms |  0.6739 ms |  1.7987 ms |    33.806 ms |   7.21 |    0.46 |    2 |
|  SqlServer | 1,524.647 ms | 30.2649 ms | 56.0980 ms | 1,526.701 ms | 324.56 |   16.41 |    3 |

### 1000000 points

|     Method |        Mean |      Error |     StdDev |      Median | Ratio | RatioSD | Rank |
|----------- |------------:|-----------:|-----------:|------------:|------:|--------:|-----:|
|     KdTree |    36.91 ms |   3.069 ms |   9.050 ms |    33.44 ms |  1.00 |    0.00 |    1 |
| BruteForce |   275.65 ms |   5.450 ms |   8.323 ms |   274.83 ms |  7.76 |    1.79 |    2 |
|  SqlServer | 2,432.42 ms | 136.778 ms | 401.146 ms | 2,398.43 ms | 70.35 |   21.49 |    3 |


## Links

* <https://en.wikipedia.org/wiki/Spatial_database#Spatial_index>
* <http://epsg.io/4326>
* <https://www.movable-type.co.uk/scripts/latlong.html>
* <https://www.youtube.com/watch?v=E1_WCdUAtyE>
