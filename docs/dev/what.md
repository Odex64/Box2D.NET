### Dictionary:

- The word "package" refers to the entire Box2D framework.
- The word "Box2D" refers to the Box2D framework/API, version **2.4.2**, found [here](https://github.com/erincatto/box2d/tree/v2.4.2).
- The word "overwrite" refers to the modification of data in memory, not a file itself.
- The phrase "Binary tree" describes how the code is organized as a file structure (or tree).
- The letters `I` and `O` directly represent I/O, where `I` means input and `O` output.

---

### Notes:

- It is strongly recommended to consult the Box2D documentation and source code for further information.
- Any standard C/C++ library definitions are not included in this document.
- This document provides only a brief explanation of the files in `include/*`.
- In the [Binary tree](#binary-tree), every file after `b2_api.h` and `b2_settings.h` likely depends on these two in some way, unless otherwise noted.
- `b2_dynamic_tree.h` and `b2_broad_phase.h` likely represent I/O operations, where `b2_dynamic_tree.h` is input (`I`) and `b2_broad_phase.h` is output (`O`).

---

### Binary tree (v1):

- `include/box2d/*` (regex: `b2_*.h`):
  - `box2d` (**Can be ignored**): Includes everything, in order.
  - `types`: Defines specific types.
  - `api`: Defines how the environment is presented. This file is likely important because it detects how the user is handling the `settings` file.
  - `settings`: Defines user settings. This file can overwrite aspects of the package or itself. Depends on `types` and `api`.
    - `user_settings`: The definition of this file is unknown. It probably does something if the user defines a setting, but it seems not to exist until the package is used.
  - `common`: Defines constant attributes.
  - `math`: Defines every math operation or definition implemented in the package.
  - `draw`: Provides a way to easily draw on the screen. Depends on `math`.
  - `timer`: Defines a base class for timing (as a timer) profiles. Direct definition: _Timer for profiling._
  - `collision`: Defines a way to detect contact points in a 2D Cartesian plane. Direct definition: _Structures and functions used for computing contact points, distance queries, and TOI queries._
  - `shape`: Defines a base class that describes the body or structure of an object's shape in the 2D Cartesian plane. It also helps with collision detection. Depends on `collision`. Direct definition: _A shape is used for collision detection._
    - Files that depend on `shape` (regex: `b2_*_shape.h`): `chain`, `circle`, `edge`, `polygon`.
  - `growable_stack`: A simple linked list.
  - `dynamic_tree` (**I**): Defines "nodes" in the pointer pool, simplifying collision detection. Directly using pointers is likely problematic. Depends on `collision` and `growable_stack`. Direct explanation: _Nodes are pooled and relocatable, so we use node indices rather than pointers._
  - `broad_phase` (**O**): Defines a way to report collisions between nodes using ray casts. Depends on `collision` and `dynamic_tree`. Direct explanation: _The broad-phase is used for computing pairs and performing volume queries and ray casts. This broad-phase does not persist pairs. Instead, it reports potentially new pairs._
