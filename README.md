<div align="center">
	<h1>Shadow MTP Library</h1>
	<strong>Simple library for Importing/Exporting MTP from Shadow the Hedgehog</strong>
</div>

# About

This is a simple library that allows you to import and export .MTP "MotionPackage" archives from Shadow The Hedgehog.

## Usage (Library)

1. Add this git repository as a submodule.
2. Add Heroes.SDK to your solution.
3. Add Heroes.SDK as a project reference to your project.

For an example/sample of using the library, see the `MTPConsole` project.

## Usage (Sample Console Application)

To use the sample application, simply run it as such:

```csharp
MTPConsole.exe <list of space separated files or directories>
```

Example:
```csharp
MTPConsole.exe BkChaos.mtp "BkLarva"
```

Would perform the following.
- Extract `BkChaos.mtp` to directory `BkChaos`. 
- Pack directory `BkLarva` into `BkLarva.MTP`.