# `stackalloc` for reference types with Roslyn and CoreCLR

This repository contains the prototype used in the post [A new stackalloc operator for reference types with CoreCLR and Roslyn](http://xoofx.com/blog/2015/10/08/stackalloc-for-class-with-roslyn-and-coreclr/)

# Content

It contains:
- A [fork of the CoreCLR](https://github.com/xoofx/coreclr/tree/stackalloc_for_class) project from the branch `stackalloc_for_class` 
- A [fork of Roslyn](https://github.com/xoofx/roslyn/tree/stackalloc_for_class) project from the branch `stackalloc_for_class`
- A project `StackAllocForClass` using the new keyword `transient` and `stackalloc` 

# How to Build?

Make sure that all submodules are correctly initialized when cloning this repository.

## Building CoreCLR

The official instructions are [here](https://github.com/dotnet/coreclr/blob/master/Documentation/building/windows-instructions.md). 

But using this simple steps should work: 

- Install [CMake](http://www.cmake.org/download) for Windows.
- Add it to the PATH environment variable.
- Open a VS2015 command prompt, go to the folder `coreclr` and type: `build skiptestbuild`

## Building Roslyn

The official build instructions are [here](https://github.com/dotnet/roslyn/wiki/Building%20Testing%20and%20Debugging)

- Open a VS2015 command prompt, go to the folder `roslyn`
- Run: `nuget.exe restore Roslyn.sln`
- Open the solution `Roslyn.sln`
- Compile the full solution (in debug mode)

# How to Test?

- Open a VS2015 command prompt
- Run: `devenv.exe /rootSuffix Roslyn` (Note that syntax analysis in VS doesn't use the new Roslyn, haven't dig enough why...)
- This will start a VS2015 with the C# compiler using the version compiled previously.
- Open the project `StackAllocForClass.sln` and build the project
- Back to the command prompt, go to the output bin folder `StackAllocForClass\bin\Debug`

If you run this command, it will use the version using `stackalloc` operator:

- `..\..\..\coreclr\bin\Product\Windows_NT.x64.Debug\CoreRun.exe StackAllocForClass.exe`

You should get the following output:

``` 
Mode: StackAlloc . To switch to HeapAlloc, simply pass an argument to this exe
[before] GC gen0 collect: 0
[before] GC gen1 collect: 0
[before] GC gen2 collect: 0
Result: -729545010
[after] GC gen0 collect: 0
[after] GC gen1 collect: 0
[after] GC gen2 collect: 0
Elapsed: 418.9245ms
```

Passing a single parameter will switch to heap mode:

- `..\..\..\coreclr\bin\Product\Windows_NT.x64.Debug\CoreRun.exe StackAllocForClass.exe 1`

In heap mode:

```
Mode: HeapAlloc
[before] GC gen0 collect: 0
[before] GC gen1 collect: 0
[before] GC gen2 collect: 0
Result: -729545010
[after] GC gen0 collect: 114
[after] GC gen1 collect: 0
[after] GC gen2 collect: 0
Elapsed: 4984.224ms
```

## License
Same license as CoreCLR and Roslyn.

## Author

Alexandre Mutel aka [@xoofx](http://xoofx.com)