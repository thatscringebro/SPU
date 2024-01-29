# Analyse détaillée


```plantuml

hide circle


class "Personne" AS ps {
    + id : int 
    + nom : string
    + prenom : string
    + courriel : string 
    + telephone : int
}

class "Coordonateur" AS co {
    Assigner(mds, stagiaire)
}

class "Enseignant" AS ens {
    Suivre(stagiaire)
}

class "Maitre de stage" AS Mds {
    Evaluer(stagiaire)
    + idCaserne : string
    + NomEntreprise : string
    + idStagiaire : int
    + idEntreprise : int
}

class "Stagiaire" AS st{
    + idEnseignant : int
    + idMds : int
}

class Stage {
    + idStage : int
    + lieu : string
}

class "Entreprise" AS ent {
    + id : int
    + adresse : string
    + telephone : int
}

class Horaire {
    + id : int
    + idMds : int
    + idStagiaire : int
    CreerDisponibilite(jour, heure)
}




```
