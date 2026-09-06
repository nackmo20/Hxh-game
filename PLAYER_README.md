# Hunter × Hunter — RPG solo (fan game privé)

## Statut

Ce dépôt contient le socle jouable du RPG : exploration 3D, interactions physiques, conversation à choix, épreuve de combat tactique, progression comportementale invisible et sauvegarde locale.

Le projet est un fan game personnel, privé et non commercial. Les ressources produites dans ce dépôt sont originales ; aucune ressource extraite du manga ou de l'anime n'est incluse.

## Lancer dans Unity

1. Installer **Unity 2022.3 LTS** avec le module Windows Build Support.
2. Ouvrir le dossier du dépôt dans Unity Hub.
3. Ouvrir `Assets/Scenes/Bootstrap.unity`, puis appuyer sur **Play**.

### Contrôles

- `WASD` / flèches : déplacement ;
- `Maj gauche` : courir ;
- `E` : interagir avec l'élément devant le personnage ;
- `F5` : sauvegarder dans le slot 1 ;
- souris : choisir les actions contextuelles et tactiques.

## Construire automatiquement sous Windows

Double-cliquer sur `BUILD_GAME.bat`. Le pipeline met à jour Git, valide le projet, génère les ressources Blender si Blender est détecté, exécute les tests Unity, puis produit `Builds/Windows/HxHGame.exe`.

Si Unity n'est pas dans le `PATH`, lancer :

```bat
BUILD_GAME.bat --unity "C:\Program Files\Unity\Hub\Editor\2022.3.62f1\Editor\Unity.exe"
```

Le seul rapport destiné au joueur est `BUILD_REPORT_SAFE.md`. Les journaux techniques restent dans `HxHGameBuilder/logs/`.

