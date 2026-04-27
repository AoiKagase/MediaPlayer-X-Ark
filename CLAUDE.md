# 個人設定

## コーディングスタイル
- タブインデント
- シングルクォート優先
- IF文の波括弧は省略可能な場合は省略
- IF文の右に処理を書かず、必ず次の行に処理を書く

## 言語設定
- 全ての応答は日本語で

## 開発環境
- Visual Studio 2026 C# WinFormsプロジェクト

# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

MediaPlayer X-Ark is a Windows desktop media player built with C# (.NET 9.0-windows7.0) and Windows Forms, using the FMOD audio engine as its core. The author is Aoi.Kagase.

## Build Commands

```bash
# Build (Release)
dotnet build "MediaPlayer X-Ark.csproj" -c Release

# Build (Debug)
dotnet build "MediaPlayer X-Ark.csproj" -c Debug

# Publish as single file
dotnet publish "MediaPlayer X-Ark.csproj" -c Release -r win-x64
```

There are no automated tests or linting tools configured. Open `MediaPlayer X-Ark.sln` in Visual Studio 2022+ for IDE development.

**Post-build**: Native DLLs (`fmod.dll`, `NFluidsynth.dll`, `ATL.dll`, `DiscordRPC.dll`) are automatically moved to a `Libs/` subfolder by a post-build target in the `.csproj`.

## Architecture

The app uses a layered architecture:

```
Forms (UI) → PlayerController → IPlayerEngine (PlayerEngine) → FMOD (native)
```

### Core Layers

**Forms** (`Forms/`) — Windows Forms UI. Each form subscribes to events from `PlayerController` for reactive updates. Cross-thread marshaling is done via `SynchronizationContext.Post()`.

**PlayerController** (`Engine/Player/PlayerController.cs`) — High-level API between UI and the audio engine. Publishes events: `TrackChanged`, `PlaybackStateChanged`, `WaveformReady`, `ErrorOccurred`. Manages NonStopMix crossfade timing via a 5ms precision timer.

**PlayerEngine** (`Engine/Player/PlayerEngine.cs`, ~1265 lines) — Core FMOD wrapper. Handles all audio playback, ReplayGain normalization, AB-repeat, sleep timer, shuffle queue, and plugin loading.

**Effector System** (`Engine/Effector/`) — Audio DSP effects. `AbstractEffectorBase` is the base class; all effects inherit from it and bridge to FMOD DSP via `FmodDspBridge`. The `Effectors` container manages all active effects. Effects support preset save/load.

**Configuration** (`Engine/Config/`) — JSON-based settings persistence. `Configuration` reads/writes `ConfigurationData`, which has sections for audio output, effects, playback, display, and network (CDDB). `SupportedFormats` tracks built-in FMOD formats plus dynamically loaded codec plugins.

**Skin System** (`Skin/`) — XML-based UI theming via `.xsk` packages. `NewSkinSystem` is the current format; `OldSkinSystem` is legacy. `SkinApplicator` applies a loaded skin across all forms. `WaveformRenderer` (in `Skin/`) uses Direct2D (Vortice) to generate the waveform bitmap for the seek bar; it supports Mix/Stereo/Overlay display modes.

**Visualize** (`Engine/Visualize/`) — `FmodSpectrum` drives FFT spectrum analysis; `WaveformAnalyzer` generates the full-track waveform peak data. `SpectrumAnalyzer` is a custom double-buffered WinForms control that renders on a dedicated draw thread.

**Direct2D Render** (`Engine/Render/D2DContext.cs`) — Singleton that initializes Vortice Direct2D1/DirectWrite/WIC factories at startup. Must call `D2DContext.Initialize()` before any D2D rendering and `D2DContext.Dispose()` on shutdown.

**CUE Sheet** (`Engine/CUE/`) — `CueParser` parses `.cue` files into `CueSheet`/`CueTrack` structures for gapless/indexed playback of single-file albums.

**Auto Update** (`Engine/Update/`) — `UpdateChecker` queries the GitHub Releases API (owner/repo configurable) and returns `UpdateInfo` when a newer version exists. `UpdateApplier` handles the download and replacement.

**CD Support** (`Engine/CD/`) — Win32 IOCTL-based raw CD reading (`CDReader`) and ripping (`CdRipper`) with FLAC/ALAC encoders, plus CDDB/MusicBrainz protocol client (`CddbClient`) for online metadata.

**Discord** (`Engine/Discord/DiscordPresenceService.cs`) — Discord Rich Presence integration showing now-playing info.

### Key Interfaces

| Interface | Implementation | Purpose |
|-----------|---------------|---------|
| `IPlayerEngine` | `PlayerEngine` | All playback operations |
| `IConfigService` | `Configuration` | Settings read/write |
| `IEffector` | `AbstractEffectorBase` subclasses | DSP effects |
| `INewSkinSystem` | `NewSkinSystem` | UI theming (`Skin/New/`) |

### Threading Model

- **UI thread (STA)**: All WinForms controls
- **Draw thread**: `SpectrumAnalyzer` rendering loop
- **Timer thread**: `PlayerController._preciseTimer` for NonStopMix (5ms)
- **D2D thread**: `WaveformRenderer` runs on background tasks (Direct2D is MT-factory)
- Always marshal back to UI thread via `SynchronizationContext.Post()` in event handlers

### Forms

- **MainForm** — Primary player window; owns `PlayerController`, `IPlayerEngine`, `IConfigService`
- **MiniPlayerForm** — Compact overlay player
- **PlayListForm** — Playlist editor
- **CDForm** — CD ripping UI
- **OptionsForm** — Settings host; embeds `OptionsControlBase` subclasses (output, effects, display, plugins, etc.)
- **FileInfoForm** — Tag viewer

### Custom Controls (`Controls/`)

- `SpectrumAnalyzer` — Double-buffered spectrum with snow-block effect
- `CustomSlider`, `ColorSlider` — Skinnable sliders
- `Knob` — Rotary knob control
- `ScrollLabel` — Auto-scrolling text for long track names

### Assembly Loading

`StartUp.cs` handles custom assembly resolution so that DLLs in the `Libs/` subfolder are found at runtime. FMOD codec plugins are loaded dynamically and registered via `SupportedFormats.RegisterLoadedCodec()`.

## Key Dependencies

- **FMOD** (`fmod.dll`) — Core audio engine (native, bundled in `Resources/`)
- **MIDI backends** — Three options selectable via `MidiRendererBackend` enum:
  - `XArkMidi` (default) — Custom native engine (`XArkMidiEngine.dll`), supports SF2/DLS
  - `FluidSynth` / `nfluidsynth` — Full General MIDI with soundfonts
  - `BassMidi` — Third-party Bass MIDI renderer
- **ATL** (`z440.atl.core`) — Audio tag reading/writing
- **Vortice** (`Vortice.Direct2D1`, `Vortice.DirectWrite`, `Vortice.WIC`) — Direct2D waveform rendering
- **DiscordRichPresence** — Discord status integration

<!-- rtk-instructions v2 -->
# RTK (Rust Token Killer) - Token-Optimized Commands

## Golden Rule

**Always prefix commands with `rtk`**. If RTK has a dedicated filter, it uses it. If not, it passes through unchanged. This means RTK is always safe to use.

**Important**: Even in command chains with `&&`, use `rtk`:
```bash
# ❌ Wrong
git add . && git commit -m "msg" && git push

# ✅ Correct
rtk git add . && rtk git commit -m "msg" && rtk git push
```

## RTK Commands by Workflow

### Build & Compile (80-90% savings)
```bash
rtk cargo build         # Cargo build output
rtk cargo check         # Cargo check output
rtk cargo clippy        # Clippy warnings grouped by file (80%)
rtk tsc                 # TypeScript errors grouped by file/code (83%)
rtk lint                # ESLint/Biome violations grouped (84%)
rtk prettier --check    # Files needing format only (70%)
rtk next build          # Next.js build with route metrics (87%)
```

### Test (90-99% savings)
```bash
rtk cargo test          # Cargo test failures only (90%)
rtk vitest run          # Vitest failures only (99.5%)
rtk playwright test     # Playwright failures only (94%)
rtk test <cmd>          # Generic test wrapper - failures only
```

### Git (59-80% savings)
```bash
rtk git status          # Compact status
rtk git log             # Compact log (works with all git flags)
rtk git diff            # Compact diff (80%)
rtk git show            # Compact show (80%)
rtk git add             # Ultra-compact confirmations (59%)
rtk git commit          # Ultra-compact confirmations (59%)
rtk git push            # Ultra-compact confirmations
rtk git pull            # Ultra-compact confirmations
rtk git branch          # Compact branch list
rtk git fetch           # Compact fetch
rtk git stash           # Compact stash
rtk git worktree        # Compact worktree
```

Note: Git passthrough works for ALL subcommands, even those not explicitly listed.

### GitHub (26-87% savings)
```bash
rtk gh pr view <num>    # Compact PR view (87%)
rtk gh pr checks        # Compact PR checks (79%)
rtk gh run list         # Compact workflow runs (82%)
rtk gh issue list       # Compact issue list (80%)
rtk gh api              # Compact API responses (26%)
```

### JavaScript/TypeScript Tooling (70-90% savings)
```bash
rtk pnpm list           # Compact dependency tree (70%)
rtk pnpm outdated       # Compact outdated packages (80%)
rtk pnpm install        # Compact install output (90%)
rtk npm run <script>    # Compact npm script output
rtk npx <cmd>           # Compact npx command output
rtk prisma              # Prisma without ASCII art (88%)
```

### Files & Search (60-75% savings)
```bash
rtk ls <path>           # Tree format, compact (65%)
rtk read <file>         # Code reading with filtering (60%)
rtk grep <pattern>      # Search grouped by file (75%)
rtk find <pattern>      # Find grouped by directory (70%)
```

### Analysis & Debug (70-90% savings)
```bash
rtk err <cmd>           # Filter errors only from any command
rtk log <file>          # Deduplicated logs with counts
rtk json <file>         # JSON structure without values
rtk deps                # Dependency overview
rtk env                 # Environment variables compact
rtk summary <cmd>       # Smart summary of command output
rtk diff                # Ultra-compact diffs
```

### Infrastructure (85% savings)
```bash
rtk docker ps           # Compact container list
rtk docker images       # Compact image list
rtk docker logs <c>     # Deduplicated logs
rtk kubectl get         # Compact resource list
rtk kubectl logs        # Deduplicated pod logs
```

### Network (65-70% savings)
```bash
rtk curl <url>          # Compact HTTP responses (70%)
rtk wget <url>          # Compact download output (65%)
```

### Meta Commands
```bash
rtk gain                # View token savings statistics
rtk gain --history      # View command history with savings
rtk discover            # Analyze Claude Code sessions for missed RTK usage
rtk proxy <cmd>         # Run command without filtering (for debugging)
rtk init                # Add RTK instructions to CLAUDE.md
rtk init --global       # Add RTK to ~/.claude/CLAUDE.md
```

## Token Savings Overview

| Category | Commands | Typical Savings |
|----------|----------|-----------------|
| Tests | vitest, playwright, cargo test | 90-99% |
| Build | next, tsc, lint, prettier | 70-87% |
| Git | status, log, diff, add, commit | 59-80% |
| GitHub | gh pr, gh run, gh issue | 26-87% |
| Package Managers | pnpm, npm, npx | 70-90% |
| Files | ls, read, grep, find | 60-75% |
| Infrastructure | docker, kubectl | 85% |
| Network | curl, wget | 65-70% |

Overall average: **60-90% token reduction** on common development operations.
<!-- /rtk-instructions -->

<!-- code-review-graph MCP tools -->
## MCP Tools: code-review-graph

**IMPORTANT: This project has a knowledge graph. ALWAYS use the
code-review-graph MCP tools BEFORE using Grep/Glob/Read to explore
the codebase.** The graph is faster, cheaper (fewer tokens), and gives
you structural context (callers, dependents, test coverage) that file
scanning cannot.

### When to use graph tools FIRST

- **Exploring code**: `semantic_search_nodes` or `query_graph` instead of Grep
- **Understanding impact**: `get_impact_radius` instead of manually tracing imports
- **Code review**: `detect_changes` + `get_review_context` instead of reading entire files
- **Finding relationships**: `query_graph` with callers_of/callees_of/imports_of/tests_for
- **Architecture questions**: `get_architecture_overview` + `list_communities`

Fall back to Grep/Glob/Read **only** when the graph doesn't cover what you need.

### Key Tools

| Tool | Use when |
|------|----------|
| `detect_changes` | Reviewing code changes — gives risk-scored analysis |
| `get_review_context` | Need source snippets for review — token-efficient |
| `get_impact_radius` | Understanding blast radius of a change |
| `get_affected_flows` | Finding which execution paths are impacted |
| `query_graph` | Tracing callers, callees, imports, tests, dependencies |
| `semantic_search_nodes` | Finding functions/classes by name or keyword |
| `get_architecture_overview` | Understanding high-level codebase structure |
| `refactor_tool` | Planning renames, finding dead code |

### Workflow

1. The graph auto-updates on file changes (via hooks).
2. Use `detect_changes` for code review.
3. Use `get_affected_flows` to understand impact.
4. Use `query_graph` pattern="tests_for" to check coverage.
