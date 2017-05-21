# Voxel Map Editor

This is a Unity3D project that refactors [AlexStv's Voxel Tutorial][1].

Things I've changed:

- [x] Standarized code style.
- [x] Move everything inside the `Voxels` namespace.
- [x] Removed terrain generators. No caves, no trees.
- [x] Removed noise-related code.
- [x] Blocks are represented as `int`s instead of `Block` subclasses.
- [x] "Infinite size" map editor out of the box, start with a single block and build huge worlds.
- [x] `World` serialization uses gzip compression.
- [x] Modified `tiles.png` with two new tiles: `sand` and `error`.
- [x] Refactored some magic numbers to `Assets/MapEditor/MapConstants.cs`.

## Why `int` instead of `GrassBlock : Block` ?

Blocks are static so just an id reference to some container that holds each block details should suffice for most cases.

This has the benefit of making serialization super easy and space efficient.

## Map editor controls

- `LMB` adds the current block to wherever you click, if possible. Default: Stone.
- `Tab` switches between **Add** and **Replace** modes. Default: Add.
- `1` switches current block to `Cobblestone`
- `2` switches current block to `Stone`
- `3` switches current block to `Dirt`
- `4` switches current block to `Grass`
- `5` switches current block to `Sand`
- `9` switches current block to `Error`
- `0` switches current block to `Air`
- `X` toggle burst mode for `LMB` (`Input.GetKey` vs `Input.GetKeyDown`)
- `B` backup world whatever is configured in `MapConstants.SaveFiles`. Default: `<root>/maps`.
- `RMD` + `WASD` to move around the scene
- `RMD` + `E` to move upwards
- `RMD` + `Q` to move downwards
- `Escape` pauses Unity Editor if in Play Mode

## Map editor: two modes?

- **Add** adds a block next to the clicked block.
- **Replace** replaces the clicked block.

## Licensing

I'm following the same licensing as AlexStv's original tutorial which can be found [here][2].

Basically you are free to use this project however you like.

The only asset that's not allowed for commercial usage is the `Assets/MapEditor/tiles.png`.

It's very easy to replace with one made by yourself.

[1]: http://alexstv.com/index.php/category/voxel-tutorial
[2]: http://alexstv.com/index.php/posts/unity-voxel-tutorial-licencing
