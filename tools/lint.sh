#!/bin/bash

buildifier -r ./
bazel-krpc2/external/dotnet_x86_64-unknown-linux-gnu/dotnet format server/src/KRPC2.csproj
bazel-krpc2/external/dotnet_x86_64-unknown-linux-gnu/dotnet format service/SpaceCenter/src/KRPC2.SpaceCenter.csproj