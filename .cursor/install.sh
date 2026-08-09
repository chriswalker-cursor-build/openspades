#!/usr/bin/env bash
# Cloud Agent install script for OpenSpades (Linux, from source).
# Idempotent: safe to run repeatedly. Installs system dependencies, then
# configures and builds the game out-of-source into `openspades.mk`.
set -euo pipefail

export DEBIAN_FRONTEND=noninteractive

# Build + runtime dependencies. Mirrors the Debian list in README.md, plus the
# extra jpeg/Xinerama/Xft packages that README notes some distributions need,
# plus Mesa software rendering and Xvfb so the GUI can be launched headlessly.
sudo apt-get update
sudo apt-get install -y --no-install-recommends \
  build-essential pkg-config cmake \
  libglew-dev libcurl4-openssl-dev libsdl2-dev libsdl2-image-dev libalut-dev \
  xdg-utils libfreetype6-dev libopus-dev libopusfile-dev \
  imagemagick zip unzip \
  libjpeg-dev libxinerama-dev libxft-dev \
  libgl1-mesa-dri libglu1-mesa-dev xvfb

# Configure and build. The out-of-source `openspades.mk` directory matches the
# project's documented Linux build convention.
#
# Build with GCC explicitly: on images whose default `c++` is Clang, Clang's
# optimizer elides the UB-based null-`this` guard in the bundled AngelScript
# (as_typeinfo.cpp), which segfaults while compiling the game's scripts at
# startup. GCC is the compiler the project's Linux instructions assume.
cmake -S . -B openspades.mk \
  -DCMAKE_BUILD_TYPE=RelWithDebInfo \
  -DCMAKE_C_COMPILER=gcc \
  -DCMAKE_CXX_COMPILER=g++
cmake --build openspades.mk -j"$(nproc)"

echo "OpenSpades build complete: openspades.mk/bin/openspades"
