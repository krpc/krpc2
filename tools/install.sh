#!/bin/bash

set -ev

bazel build //server:KRPC2
cp bazel-bin/server/bazelout/netstandard2.0/KRPC2.dll lib/ksp2/BepInEx/plugins/kRPC
