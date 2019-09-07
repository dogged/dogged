<h1><img src="http://dogged.app/logos/dogged-withtype.png" alt="Dogged" width="350" height="75"></h1>

[![Build Status](https://github.com/ethomson/dogged/workflows/CI/badge.svg)](https://github.com/ethomson/dogged/actions)

[Dogged](https://github.com/ethomson/dogged) and is a .NET wrapper around
[libgit2](https://github.com/libgit2/libgit2).  It was inspired by the
[LibGit2Sharp](https://github.com/libgit2/libgit2sharp) project.

LibGit2Sharp remains the more mature option for .NET development of Git
repository management tasks.  This library is in early development;
Dogged differs from LibGit2Sharp in a number of ways:

* **Providing direct native access and a higher-level wrapper**  
  LibGit2Sharp hides the PInvoke bindings as private methods; Dogged
  exposes them in the `Dogged.Natives` package.  A number of users want
  to avoid any managed layer and call the native code directly; with
  `Dogged.Native` they can do so.
* **More direct native access with minimal caching or pre-loading**  
  A number of LibGit2Sharp APIs try to "pre-load" data from the underlying
  native bindings.  This allows LibGit2Sharp to offer an API that has few
  `IDisposable` types at the expense of always loading data that may go
  unused.  In contrast, Dogged attempts to only load data when necessary.
* **A more direct mapping to libgit2**  
  LibGit2Sharp includes a number of classes that attempt to emulate parts
  of the Git command-line.  Dogged attempts to more directly map libgit2,
  believing that innovation should occur in the shared native library so
  that all consumers can benefit, not just the .NET users.

Dogged is available under the MIT license, see the included file `LICENSE`
for details.
