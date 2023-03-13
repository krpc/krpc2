# kRPC2

kRPC2 allows you to control Kerbal Space Program 2 from scripts running outside of
the game, and comes with client libraries for many popular languages.

This is a continuation of the kRPC mod for Kerbal Space Program 1.

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

Here's an example, with all the currently available telemetry:
```
import krpc
conn = krpc.connect()
print(conn.space_center2.horizontal_surface_speed)
print(conn.space_center2.vertical_surface_speed)
print(conn.space_center2.terrain_altitude)
print(conn.space_center2.sealevel_altitude)
```

## Links for more info

 * [Documentation for kRPC1](https://krpc.github.io/krpc)
 * [Discord](https://discord.gg/bXuaTrj)

## Building

To compile the project:

 * Copy all of the KSP2 DLLs, and the SpaceWarp DLLs into the external DLLs folder.
 * Copy the kRPC "core" DLLs (from the release page) to the krpc_dlls folder.
 * Use the KRPC.sln to build the project.
