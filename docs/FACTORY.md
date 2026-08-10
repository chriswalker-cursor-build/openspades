# OpenSpades Factory — Ops Notes

## Mobile watch

- Agents: https://cursor.com/agents
- PRs: https://github.com/chriswalker-cursor-build/openspades/pulls
- Upstream reference: https://github.com/yvt/openspades

## Quality gates (every PR)

1. **Compile** — `build-ubuntu-free` (`OPENSPADES_NONFREE_RESOURCES=NO`, `OPENSPADES_YSR=NO`)
2. **Format** — `lint-clang-format` (IN-SCOPE files; see `AGENTS.md`)
3. **Tests** — `ctest` in the Ubuntu free job
4. **Bugbot** — address actionable review threads before merge

Agents fix red CI themselves. Do not leave failures for a human.

## Bugbot enablement checklist

- [ ] Bugbot enabled on `chriswalker-cursor-build/openspades`
- [ ] Reviews requested on factory PRs
- [ ] Agents reply to / fix Bugbot comments on their own PR
- [ ] No blocking Bugbot threads before merge when fixable in-allowlist

## Fan-out

After **Slice 0** merges to the default branch, Automations start visual slices **A ∥ B ∥ C** in parallel.

Slice 0 is foundation only: `AGENTS.md`, Ubuntu CI smoke, clang-format lint job, minimal ctest suite, factory docs. **No visual game changes** in Slice 0.

## Format reminder

- Root `.clang-format` is authoritative.
- CI enforces `Tests/**` C++ format.
- Pre-existing first-party `Sources/` drift is documented OUT OF SCOPE for the fail gate; format files you touch in visual slices.
- AngelScript under `Resources/Scripts` stays on `run-clang-format.ps1` locally (not CI).
