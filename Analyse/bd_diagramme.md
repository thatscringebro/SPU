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

class "Chat" as chat {
    + id : int

    + *idStagiaire
    + *idMds
    + *idEnseignant
}


class "MDS" AS Mds {
    + id : int
    + idMatricule : string
    + nom : string
    + prenom : string
    + nom&prénom : string
    + courriel : string
    + status : enum
    + civilité : enum
    + telMaison : string
    + telCellulaire : string
    + CISSS/CIUSSS : string
    + CISSS/CIUSSS2 : string
    + employeur : string
    + employeur2 : string
    + Accréditation
    + commentairesPRIVÉ : string
    + commentairesCIUSSPRIVÉ : string

    + *idStagiaire : int
    + *idEntreprise : int
}



' Un entreprise peut avoir plusieurs maitre de stage
' Un maitre de stage peut avoir plusieurs entreprise

class EntrepriseMDS {
    + *idEntreprise : int
    + *idMDS : int
}

class "Entreprise" AS ent {
    + id : int
    + nom : string
    + adresse : string
    + telephone : int

    + *idMds : int
}

class Horaire {
    + id : int

    + *idMds : int
    + *idStagiaire : int
}

class Evaluation {
    + id : int
    + LienGoogleForms : string
    + EstStagiaire : bool
    + Actif : bool
    + Consulte : bool
    

    + *idStagiaire : int
    + *idMds : int
}



class PlageHoraire {
    id : int
    ' heureDebut : int
    ' minutesDebut : int
    ' heureFin : int
    ' minutesFin : int
    ' journeeSemaine : string
    DateDebut : DateTime
    DateFin : DateTime
    

    + *idHoraire : int
}

class ConfirmationStagiaire {
    + id : int
    + ConfirmationPrésence : bool
    + CommentaireAbsence : string

    + *idPlageHoraire
    ' Entreprise : string
    ' Nom&PrénomStagiaire : string
    ' ConfirmationPrésence : bool
    ' CommentaireAbsence : string
    ' date : dateTime
    ' durée : int
    ' MatriculeTAP1 : string
    ' MatriculeTAP2 : string
}

class ConfirmationMDS {
    + id : int
    + ConfirmationPrésence : bool

    + *idPlageHoraire
    ' Entreprise : string
    ' Nom&PrénomStagiaire : string
    ' ConfirmationPrésence : bool
    ' CommentaireAbsence : string
    ' date : dateTime
    ' durée : int
    ' MatriculeTAP1 : string
    ' MatriculeTAP2 : string
}

class Contract {
    + id : int
    + Forms : Forms

    + *idStagiaire : int
    + *idMds : int
}

class Stage {
    - idStage : int
    - milieuStage : string
    - titre : string
    - fonction : string 
    - signataire : string
    - secteur : string
    - program : string
    - typeStage : string
    - dateDebutStage : dateTime
    - dateFinStage : dateTime
    - superviseurCollège : Enseignant
    - poste : string

    - *idEntreprise : int
    - *idMds : int
    - *idStagiaire : int
}

class StageMds {
    - *idMds
    - *idStagiaire
}




' class TypeEmployeur {
'     TypeEmployeur : Enum
' }

' class Statut {
'     Enum
' }

' class Statut {
'     Enum
' }


'Chat enseignant plusieurs'
'Chat stagiaire 1 chat
'Chat mds plusieurs

chat "1" -- "1..*" message

chat "1" -- "1" stagiaire
chat "1" -- "1" Mds
chat "1" -- "1..*" enseignant


Stage "1..*" -- "1" ent
Stage "1" -- "1..*" Mds
Stage "1..*" -- "1" StageMds
Mds "1" -- "1" StageMds
Stage "1" -- "1" stagiaire

Horaire "1" --- "0..*" PlageHoraire

Mds "0..*" -- "1" EntrepriseMDS
EntrepriseMDS "1" -- "0..*" ent

Mds "1..*" -- "1" stagiaire

enseignant "1" --- "0..*" stagiaire


' Evaluation "1" -- "1" Mds 
' Evaluation "1" -- "1" stagiaire

' AutoEvaluation "1" -- "1" stagiaire 
' AutoEvaluation"1" -- "1" Mds

Contract "1" -- "1" stagiaire
Mds "1" -- "1..*" Contract

Horaire "1" -- "1" stagiaire
Horaire "1..*" -- "1" Mds

PlageHoraire "1" -- "1" ConfirmationStagiaire
PlageHoraire "1" -- "1" ConfirmationMDS

Evaluation "1" -- "1" Mds
Evaluation "1..*" -- "1" stagiaire


```

