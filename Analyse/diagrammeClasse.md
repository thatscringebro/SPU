# Analyse détaillée


```plantuml

hide circle


class Personne {
    int : id
    string : nom
    string : prenom
    string : courriel
    int : telephone
}

class Coordonateur {
    Assigner(mds, stagiaire)
}

class Enseignant {
    Suivre(stagiaire)
}

class "Maitre de stage" AS MDS {
    Evaluer(stagiaire)
    string : idCaserne
    string : NomEntreprise
    int : idStagiaire
    int : idEntreprise
}

class Stagiaire {
    int : idEnseignant
    int : idMds
}

class Entreprise {
    int : id
    string : adresse
    int : telephone
}



```
