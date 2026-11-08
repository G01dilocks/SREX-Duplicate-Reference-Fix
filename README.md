# SREX Duplicate Reference Fix

A [Synthesis](https://github.com/Mutagen-Modding/Synthesis) patcher for
Skyrim Special Edition that fixes duplicate references left behind by
SR Exterior Cities style transplants.

## The problem

SR Exterior Cities moves references out of the five city worldspaces
(Whiterun, Solitude, Windhelm, Riften, Markarth) into the corresponding
Tamriel cells. For most references that exist twice in vanilla Skyrim (a
"gated" copy inside the city worldspace and a "wall" copy in Tamriel), the
mod disables the Tamriel twin itself. A number of pairs were missed: both
copies end up enabled in the same Tamriel cell at the same position, so the
object renders twice — for example the Windhelm main gate stairs, chimney
smoke and palace lights.

## What this patcher does

Everything is re-derived from the final load order on every run:

1. Winning placed objects born in Skyrim.esm that sit in Tamriel cells are
   grouped by (cell, base object), then clustered by position.
   Already-disabled or deleted records are ignored, so fixes shipped by
   other mods win automatically.
2. A group of exactly two is a duplicate pair when one record was born in
   that very Tamriel cell (the native twin) and the other was born in a
   different, non-Tamriel worldspace (the transplant). Ambiguous groups are
   logged and left alone.
3. The native twin is disabled with the same pattern SR Exterior Cities
   itself uses: Initially Disabled flag, Enable Parent set to opposite of
   the Player, and Z position set to -30000.

Safety valves:

- **Base Object Swapper awareness.** If the transplanted side is disabled
  at runtime by a `*_SWAP.ini` transform (Initially Disabled flag or a move
  to Z <= -29000), the pair is skipped: the twin is then the only visible
  copy and must stay. Relative `posR` moves are evaluated against the
  winning Z. Swap inis are read from the Data folder root, where Base
  Object Swapper itself expects them.
- **Transplant filter.** By default only transplants whose override
  history includes `SR Exterior Cities.esp` are treated as transplants.
  The winning override does not have to be that plugin, so the fix still
  applies when a later patch — including the SR patcher's own output —
  wins the transplanted reference.
- **Manual skip list.** Individual FormKeys can be excluded in the
  settings.

## Installation

Add this repository to Synthesis as a **GitHub patcher** and place it after
the SR Exterior Cities patcher in the same group (or in a later group), so
it patches that patcher's output.

The patcher needs no masters beyond what the overridden records require
(Skyrim.esm). The output plugin name only matters when the patcher runs
standalone; inside a group, the group's output name wins.

## Settings

- **Transplant filter plugins** — only treat a reference as transplanted
  when one of these plugins appears anywhere in the transplanted
  reference's override history (the winning override does not have to be
  theirs). Defaults to `SR Exterior Cities.esp`; clear the list to accept
  transplants from any plugin.
- **Read Base Object Swapper inis** — scan `*_SWAP.ini` files in the Data
  folder and skip pairs whose transplanted side is disabled by a BOS
  transform. On by default.
- **Skip FormIDs** — FormKeys that must never be disabled, in the format
  `09A500:Skyrim.esm`.
- **Verbose logging** — log every candidate group that was inspected,
  including the ones that were skipped.

## Scope

- Placed objects (REFR) only; placed NPCs and traps are not inspected.
- Positions must agree to within 1 unit on every axis to count as
  duplicates; rotation is not compared.
- Only pairs are handled. Groups of three or more are logged as ambiguous
  and left alone.

## Building

```
dotnet build -c Release
```

Built against Mutagen 0.54.4 / Synthesis 0.36.6 (.NET 9).
