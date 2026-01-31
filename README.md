# Falling Sand Simulation
Multithreaded falling sand game.
- Field is divided into 64x64 chunks
- Chunks updated from bottom to up with chess-like order so it allows to avoid synchronization and false sharing.
- Each chunk contains 4 squares (32x32) called activity zone. If there were no changes in last frame inside of square, it becomes disabled.
- Additionaly each activity zone has dirty rect (same as AABB box), so empty cells are ignored.
- Save file by using MemPack

[![PREVIEW](https://img.youtube.com/vi/VoVmgYxr6t8/0.jpg)](https://www.youtube.com/watch?v=VoVmgYxr6t8)
