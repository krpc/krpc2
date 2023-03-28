#!/usr/bin/env bash

set -ev
bazel build --verbose_failures //:plugin_files
rm -rf lib/ksp2/BepInEx/plugins/kRPC2
rm -f lib/ksp2/BepInEx/config/krpc2.cfg

mkdir -p lib/ksp2/BepInEx/plugins/kRPC2
cp -r bazel-bin/kRPC2/* lib/ksp2/BepInEx/plugins/kRPC2
chmod -R 0664 lib/ksp2/BepInEx/plugins/kRPC2/*
