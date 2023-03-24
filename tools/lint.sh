#!/bin/bash

buildifier -r ./
dotnet format KRPC2.sln --exclude bazel-bin
