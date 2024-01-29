```plantuml

hide circle

class "Coordonateur" AS co {
    + id : int 
    + nom : string
    + prenom : string
    + courriel : string 
    + telephone : int
}

class "Enseignant" AS enseignant {
    + id : int 
    + nom : string
    + prenom : string
    + courriel : string 
    + telephone : int
}

class "Stagiaire" AS stagiaire {
    + id : int 
    + nom : string
    + prenom : string
    + courriel : string 
    + telephone : int

    + *idEnseignant : int
    + *idMds : int
}

class "Message" AS message {
    + id : int
    + message : string
    + date&heure : string

    + *idutilisateur : int
}


class "MDS" AS Mds {
    + id : int
    + idMatricule : string
    + status : enum
    + civilité
    + nom&prénom : string
    + nom : string
    + prénom : string
    + telMaison : string
    + telCellulaire : string
    + courriel : string
    + CISSS/CIUSSS : string
    + employeur : string
    + Accréditation
    + commentaires : string
    + commentairesCIUSS : string

    + *idStagiaire : int
    + *idEntreprise : int
}



' Un entreprise peut avoir plusieurs maitre de stage
' Un maitre de stage peut avoir plusieurs entreprise

class EntrepriseMDS {
    idEntreprise : int
    idMDS : int
}

class "Entreprise" AS ent {
    + id : int
    + adresse : string
    + telephone : int

    + *idMds : int
}

class Horaire {

    PlageHoraire plageHoraire
    + id : int

    + *idMds : int
    + *idStagiaire : int
}

class Evaluation {
    + id : int
    + GoogleForms : GoogleForms

    + *idStagiaire : int
    + *idMds : int
}

class AutoEvaluation {
    + id : int
    + GoogleForms : GoogleForms

    + *idStagiaire : int
    + *idMds : int
}



class PlageHoraire {
    id : int
    heureDebut : int
    minutesDebut : int
    heureFin : int
    minutesFin : int
    journeeSemaine : string
    jour : int
    mois : string
    année : int

    + *idHoraire : int
}

class Contract {
    id : int
    Forms : Forms
    idStagiaire : int
    idMds : int
}

Horaire "1" --- "0..*" PlageHoraire

Mds "0..*" -- "1" EntrepriseMDS
EntrepriseMDS "1" -- "0..*" ent

Mds "1..*" -- "1" stagiaire

enseignant "1" --- "0..*" stagiaire

message "0..*" -- "1" stagiaire
message "0..*" -- "1" Mds
message "0..*" -- "1" enseignant

Evaluation "1" -- "1" Mds 
Evaluation "1" -- "1" stagiaire

AutoEvaluation "1" -- "1" stagiaire 
AutoEvaluation"1" -- "1" Mds

Contract "1" -- "1" stagiaire
Mds "1" -- "1..*" Contract

Horaire "1" -- "1" stagiaire
Horaire "1..*" -- "1" Mds

```

