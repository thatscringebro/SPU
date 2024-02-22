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

### Fonctionnalités générales

- **Connexion/ Déconnexion**: Complet
  - Lorsque non connecté, l'accès est restreint aux boutons Messagerie, horaire, évaluations et ne permet pas de changer de page.

### En tant que coordonnateur

#### Page Gestion:

- Ajout d'utilisateurs
- Attribution de liens aux futurs utilisateurs
- Mise à jour et suppression des utilisateurs
- Recherche des utilisateurs par catégorie dans la barre de recherche
- Réduction des barres de catégorie
- Exportation des données

**Limites:**
- Exportation de toutes les données sans option de choix
- Suppression automatique après un an non implémentée
- Modification du mot de passe indisponible
- Recherche limitée aux mots complets
- Risque de plantage si aucun horaire défini
- Risque de suppression accidentelle d'une plage horaire du mds

#### Actions spécifiques:

- **Mise à jour:**
  - Stagiaire: Modification possible du courriel si autorisation donnée
  - Autres utilisateurs: Message d'erreur en cas d'informations incorrectes, impossibilité de choisir un nom d'utilisateur existant.

- **Suppression:**
  - Supprime la personne sélectionnée.

- **Détail:**
  - Affichage des détails de la personne sélectionnée.
  - Pour les stagiaires ayant autorisé le partage d'informations, affichage du courriel et du numéro de téléphone.

- **Copier un lien:**
  - Copie le lien du formulaire pour inscrire la catégorie choisie.

- **Barre noire:**
  - Rétracte la catégorie.

- **Recherche d'utilisateur:**
  - Recherche toutes les données.

- **Horaire pour les mds:**
  - Affiche l'horaire du mds sélectionné, le nom du stagiaire et les dates de stage.
  - Possibilité de visualiser les semaines suivantes et les couleurs de plage pour détecter les absences.

### Page Messagerie:

- Consultation du chat d'un étudiant sélectionné
- Recherche d'un étudiant dans la liste

**Limite Messagerie:** Le coordonnateur peut discuter avec tous les chats.

### Page Liaison:

**Limites:**
- Attribution d'un maître de stage à un stagiaire uniquement si un enseignant est sélectionné et une date de stage est entrée.
- Impossible de changer le mds1 d'un stagiaire si un mds2 est déjà assigné.
- Interdiction de choisir deux fois le même maître de stage dans mds1 et mds2.
- Impossibilité d'assigner un mds2 à un stagiaire sans avoir choisi de mds1.
- Absence de validation pour les dates de stages et l'horaire du mds, mais possibilité de les modifier ultérieurement.

### Ajout d'utilisateur:

- Redirection vers la page de choix d'utilisateur
- Options disponibles: Stagiaire, Coordonnateur, Enseignant, Maître de stage, Employeur

**Limites:**
- Nom d'utilisateur unique requis
- Nom/prénom et courriel entre 3 et 25 caractères
- Format valide pour le courriel et le numéro de téléphone
- Sélection de l'école obligatoire
- Mot de passe avec au moins 8 caractères, une majuscule, un chiffre et un symbole
- Confirmation du mot de passe identique

**Pour le stagiaire:**
- Case à cocher pour partager le courriel et le numéro de téléphone avec le coordonnateur

**Pour les autres rôles:** Aucune différence dans les limites

### Déconnexion:

- Déconnexion réussie lors de l'appui sur le bouton.


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
