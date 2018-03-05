RTree
=====

RTree is a high-performance .NET library for 2D **spatial indexing** of points and rectangles.
It's based on an optimized **R-tree** data structure with **bulk insertion** support.
Compatible with Unity and .Net 3.5

*Spatial index* is a special data structure for points and rectangles
that allows you to perform queries like "all items within this bounding box" very efficiently
(e.g. hundreds of times faster than looping over all items).
It's most commonly used in maps and data visualizations.

This code has been copied over from the C# [RBush](https://github.com/viceroypenguin/RBush) library.
which in turn was copied over from the Javascript [RBush](https://github.com/mourner/rbush) library.

## Usage

### Creating a Tree

First, define the data item class to implement `ISpatialData`, which requires that
the class expose the `Envelope` property.  Then the class can be used as such:

```csharp
var tree = new RBush<Point>()
```

An optional argument (`maxEntries: `) to the constructor defines the maximum number 
of entries in a tree node. `9` (used by default) is a reasonable choice for most 
applications. Higher value means faster insertion and slower search, and vice versa.

```csharp
var tree = new RBush<Point>(maxEntries: 16)
```

### Adding Data

Insert an item with an extent:

```csharp
var item = new Point(double minX, double minY, double maxX, double maxY);
tree.Insert(item);

or

Insert an item with a position:

var item = new Evnelope(float x, float y)
tree.Insert(item);

```

### Removing Data

Remove a previously inserted item:

```csharp
tree.Delete(item);
```

By default, RBush uses `object.Equals()` to select the item. If the item being 
passed in is not the same reference value, ensure that the class supports 
`object.Equals()` equality testing.

Remove all items:

```csharp
tree.Clear();
```

### Bulk-Inserting Data

Bulk-insert the given data into the tree:

```csharp
var points = new List<Point>();
tree.BulkLoad(points);
```

Bulk insertion is usually ~2-3 times faster than inserting items one by one.
After bulk loading (bulk insertion into an empty tree),
subsequent query performance is also ~20-30% better.

Note that when you do bulk insertion into an existing tree,
it bulk-loads the given data into a separate tree
and inserts the smaller tree into the larger tree.
This means that bulk insertion works very well for clustered data
(where items in one update are close to each other),
but makes query performance worse if the data is scattered.

### Search

```csharp
var result = tree.Search(
    new Envelope
    {
        MinX: 40,
        MinY: 20,
        MaxX: 80,
        MaxY: 70
    });
```

Returns an `IEnumerable<T>` of data items (points or rectangles) that the given bounding box intersects.

```csharp
var allItems = tree.Search();
```

Returns all items of the tree.

<!--## Performance
!!Notice.. these performance statics are from the original C# [RBush](https://github.com/viceroypenguin/RBush).
I remade the library to be Unity compatible and friendly.

The following sample performance test was done by generating
random uniformly distributed rectangles of ~0.01% area and setting `maxEntries` to `16`
(see `debug/perf.js` script).
Performed with Node.js v6.2.2 on a Retina Macbook Pro 15 (mid-2012).

Test                         | RBush  | [old RTree](https://github.com/imbcmdth/RTree) | Improvement
---------------------------- | ------ | ------ | ----
insert 1M items one by one   | 3.18s  | 7.83s  | 2.5x
1000 searches of 0.01% area  | 0.03s  | 0.93s  | 30x
1000 searches of 1% area     | 0.35s  | 2.27s  | 6.5x
1000 searches of 10% area    | 2.18s  | 9.53s  | 4.4x
remove 1000 items one by one | 0.02s  | 1.18s  | 50x
bulk-insert 1M items         | 1.25s  | n/a    | 6.7x

If you're indexing a static list of points (you don't need to add/remove points after indexing), 
you should use [kdbush](https://github.com/mourner/kdbush) which performs point indexing 5-8x 
faster than RBush.
-->

## Credit
This code (and most of this readme) has been adapted from the C# [RBush](https://github.com/viceroypenguin/RBush) library.
Which in turn was adapted from a Javascript library called [RBush](https://github.com/mourner/rbush). 
The only changes made were to adapt coding styles, preferences and to add compatibility and support for working in Unity game engine.

## Algorithms Used

* single insertion: non-recursive R-tree insertion with overlap minimizing split routine from R\*-tree (split is very effective in JS, while other R\*-tree modifications like reinsertion on overflow and overlap minimizing subtree search are too slow and not worth it)
* single deletion: non-recursive R-tree deletion using depth-first tree traversal with free-at-empty strategy (entries in underflowed nodes are not reinserted, instead underflowed nodes are kept in the tree and deleted only when empty, which is a good compromise of query vs removal performance)
* bulk loading: OMT algorithm (Overlap Minimizing Top-down Bulk Loading) combined with Floyd�Rivest selection algorithm
* bulk insertion: STLT algorithm (Small-Tree-Large-Tree)
* search: standard non-recursive R-tree search

## Papers

* [R-trees: a Dynamic Index Structure For Spatial Searching](http://www-db.deis.unibo.it/courses/SI-LS/papers/Gut84.pdf)
* [The R*-tree: An Efficient and Robust Access Method for Points and Rectangles+](http://dbs.mathematik.uni-marburg.de/publications/myPapers/1990/BKSS90.pdf)
* [OMT: Overlap Minimizing Top-down Bulk Loading Algorithm for R-tree](http://ftp.informatik.rwth-aachen.de/Publications/CEUR-WS/Vol-74/files/FORUM_18.pdf)
* [Bulk Insertions into R-Trees Using the Small-Tree-Large-Tree Approach](http://www.cs.arizona.edu/~bkmoon/papers/dke06-bulk.pdf)
* [R-Trees: Theory and Applications (book)](http://www.apress.com/9781852339777)


## Compatibility

R_Tree should run on any .NET system that supports .NET Framework 3.5 or later; 


