{
  description = "Remote Procedure Calls for Kerbal Space Program 2";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/master";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, flake-utils }:
    flake-utils.lib.eachDefaultSystem (system:
      let
        pkgs = import nixpkgs {
          inherit system;
          config.allowUnfree = true;
        };
        bazel = pkgs.bazel_5;
        wrapped-bazel = pkgs.writeShellScriptBin "bazel" ''
            ${pkgs.steam-run}/bin/steam-run ${bazel}/bin/bazel $@
          '';
      in
      rec {
        devShells.default = with pkgs;
          pkgs.mkShell {
            packages = [
              # build
              wrapped-bazel
              jdk11
              # lint
              buildifier
              dotnet-sdk_7
              # steam
              steam
              steam-run
            ];
            STEAM_RUN_WRAPPER = "${steam-run}/bin/steam-run";
          };

        packages.krpc = pkgs.buildBazelPackage {
            name = "krpc2-dev";
            pname = "krpc";
            bazel = wrapped-bazel;
            nativeBuildInputs = [
              pkgs.git
            ];
            bazelTarget = ":plugin_files";
            src = ./.;
            fetchAttrs = {
              sha256 = "sha256-0TynZKODPXK5DZwB8bNWX8vDSyFi5syWVB0QZIue92E=";
            };
          };
        packages.default = packages.krpc;
      });
}
