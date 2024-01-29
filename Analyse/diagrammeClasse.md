# Analyse détaillée


```plantuml

hide circle


class "Utilisateur" AS User {
    - id : int 
    - nom : string
    - prenom : string
    - courriel : string 
    - telephone : int
    - SeConnecter()

}

class "Coordonateur" AS co {
    + Assigner(mds, stagiaire)
}

class "Enseignant" AS ens {
    + Suivre(stagiaire)
}

class "Maitre de stage" AS Mds {
    + Evaluer(stagiaire)
    - idCaserne : string
    - NomEntreprise : string
    - idStagiaire : int
    - idEntreprise : int
}

class "Stagiaire" AS st{
    - idEnseignant : int
    - idMds : int

    + Ajouter()
    + Consulter()
    + MettreAJour()
    + Rechercher()
}

class Stage {
    - idStage : int
    - lieu : string
    - idEntreprise : int
    - dateDebutStage : dateTime
    - dateFinStage : dateTime
}

class "Entreprise" AS ent {
    - id : int
    - adresse : string
    - telephone : int

    + ConsulterLstStagiaire()
}

class Horaire {
    - id : int
    - idMds : int
    - idStagiaire : int
    + CreerDisponibilite(jour, heure)
}




```
