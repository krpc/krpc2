#!/bin/bash

set -ev
bazel build //:krpc2
rm -rf lib/ksp2/BepInEx/plugins/kRPC2
cp -r bazel-bin/kRPC2 lib/ksp2/BepInEx/plugins/
chmod 0664 lib/ksp2/BepInEx/plugins/kRPC2/*
