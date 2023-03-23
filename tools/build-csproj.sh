#!/bin/bash

set -ev

bazel clean
bazel build //:csproj

dotnet restore KRPC2.sln
dotnet clean KRPC2.sln
dotnet build KRPC2.sln
