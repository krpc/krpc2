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

Here's an example in Python, with all the currently available telemetry:
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
 * [SpaceDock](https://spacedock.info/mod/3322/kRPC2)
 * [Discord](https://discord.gg/bXuaTrj)

## Building

 * Install Bazel
 * Put a copy of KSP2 in lib/ksp2 (so you have lib/ksp2/KSP_x64_Data/...)
 * Run `bazel build //server:KRPC`
