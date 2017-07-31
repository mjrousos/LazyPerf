Lazy<T> Perf Exploration
========================

This is just a super simple app to look at how two different Lazy<T> 
scenarios (creating via default ctor and creating via func) compare perf-wise 
to creating a singleton explicitly with double-checked locking.

Results (below) indicate that there is a modest perf degrade involved in
using the default-constructor Lazy<T> code path (as is to be expected given 
the Activator.CreateInstance dependency), but using the func path instead has 
little perf downside. In super-sensitive scenarios, you may want to avoid 
Lazy<T> all together, but I have trouble imagining a use case where Lazy<T> 
object creating is on such a perf-sensitive path (hopefully lazilly created 
objects aren't being constructed in tight loops or anything like that).

Sample Results
--------------

```
Lazy<T> Perf Tester
-------------------

Running CreateWithLazyDefaultCtor 10000000 times... 1268 ms
Running CreateWithLazyFactory 10000000 times... 611 ms
Running CreateWithDoubleCheckedLocking 10000000 times... 452 ms
```
