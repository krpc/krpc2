# kRPC2

kRPC2 allows you to control Kerbal Space Program 2 from scripts running outside of
the game, and comes with client libraries for many popular languages.

This is a continuation of the kRPC mod for Kerbal Space Program 1.

## Links for more info

 * [SpaceDock](https://spacedock.info/mod/3322/kRPC2)
 * [Forum thread](https://forum.kerbalspaceprogram.com/index.php?/topic/214999-krpc2-control-the-game-using-python-c-c-java-lua/)
 * [Discord](https://discord.gg/bXuaTrj)
 * [Documentation for kRPC1](https://krpc.github.io/krpc)
 * [Contribution guide](https://github.com/krpc/krpc/blob/main/Contributing.md) (for kRPC1, but still applies here)
 
## Installing

Requires SpaceWarp/BepInEx to be installed.

Download the release archive and extract it into the BepInEx folder.

## Current Status

The mod is a very early work in progress, so don't expect much!

Currently there is a single "service", called SpaceCenter2, with a few properties for basic telemetry.

It works with the existing kRPC client libraries (as it speaks the same protocol).
However we don't yet provide generated stubs for clients like C++, C# and Java.
It has only been tested using the python client, which does not require
generated stubs (it can auto-generate them on connection).

When the game starts, a server is created with RPC port 50000 and Stream port 50001.
There is no configuration for this yet - it is hard coded in.

Here's an example in Python, with all the currently available telemetry:
```
import krpc
conn = krpc.connect()
print(conn.space_center2.horizontal_surface_speed)
print(conn.space_center2.vertical_surface_speed)
print(conn.space_center2.terrain_altitude)
print(conn.space_center2.sealevel_altitude)
```

## Building on Linux

 * [Install Bazel](https://bazel.build/install/)
 * Create a symlink from `lib/ksp2` to where you have Kerbal Space Program 2 installed, so that you have `lib/ksp2/KSP_x64_Data/Managed/...`
 * Run `bazel build //:krpc2`
 * The resulting DLLs are placed in the `bazel-bin` directory

## Building on Windows

 * [Install Bazel](https://bazel.build/install/)
   * If your user directory contains spaces, the build may not work. If this is the case, create a file called
     `%ProgramData%/bazel.bazelrc` containing the following:
     ```
     startup --output_user_root="C:/bazel-root"
     ```
   * If you get permissions errors related to symlinks when building you need to enable "Developer Mode".
 * Put a copy of KSP2 in lib/ksp2 (so you have `lib/ksp2/KSP_x64_Data/Managed/...`)
 * Run `bazel build //:krpc2`
 * The resulting DLLs are placed in the `bazel-bin` directory
