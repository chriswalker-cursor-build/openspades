# OpenSpades Factory Log

## Meta
- Fork: https://github.com/chriswalker-cursor-build/openspades
- Upstream: https://github.com/yvt/openspades
- Constraint: visual-only; CI uses OPENSPADES_NONFREE_RESOURCES=NO + OPENSPADES_YSR=NO
- Quality gates: compile (build-ubuntu-free) + clang-format (lint-clang-format) + ctest + Bugbot
- Fan-out: Automations A/B/C trigger on Slice 0 merge

## Conventions
- Append-only sections per slice.
- Every fix: repro → root cause → files → proof → status.
- Lint / Tests fields required on every slice entry.

---

## Slice 0 — Foundation
### 2026-08-10
- Change: Added factory foundation — `AGENTS.md`, `.cursor/rules/factory-quality-gates.mdc`, Ubuntu `build-ubuntu-free` + `lint-clang-format` jobs, Catch2 minimal `Tests/` suite wired via `enable_testing()` / `add_subdirectory(Tests)`, `docs/FACTORY.md` + this log. Documented format IN/OUT scope (CI enforces `Tests/**`; first-party `Sources/` excluded from fail gate due to pre-existing clang-format-18 drift + Slice 0 allowlist / forbidden paths). Updated `.cursor/install.sh` to NONFREE=NO / YSR=NO + clang-format.
- Proof: Local `cmake -S . -B openspades.mk -DCMAKE_BUILD_TYPE=RelWithDebInfo -DOPENSPADES_NONFREE_RESOURCES=NO -DOPENSPADES_YSR=NO -DCMAKE_C_COMPILER=gcc -DCMAKE_CXX_COMPILER=g++` configure/build; `ctest --test-dir openspades.mk --output-on-failure` → 8/8 Catch2 cases (26 assertions); `clang-format --dry-run --Werror` on `Tests/MathTests.cpp` clean. Full `OpenSpades` target still links with NONFREE=NO.
- CI: `build-ubuntu-free` + `lint-clang-format` (existing Win/macOS/Nix kept).
- Lint: IN SCOPE = `Tests/**` C++ (exclude `Tests/third_party`); OUT = AngelScript vendor, third-party Sources trees, pre-existing drifted first-party Sources (~181/382 files vs clang-format-18), `Resources/Scripts/**/*.as` (see `AGENTS.md` + workflow comments).
- Tests: Catch2 v2.13.10 single-header characterisation suite (`openspades_unit_tests`) — IntVector3 / Vector3 / UTF-8 helpers from `Sources/Core/Math.h` (header-only, no GPU). Honest minimal coverage, not a game suite.
- Bugbot: Addressed medium finding — removed unused `submodules: true` from `build-ubuntu-free` (apt/free smoke does not need vcpkg/flatpak submodules).
- Status: required gates green locally + on CI (ubuntu-free + lint); re-push for Bugbot fix; merge when Autofix settles / permissions allow



---

## Slice A — Lighting
### 2026-08-10
- Change: Visual-only lighting pass within Slice A allowlist — warmer sun/ambient/dynamic lights; softer SSAO + ambient-shadow contact terms with lifted black floor; softer map-shadow penumbra / variance min; mild sun wrap + AO remap on BasicBlock / VoxelModel / OptimizedVoxelModel / BasicBlockPhys so voxels read less flat. Touched shaders under `Resources/Shaders/{Shadow,DynamicLight,PostFilters/SSAO*,BasicBlock*,VoxelModel*,OptimizedVoxelModel*}` and C++ `GLAmbientShadowRenderer`, `GLSSAOFilter`, `GLShadowShader`, `GLDynamicLightShader`. Did not touch GLWater*/other PostFilters/GLFog*/GLSettings/Client/netcode. Did not widen into `GLRenderer.cpp`.
- Proof: `clang-format --dry-run --Werror` on edited Draw C++ clean; `cmake -S . -B openspades.mk -DCMAKE_BUILD_TYPE=RelWithDebInfo -DOPENSPADES_NONFREE_RESOURCES=NO -DOPENSPADES_YSR=NO -DCMAKE_C_COMPILER=gcc -DCMAKE_CXX_COMPILER=g++` + build OpenSpades OK; `ctest --test-dir openspades.mk --output-on-failure` → 1/1 (`openspades_unit_tests`) passed.
- CI: `build-ubuntu-free` + `lint-clang-format` + ctest on PR (self-fix).
- Lint: no `Tests/**` edits; formatted touched `Sources/Draw/*` before push (OUT-OF-SCOPE fail-gate files still formatted per AGENTS).
- Tests: no characterisation changes required; Catch2 suite green unchanged (8 cases via single ctest entry).
- Bugbot: address review threads if any after PR open.
- Status: local gates green; PR open → self-fix CI → merge if permissions allow

---

## Slice B — Water / Fog / Post-FX
### YYYY-MM-DD
- Change:
- Proof:
- CI:
- Lint:
- Tests:
- Bugbot:
- Status:

---

## Slice C — HUD
### YYYY-MM-DD
- Change:
- Proof:
- CI:
- Lint:
- Tests:
- Bugbot:
- Status:
