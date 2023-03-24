#!/bin/bash

set -ev
bazel build //:krpc2
rm -rf lib/ksp2/BepInEx/plugins/kRPC2
rm -f lib/ksp2/BepInEx/config/krpc2.cfg
cp -r bazel-bin/kRPC2 lib/ksp2/BepInEx/plugins/
chmod 0664 lib/ksp2/BepInEx/plugins/kRPC2/*
