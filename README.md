# Projet pour la gestion stages en SPU

Ce projet est un site web permettant à gérer les stages en SPU.

## Fonctionnement de l'application

### Section horaire

La section horaire permet aux maîtres de stages(mds) et aux stagiaires de gérer leur horaire. Le mds rentre son horaire dans le système lors de la création de son compte, et le stagiaire assigné à celui-ci peut le consulter à toute occasion. Le système prend en compte la récurrence par 2 semaines de l'horaire du mds et la gestion des abscences pour les mds et les stagiaires. Une plage horaire peut être mise à jour en tout moment par le mds en appuyant dessus. C'est là comme ça que le mds peut ajuster les heures de la plage horaire et que le mds et le stagiaire peut se mettre comme abscent. 

### Section Chat

La section Chat permet aux utilisateurs de l'application de communiquer entre eux sous forme de messagerie instantannée. Les stagiaires et les mds ont uniquement accès à un seul chat: celui entre eux et avec l'enseignant du stagiaire. Les enseignants ont accès à tout les chats de leurs étudiants et le coordonateur a accès aux chats de tout les stagiaires. Le chat ne supporte pas les images ou les vidéos mais supporte tous les types de textes, incluant les émoticônes. Pour les enseignants et le coordonateur, la barre de recherche en haut de la liste des chat permet de filtrer les chats rapidement.

### Section Évaluations

La section des évaluations permet aux enseignants de diffuser des liens forms à ses étudiants et aux mds de ses étudiants. Il peut rapidement actver ou désactiver les évaluations individuellement pour chaque utilisateur et voir si ceux-ci les ont consultés. Les étudiants et les mds peuvent voir les évaluations qui ont été activés et accéder rapidement au form, en appuyant sur le lien. Cette action marque automatiquement l'évaluation comme consultée dans le système. Il est important que l'enseignant mette le lien complet du form comme ceci: "https://exemple.com/".

### Gestion des données

Le coordonateur est celui qui s'occupe des informations dans le système. À partir de sa page d'administration, le coordo peut assigner un mds à un stagiaire, créer des utilisateurs, mettre à jours des informations des utilisateurs et bien d'autres. Le informations du système peuvent être exportées sous forme de excel par le coordonateur à tout moment.

## Limitations du programme


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
