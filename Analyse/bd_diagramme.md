```plantuml

hide circle

class "Coordonateur" AS co {
    + id : Guid 
    + nom : string
    + prenom : string
    + courriel : string 
    + telephone : string

    + *ecoleId : Guid
    + *idutilisateur : Guid 
}

class "Enseignant" AS enseignant {
    + id : Guid
    + nom : string
    + prenom : string
    + courriel : string
    + telephone : string

    + *ecoleId : Guid
    + *idutilisateur : Guid 
}

class "Stagiaire" AS stagiaire {
    + id : Guid 
    + nom : string
    + prenom : string
    + courriel : string
    + telephone : int

    + *chatId : Guid
    + *employeurId : Guid
    + *idEnseignant : Guid
    + *ecoleId : Guid
    + *idutilisateur : Guid 
}

class "Ecole" AS ecole{
    + id : Guid
    + Nom : string
    + NumDeTel : string
    
    + *AdresseId : Guid
}


class "Message" AS message {
    + id : Guid
    + message : string
    + date&heure : datetime
    + *idutilisateur : Guid
    + *chatId : Guid
}

class "Chat" as chat {
    + id : Guid

    + *idCoordonateur: Guid
    + *idEnseignant : Guid
}


class "MDS" AS Mds {
    + id : Guid
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

    ' A verifié ici, on est des enums

    + *idutilisateur : Guid
    + *idChat : Guid
    + *idStagiaire : Guid
    + *idStagiaire : Guid
    + *idEmployeur : Guid
}

class Notifications {
   + id : Guid

}

' Un Employeur peut avoir plusieurs maitre de stage
' Un maitre de stage peut avoir plusieurs Employeur

class EmployeurMDS {
    + *idEntreprise : Guid
    + *idMDS : Guid
}

class "Employeur" AS ent {
    + id : Guid
    + nom : string
    + adresse : string
    + telephone : string
    + *adresseid : Guid
    + *idutilisateur : Guid
}

class Horaire {
    + id : Guid

    + *idMds : Guid
    + *idStagiaire : Guid
}

class Evaluation {
    + id : Guid
    + LienGoogleForms : string
    + EstStagiaire : bool
    + Actif : bool
    + Consulte : bool
    

    + *idStagiaire : Guid
    + *idMds : Guid
}



class PlageHoraire {
    id : Guid
    ' heureDebut : int
    ' minutesDebut : int
    ' heureFin : int
    ' minutesFin : int
    ' journeeSemaine : string
    DateDebut : DateTime
    DateFin : DateTime
    ConfirmationPresence : bool 
    Commentaire : string

    + *idHoraire : Guid
}

' class ConfirmationTemps {
'     + id : Guid
'     + ConfirmationPrésence : bool
'     + CommentaireAbsence : string
'     + EstStagiaire : bool

'     + *idPlageHoraire
'     ' Entreprise : string
'     ' Nom&PrénomStagiaire : string
'     ' ConfirmationPrésence : bool
'     ' CommentaireAbsence : string
'     ' date : dateTime
'     ' durée : int
'     ' MatriculeTAP1 : string
'     ' MatriculeTAP2 : string
' }

class Contract {
    + id : Guid
    + Forms : Forms

    + *idStagiaire : Guid
    + *idMds : Guid
}

class Stage {
    - idStage : Guid
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

    - *idEntreprise : Guid
    - *idMds : Guid
    - *idStagiaire : Guid
}

class StageMds {
    - *idMds : Guid
    - *idStagiaire : Guid
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

PlageHoraire "1" -- "1..2" ConfirmationTemps

Evaluation "1" -- "1" Mds
Evaluation "1..*" -- "1" stagiaire


```

