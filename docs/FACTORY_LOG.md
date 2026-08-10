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
### YYYY-MM-DD
- Change:
- Proof:
- CI:
- Lint:
- Tests:
- Bugbot:
- Status:

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
### 2026-08-10
- Change: Visual-only HUD/chrome polish within Slice C allowlist — softer chat/killfeed shadows + expanded panel; eased hurt-ring opacity/size; scoreboard vignette/team-bar/players-bg alphas reduced and DrawShadow typography for names/scores/spectators; limbo menu fills + HeadingFont Spawn label; minimap scrim/grid/label/border alphas cleaned; TC progress + center-message shadows softened; Client Gui chat-log/menu overlay alphas reduced (`ChatLogWindow.as`, `Menu.as`). No netcode/gameplay/physics/protocol; did not touch Client_Draw/Client_Scene/Sources/Draw/**/Shaders.
- Proof: Local `cmake -S . -B openspades.mk -DCMAKE_BUILD_TYPE=RelWithDebInfo -DOPENSPADES_NONFREE_RESOURCES=NO -DOPENSPADES_YSR=NO -DCMAKE_C_COMPILER=gcc -DCMAKE_CXX_COMPILER=g++` configure + build OK; `ctest --test-dir openspades.mk --output-on-failure` → 1/1 (`openspades_unit_tests`, Catch2 cases green); `clang-format --dry-run --Werror` clean on edited HUD Client sources.
- CI: PR https://github.com/chriswalker-cursor-build/openspades/pull/3 — require `build-ubuntu-free` + `lint-clang-format` + ctest green.
- Lint: Edited first-party `Sources/Client/{ChatWindow,HurtRingView,CenterMessageView,TCProgressView,ScoreboardView,LimboView,MapView}.cpp` clang-formatted; CI IN-SCOPE remains `Tests/**` (untouched). Scripts `*.as` out of CI lint by design.
- Tests: Minimal Catch2 suite unchanged (no Math/helper API edits); ctest 1/1 passed locally after HUD rebuild.
- Bugbot: Watching PR #3 for in-allowlist threads.
- Status: local proof green; PR open; merge when CI/Bugbot allow.
