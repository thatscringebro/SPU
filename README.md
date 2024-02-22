# Projet pour la gestion stages en SPU

Ce projet est un site web permettant de gérer les stages dans le programme de SPU.

## Fonctionnement de l'application

### Section horaire

La section Horaire permet aux maîtres de stages(mds) et aux stagiaires de gérer leur horaire. Le mds ajoute son horaire dans le système lors de la création de son compte, et le stagiaire assigné à celui-ci peut le consulter à toute occasion. Le système prend en compte la récurrence par 2 semaines de l'horaire du mds et la gestion des absences pour les mds et les stagiaires. Une plage horaire peut être mise à jour à tout moment par le mds en appuyant dessus. C'est ainsi que le mds peut ajuster les heures de la plage horaire et que le mds et le stagiaire peuvent se mettre comme étant absents. 

### Section Chat

La section Chat permet aux utilisateurs de l'application de communiquer entre eux sous forme de messagerie instantanée. Les stagiaires et les mds ont uniquement accès à un seul chat: celui entre eux et avec l'enseignant du stagiaire. Les enseignants ont accès à tout les chats de leurs étudiants et le coordonnateur a accès aux chats de tout les stagiaires. Le chat ne supporte pas les images ou les vidéos, mais supporte tous les types de textes, incluant les émoticônes. Pour les enseignants et le coordonnateur, la barre de recherche en haut de la liste des chats permet de filtrer les chats rapidement.

### Section Évaluations

La section Évaluations permet aux enseignants de mettre à disposition de leurs étudiants des liens forms et aux mds de ses étudiants. Il peut rapidement activer ou désactiver les évaluations individuellement pour chaque utilisateur et voir si ceux-ci les ont consultées. Les étudiants et les mds peuvent voir les évaluations qui ont été activés et accéder rapidement au form, en appuyant sur le lien. Cette action marque automatiquement l'évaluation comme consultée dans le système. Il est important que l'enseignant mette le lien complet du form comme ceci: "https://exemple.com/".

### Gestion des données

Le coordonnateur est celui qui s'occupe des informations dans le système. À partir de sa page d'administration, le coordo peut assigner un mds à un stagiaire, créer des utilisateurs, mettre à jours des informations des utilisateurs et bien d'autres. Les informations du système peuvent être exportées sous forme de excel par le coordonnateur à tout moment.

## Limitations du programme

# Notes d'utilisation du MDS

## Connexion du MDS

---

### Onglet Chat

- Lorsqu'il appuie sur l'onglet chat :
  - S'il a un stagiaire attitré :
    - Il peut voir le chat de lui, du stagiaire et du MDS2 si applicable.

---

### Onglet Horaire

- Lorsqu'il appuie sur "horaire" :
  - Les informations affichent :
    - Le nom des MDS.
    - Si MDS2 est null, il est indiqué : N/A.
    - Le nom du stagiaire s'il en a un.
    - Son accréditation.
    - Les dates de stages du stagiaire s'il en a un.
  - Il peut naviguer entre les pages pour voir la suite de l'horaire.
  - Pour ajouter des plages, il doit appuyer sur "Ajout d'une plage horaire".

---

### Ajout d'une plage horaire

- Lorsqu'il appuie sur "Ajout d'une plage horaire" :
  - Un formulaire s'ouvre.
  - Il doit entrer la date de début et de fin pour la journée qu'il veut ajouter.
  - Limites :
    - Les heures doivent faire sens (ex: commencer à 8h et finir à 7h n'est pas valide).
    - La date de début doit être la même que la date de fin.
    - L'ajout de récurrence ne fonctionne pas toujours.

---

### Modification d'une plage horaire

- Lorsqu'il appuie sur une plage horaire :
  - Il peut modifier les heures et les dates en respectant les formats demandés.
  - Il peut indiquer s'il est présent ou absent.
  - Il peut ajouter un commentaire (seulement s'il est absent).
  - Il peut supprimer la plage horaire.
  - Il peut annuler la modification ou confirmer le changement.

---

### Confirmation de la modification

- Lorsqu'il modifie une plage horaire :
  - Un message de confirmation s'affiche pour confirmer s'il veut vraiment modifier ou non.
  - S'il appuie sur "annuler", il retourne au formulaire de modification.
  - S'il appuie sur "oui", la modification est effectuée.
  - Si la plage horaire où il est absent est modifiée, elle devient jaune. Il peut alors rétablir sa présence en appuyant dessus.

---

### Évaluations

- Lorsqu'il appuie sur "Évaluations" :
  - Limite : Si aucune évaluation n'est assignée au MDS, la liste des évaluations sera vide.
  - S'il y a une évaluation assignée, il peut cliquer sur le lien pour accéder au formulaire.
  - Une fois le lien consulté, le statut de l'évaluation change.

---

## Déconnexion

- Le MDS peut se déconnecter à tout moment.

# Enseignant

### Gestion des élèves
- Enseignant arrive sur la page de gestion des élèves.
- Il voit la liste de ses élèves.

### Messagerie
- Lorsqu'il appuie sur Messagerie:
  - Il peut choisir un stagiaire.
  - Il peut faire une recherche dans la barre de recherche pour trouver un stagiaire.
  - Il partage le chat avec un stagiaire, et les mds du stagiaire.

### Horaire
- En appuyant sur Horaire:
  - Il voit la liste des horaires des stagiaires qui ont des horaires.
  - Limite : Si l'étudiant n'a pas d'horaire, il ne sera pas affiché.
  - S'il clique sur l'horaire d'un stagiaire :
    - Ça affiche l'horaire du cégep.
    - Limite : Parfois les plages horaires n'affichent pas.
    - Limite : Ne pas appuyer sur les plages horaires des horaires, car cela modifie les plages horaires.

### Évaluations
- En appuyant sur Évaluations:
  - Il peut ajouter une évaluation.
  - Une fois qu'il appuie sur Ajouter une évaluation:
    - Il doit écrire l'URL de l'évaluation.
    - Il peut choisir si c'est pour un stagiaire ou un MDS.
    - Il peut décider si les évaluations sont actives ou non.
  - Limite : Aucun message d'erreur si le formulaire d'ajout n'est pas bon, l'ajout ne se fait juste pas.
  - Limite : Lorsque des stagiaires sont ajoutés par le coordo à l'enseignant et que l'enseignant appuie sur l'icône pour modifier le formulaire, les stagiaires sont doublés.
  - Limite : Si on appuie trop souvent sur le bouton Modifier les évaluations, le formulaire n'affiche plus.
  - Limite : Si on ajoute 2 fois le même lien, cela double le groupe de stagiaires au lieu de donner un message d'erreur.
  - Il peut voir le nombre de ses étudiants qui ont consulté l'évaluation.
  - Il peut voir le nombre d'évaluations qui sont actives.
  - Il peut effacer une évaluation.

### Déconnexion
- En appuyant sur le bouton pour se déconnecter:
  - Il se déconnecte.

# Coordonateur

## Fonctionnalités générales

- **Connexion/Déconnexion**:
    - Log in - log out = complet
    - Lorsqu'on n'est pas connecté :
        - Les boutons Messagerie, Horaire, Évaluations sont inaccessibles
        - Impossible de changer de page sans être connecté

## Fonctionnalités pour les coordonnateurs

### Page Gestion

- Ajout d'utilisateurs
- Attribution de liens aux futurs utilisateurs
- Mise à jour et suppression de tous les utilisateurs
- Recherche des utilisateurs par catégorie dans la barre de recherche
- Réduction des barres de catégorie
- Exportation des données

#### Limites

- Exportation de toutes les données sans possibilité de sélection
- Suppression automatique après un an non implémentée
- Impossibilité de se supprimer soi-même (pour éviter de perdre l'accès de coordonnateur)
- Impossibilité de modifier le mot de passe
- Recherche fonctionne uniquement avec des mots complets
- Risque de plantage si un stagiaire n'a pas d'horaire
- Risque d'effacement de l'horaire du maître de stage en cliquant sur une plage horaire

#### Mise à jour

- Pour les stagiaires :
    - Si autorisation donnée, le courriel est modifiable
    - Sinon, le champ de courriel n'apparaît pas dans le formulaire
- Pour les autres utilisateurs :
    - En cas d'informations incorrectes, un message rouge indique le problème
    - Impossible d'utiliser un nom d'utilisateur déjà existant

#### Suppression

- Suppression de l'utilisateur sélectionné

#### Détail

- Affiche les détails de l'utilisateur sélectionné
- Pour les stagiaires, affichage du courriel et du numéro de téléphone si autorisé

#### Copier un lien

- Copie du lien du formulaire pour inscrire la catégorie choisie
- Limite : Vérification de l'URL avec le domaine dans la vue Manage.cshtml

#### Barre noire

- Réduit la catégorie affichée

#### Recherche d'utilisateur

- Recherche dans toutes les données

#### Horaire pour les maîtres de stage

- Redirige vers la page d'horaire du maître de stage sélectionné
- Affiche l'horaire, le nom du stagiaire et les dates de stage
- Permet de consulter les semaines suivantes
- Affiche les couleurs de plage pour indiquer les absences

### Page Messagerie

- Consultation du chat d'un étudiant sélectionné
- Recherche d'un étudiant dans la liste des chats

#### Limite

- Le coordonnateur peut discuter avec tous les chats

### Page Liaison

#### Limites

- Liaison possible entre un maître de stage et un stagiaire seulement si un enseignant est sélectionné et qu'une date de stage est renseignée
- Impossibilité de changer le maître de stage 1 si un maître de stage 2 est déjà assigné
- Impossibilité de choisir deux fois le même maître de stage dans les cases maître de stage 1 et maître de stage 2
- Impossibilité d'assigner un maître de stage 2 à un stagiaire sans avoir choisi un maître de stage 1
- Pas de validation pour les dates de stage et l'horaire du maître de stage, mais possibilité de les modifier ultérieurement

#### Actions

- Le bouton "relier" effectue l'action avec gestion des erreurs
- Le bouton "Gérer les utilisateurs" retourne à la vue de gestion

### Ajout d'utilisateur

- Choix du type d'utilisateur à ajouter
- Redirection vers le formulaire correspondant au type choisi
- Le bouton "retour" ramène à la page de gestion

#### Limites communes

- Nom d'utilisateur unique
- Nom/prénom de 3 à 25 caractères
- Format valide pour le courriel et le numéro de téléphone
- École sélectionnée
- Mot de passe de 8 caractères minimum avec majuscule, chiffre et symbole
- Confirmation du mot de passe identique

#### Limites spécifiques par type d'utilisateur

- **Stagiaire** :
    - Option de partager le courriel et le numéro de téléphone avec le coordonnateur
    - Bouton "retour" pour revenir à l'index
    - Création effectuée si le formulaire est valide

- **Coordonnateur** :
    - Aucune différence dans les limites
    - Bouton "retour" pour revenir à l'index
    - Création effectuée si le formulaire est valide

- **Enseignant** :
    - Aucune différence dans les limites
    - Bouton "retour" pour revenir à l'index
    - Création effectuée si le formulaire est valide

- **Maître de stage** :
    - Limites communes
    - Matricule composé d'une lettre et de quatre chiffres
    - Entreprise non vide
    - Civilité non vide
    - Type d'employeur non vide
    - Téléphone fixe valide
    - Bouton "retour" pour revenir à l'index
    - Création effectuée si le formulaire est valide

- **Employeur** :
    - Limites communes
    - Nom d'utilisateur doit être le nom de l'entreprise
    - Adresse requise
    - Code postal de format valide
    - Bouton "retour" pour revenir à l'index
    - Création effectuée si le formulaire est valide

### Déconnexion

- La déconnexion est effectuée

# Stagiaire
- Peut se connecter, une fois connecté il est redirigé vers la page d'accueil.
- Il peut choisir entre la messagerie, l'horaire, les évaluations et se déconnecter.

## Quand on appuie sur "Chat" :
- Le stagiaire a accès à son chat. S'il y a des messages, il les voit ; s'il n'y en a pas, il ne voit rien.
- Limite : Il peut envoyer des messages même s'il est seul dans son chat.

## Quand on appuie sur "Horaire" :
- Limite : Aucune plage horaire n'est assignée, un message indique que l'horaire est indisponible.
- Limite : Quand un stagiaire est absent, la plage horaire devient grise (pour lui et pour le mds).
- Limite : Quand un stagiaire n'est pas absent finalement, il peut recliquer sur la même plage horaire (où il est absent) et indiquer qu'il est présent, la plage redevient bleue.
- Si un mds est assigné : L'horaire affiche les plages horaires du mds1 et non du 2. Un affichage avec le nom des mds, du stagiaire, et les dates du stage du stagiaire est affiché. Son horaire est affiché en fonction de son stage -> des dates de stage. Il peut appuyer sur une plage horaire pour indiquer s'il est absent ou si un mds est absent ou pas.

## Quand on appuie sur "Évaluations" :
- Limite : Si aucune évaluation en ligne, aucune évaluation n'est affichée.
- Si des évaluations sont en ligne : Il y a une liste d'évaluation, il peut cliquer sur le lien. Le statut change s'il est consulté ou pas.

## Se déconnecte :
- Se déconnecte avec succès.

## Employeur

- Peut se connecter.
- Il est redirigé vers une page de gestion où il peut voir la liste de ses maîtres de stage.
- L'employeur peut appuyer sur le bouton "Horaire" du maître de stage dans la liste.
  - Limite : Si le mds n'a pas d'horaire, un message d'erreur apparaît.
  - Si le mds a un horaire, l'horaire du mds s'affiche.
  - Limite : Ne pas toucher aux plages horaires sinon on peut les effacer.
  - Limite : Les plages horaires ne s'affichent pas toujours.
- La déconnexion fonctionne.


## Développement et déploiement

Ce guide fournit des instructions pour développer l'application dotnet 7.0 basée sur ASP.NET MVC avec Identity Framework pour la gestion des utilisateurs et Entity Framework pour la couche d'accès aux données. La base de données PostgreSQL est utilisée avec Entity Framework en mode code-first.

## Prérequis

Avant de commencer à développer l'application, assurez-vous d'avoir installé les outils suivants :

- .NET SDK 7.0
- PostgreSQL
- Un éditeur de texte ou un IDE compatible avec le développement dotnet (par exemple, Visual Studio Code, Visual Studio, Rider, etc.)

## Configuration de la base de données

Assurez-vous que PostgreSQL est installé et configuré sur votre système. Vous pouvez modifier les paramètres de connexion à la base de données dans le fichier `appsettings.json`.

## Installation des dépendances

Pour installer les dépendances nécessaires à l'application, exécutez la commande suivante dans le répertoire racine de l'application :

```bash
dotnet restore
```

## Migration de la base de données

Avant de lancer l'application, assurez-vous d'avoir migré la base de données en exécutant les migrations. Les migrations sont généralement ignorées dans le système de contrôle de version (git), donc vous devez les générer localement.

Pour créer une nouvelle migration, exécutez la commande suivante dans le répertoire racine de l'application :

```bash
dotnet ef migrations add init
```

Ensuite, appliquez les migrations à la base de données avec la commande suivante :

```bash
dotnet ef database update
```

Ces commandes vont créer et mettre à jour la base de données en fonction des modèles de données définis dans votre application.

## Lancement de l'application

Pour lancer l'application, exécutez la commande suivante dans le répertoire racine de l'application :

```bash
dotnet run
```

Cela va compiler et exécuter l'application dotnet. Vous pouvez accéder à l'application à l'adresse suivante : `http://localhost:5000` (peut être différente selon votre environnement de travail, veuillez consulter le output dans le terminal).


| UserName github | Programmeur |
|---|---|
| yalaoumbaca | Gabriel Bruneau |
| Claudel-D-Roy  | Claudel D Roy  |
| thatscringebro | Merlin Gélinas |
| FrancoisLampron  | François Lampron  |
| SynsWolf | Syntich Makougang |
