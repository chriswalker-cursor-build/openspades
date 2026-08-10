# OpenSpades Factory — Agent Guide

Fork of [yvt/openspades](https://github.com/yvt/openspades) (GPL-3.0 C++/OpenGL voxel FPS).
Working fork: https://github.com/chriswalker-cursor-build/openspades

## Mission

Improve **visual look only**. Automations fan out parallel visual slices **A ∥ B ∥ C** after **Slice 0** merges.

Do **not** wait for a human. Decide, implement, keep quality gates green, merge when allowed.

## Quality gates (every PR)

Every PR must keep **all** of these green. Agents fix failures themselves — do not leave red CI for a human.

| Gate | How |
| ---- | --- |
| Compile | `build-ubuntu-free` — free cmake smoke |
| Format | `lint-clang-format` — clang-format dry-run on IN-SCOPE files |
| Unit tests | `ctest` after the Ubuntu free build |
| Review | Address Bugbot threads you can fix |

### Canonical local / CI cmake proof

```bash
cmake -S . -B openspades.mk -DCMAKE_BUILD_TYPE=RelWithDebInfo \
  -DOPENSPADES_NONFREE_RESOURCES=NO -DOPENSPADES_YSR=NO \
  -DCMAKE_C_COMPILER=gcc -DCMAKE_CXX_COMPILER=g++
cmake --build openspades.mk -j"$(nproc)"
ctest --test-dir openspades.mk --output-on-failure
```

CI uses the same flags with `-B build` instead of `openspades.mk`.

**Always** `OPENSPADES_NONFREE_RESOURCES=NO` and `OPENSPADES_YSR=NO`.
Do **not** download or commit proprietary Ace of Spades paks / YSR binaries.
In-tree `Resources/Shaders` + free gfx are enough for compile proof.
GPU playtest is optional/local; prove look via shader/C++ review + compile + format + ctest + `docs/FACTORY_LOG.md` notes.

Prefer **GCC** on Linux: Clang can miscompile the bundled AngelScript null-`this` guard and segfault at script compile time.

## Visual-only scope

Allowed themes for later slices (not Slice 0): lighting, water/fog/post-FX, HUD — look only.

### Forbidden paths (never edit intentionally)

```
Sources/Client/NetClient.*
Sources/Client/World.*
Sources/Client/Player.*
Sources/Client/Weapon.*
Sources/Client/Grenade.*
Sources/Client/CTFGameMode.*
Sources/Client/TCGameMode.*
Sources/Client/PhysicsConstants.h
Sources/Client/Client_Draw.cpp
Sources/Client/Client_Scene.cpp
Sources/ENet/**
```

No netcode / gameplay / physics / protocol changes.

## Path allowlists

Path allowlists are **STRICT** and **DISJOINT**. Never expand mid-flight.

Exception for visual phases: `Tests/**` and root `CMakeLists.txt` test wiring **only** when needed to keep the minimal suite green after a visual change — prefer not to break tests.

If blocked on shared glue (e.g. `GLRenderer.cpp`), note it in `docs/FACTORY_LOG.md` and continue with the best allowlisted change; do **not** silently widen scope.

Prefer merge order: **Slice 0 → A → B → C**. If `FACTORY_LOG` conflicts, concatenate sections.

## Format scope (`lint-clang-format`)

Uses the repo-root `.clang-format` (do not invent a second style).

### IN SCOPE (CI fails on drift)

- `Tests/**/*.{cpp,h,hpp,c,cc}` — factory unit-test tree (Slice 0+)

### OUT OF SCOPE (documented; not failed in CI)

| Path | Why |
| ---- | --- |
| `Sources/AngelScript/**` | Vendor tree; formatting is hell / not first-party |
| `Sources/ENet/**`, `Sources/json/**`, `Sources/kiss_fft130/**`, `Sources/unzip/**`, `Sources/binpack2d/**` | Third-party / vendored |
| `Sources/Imports/**` | Import headers / non-owned surface |
| `Sources/{Client,Core,Draw,Gui,Audio,ScriptBindings}/**` | Pre-existing drift vs clang-format-18 + root `.clang-format` (~half of first-party files). Mass reformat is outside Slice 0 allowlist and would touch **FORBIDDEN** gameplay paths. Visual slices **must** `clang-format` every first-party file they edit. |
| `Resources/Scripts/**/*.as` | `run-clang-format.ps1` pretends `.as` is Java then fixes `@ this.` → `@this.`; dry-run without that post-pass is unreliable. Keep Scripts out of CI lint. |

Portable C++ format check (CI) is the answer for first-party C++; `run-clang-format.ps1` remains the Windows/local helper for AngelScript and may gain C++ parity later — Ubuntu CI must not depend on PowerShell.

When a visual slice edits an OUT-OF-SCOPE first-party file, format that file before push so drift does not grow.

## Unit tests (honest minimal suite)

The Catch2 suite under `Tests/` is **minimal characterisation** of pure helpers (math/string-style APIs in headers such as `Sources/Core/Math.h`). It is **not** full game coverage. No GPU/OpenGL tests.

`ctest --test-dir <build> --output-on-failure` must stay green (about 3–10 tests).

## Factory log

Append-only `docs/FACTORY_LOG.md` with unique `### Slice X` / dated entries.
Every slice entry needs **Lint** and **Tests** fields (plus Change / Proof / CI / Bugbot / Status).

See also `docs/FACTORY.md` (mobile watch + Bugbot checklist + fan-out reminder).

## GPL-3

Do not import proprietary assets. Keep `OPENSPADES_NONFREE_RESOURCES=NO`.

## Async / walk-away

Do not ask clarifying questions when the task is clear. Self-fix CI and Bugbot. Merge Slice 0 when green so Automations A/B/C can fire.

## Slice map

| Slice | Focus |
| ----- | ----- |
| 0 | Foundation: AGENTS, CI smoke, format lint, ctest, FACTORY docs |
| A | Lighting (visual) |
| B | Water / fog / post-FX (visual) |
| C | HUD (visual) |
