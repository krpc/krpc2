load("@rules_dotnet//dotnet:defs.bzl", "csharp_library", "import_dll")
load("//lib:ksp.bzl", "ksp_libs")

import_dll(
    name = "BepInEx",
    dll = "@spacewarp//:BepInEx/core/BepInEx.dll",
)

import_dll(
    name = "SpaceWarp",
    dll = "@spacewarp//:BepInEx/plugins/SpaceWarp/SpaceWarp.dll",
)

import_dll(
    name = "KRPC.Core",
    dll = "@krpc_core//:BepInEx/plugins/kRPC2/KRPC.Core.dll",
)

csharp_library(
    name = "KRPC2.SpaceCenter",
    srcs = glob(["src/**/*.cs"]),
    target_frameworks = ["netstandard2.0"],
    visibility = ["//visibility:public"],
    deps = [
        ":BepInEx",
        ":KRPC.Core",
        ":SpaceWarp",
        "//server:KRPC2",
    ] + ksp_libs,
)
