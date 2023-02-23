## Assets folder

Try to use subfolders as much as possible. My rule of thumb is that if there are so many assets in a folder that I need to actually scroll down on the Unity file explorer, then I need to split them up into various folders.



## Scene hierarchy

1. Game Managers
2. Player
3. Environment
4. Visual Effects / Post-Processing



## GameObject components

1. Transform
2. Renderers (*e.g.* Mesh, Sprite, Canvas)
3. RigidBody
4. Colliders
   1. Non-trigger
   2. Trigger
5. [any other Components]
6. Scripts



## Naming

- **Folders:** Capitalized words with spaces (*e.g.* `Environment Textures`)
- **Files:** Upper Camel Case (*e.g.* `TitleBackground.png`)
- **GameObjects**
  - **Functional:** Upper Camel Case (*e.g.* `PuzzleInteractable1`)
  - **Organizational:** Capitalized words with spaces (*e.g.* `Game Managers`)
- **Programming**
  - **Scripts**
    - Controller (moves/manipulates something)
    - Manager (manages/reads other scripts)
  - **Variables**
    - **Private:** Lower Camel Case (*e.g.* `myVariable`)
    - **Public:** Upper Camel Case (*e.g.* `MyVariable`)
  - **Methods:** Upper Camel Case (*e.g.* `MyMethod`)
  - **Comments**
    - **Class/Method:** `///` comments (with parameters/returns/etc.)
    - **In-code:** `//` comments