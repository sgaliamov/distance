# Distance

Locations at a given distance.

## Requirements

* The system must be written in C#.
* It must be possible to set a maximum distance.
* It must be possible to set a maximum number of results.
* Results should be ordered by distance.
* The system should not slow down significantly if the number of locations increases.

## Roadmap

- [x] research, find algorithm: <https://en.wikipedia.org/wiki/Spatial_database#Spatial_index>
- [x] check sql server capabilities
- [ ] create repo and data structures
- [ ] create BLL
- [ ] try spatial index
- [ ] create cli
- [ ] create webapi
- [ ] create web client?

## Reasoning

- why sql server?
  - tested solution
  - maintanability
  - integration with other data
- <http://epsg.io/4326>
- <https://www.movable-type.co.uk/scripts/latlong.html>