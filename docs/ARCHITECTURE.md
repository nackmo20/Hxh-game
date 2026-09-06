# Architecture technique

## Runtime Unity

- `RuntimeBootstrap` instancie le graphe minimal depuis une scène stable.
- `GameDirector` orchestre les modes sans mélanger persistance, locomotion et simulation tactique.
- `PlayerExplorer` traite déplacement, collision et interaction physique.
- `StrategicCombat` est une simulation C# déterministe indépendante de l'interface.
- `SaveSystem` effectue une écriture atomique avec copie de secours.
- `WorldFactory` fournit actuellement le décor original temporaire du vertical slice.

Les prochaines itérations doivent extraire dialogue, quêtes, inventaire, relations, factions, voyage et audio en services dédiés avant d'augmenter le contenu.

## Séparation anti-spoiler

`GameMaster/` est la source interne protégée. Les rapports publics n'énumèrent ni n'interprètent ses entrées. `PLAYER_README.md` et `BUILD_REPORT_SAFE.md` constituent les deux surfaces documentaires joueur.

## Pipeline

`HxHGameBuilder/build_game.py` orchestre validation, Blender, tests Unity et construction Windows. `AutoBuilder.Build` est l'unique point d'entrée de construction Unity en batchmode.

