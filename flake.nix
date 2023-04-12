{
  description = "Remote Procedure Calls for Kerbal Space Program 2";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-22.11";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, nixpkgs, flake-utils }:
    # Flake supports just Linux for now
    flake-utils.lib.eachSystem [ "x86_64-linux" ] (system:
      let
        ksp2-version = "ksp2-0.1.1";
        pkgs = import nixpkgs {
          inherit system;
          # Required for steam deps.
          config.allowUnfree = true;
        };
        # We wrap Bazel here for NixOS support which does not have the standard
        # FHS. NOTE: This should also act as an environment regularizing command
        # on other OSes, but may make inital build times a bit longer.
        wrapped-bazel = pkgs.bazel_5.overrideAttrs (old: {
          doCheck = false;
          doInstallCheck = false;
          postFixup = ''
            mv $out/bin/bazel $out/bin/bazel-raw
            echo '#!${pkgs.stdenv.shell}' > $out/bin/bazel
            echo "${pkgs.steam-run}/bin/steam-run $out/bin/bazel-raw \$@" >> $out/bin/bazel
            chmod +x $out/bin/bazel
          '';
        });
        stripped-krpc = pkgs.fetchzip {
          url = "https://github.com/krpc/ksp-lib/raw/main/ksp2/${ksp2-version}.zip";
          sha256 = "sha256-Byyn9CZBO364NIJHeJfeJnM4J11ZY/jDxfQZNdS0CCA=";
        };
      in
      rec {
        devShells.default = with pkgs;
          pkgs.mkShell {
            packages = [
              # build
              dotnet-sdk_6
              jdk11
              wrapped-bazel
              # lint
              buildifier
              # steam
              steam
              steam-run
            ];
            STEAM_RUN_WRAPPER = "${steam-run}/bin/steam-run";
          };

        packages.krpc2 = pkgs.buildBazelPackage {
          name = "krpc2-dev";
          pname = "krpc2";
          bazel = wrapped-bazel;
          bazelTarget = ":plugin_files";
          nativeBuildInputs = [
            pkgs.git
          ];
          src = ./.;
          # NOTE: Update on change to bazel fetch deps.
          fetchAttrs = {
            sha256 = "sha256-ce20qIEBO0JyQWvOrt/mLGSH0PLIHtdGjdMJkBG13Ws=";
          };
          patchPhase = ''
            # Copied directory, so will not influence local symlinks.
            ln -sf ${stripped-krpc} lib/ksp2
            mv lib/ksp2 lib/KSP2_x64_Data
            mkdir -p lib/ksp2
            mv lib/KSP2_x64_Data lib/ksp2/
          '';
          buildAttrs = {
            installPhase = ''
              mkdir -p $out/lib
              install -Dm0755 bazel-bin/kRPC2/* $out/lib/
            '';
          };
        };
        packages.default = packages.krpc2;
      });
}
